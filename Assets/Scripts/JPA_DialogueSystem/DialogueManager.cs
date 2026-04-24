using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using JPA_DialogueSystem.UI;
using JPA_DialogueSystem.Utils;
using JPA_DialogueSystem.Input;
using JPA_DialogueSystem.Enums;
/// 
/// Uses a ScriptableStory to output dialogue.
/// Does not support branching dialogue. (See Ink module for this)
/// ///
namespace JPA_DialogueSystem
{
    public class DialogueManager : IPausable
    {
        [Header("Dialogue Settings")]
        [SerializeField]
        protected float _timeBetweenSentences = 2.0f;
        [SerializeField]
        protected bool _canFastfowardText = false;
        [Header("Dependencies")]
        [SerializeField]
        protected UIDialogueHandler _dialogueHandler;
        [SerializeField]
        protected NullableObject<UIPortaitHandler> _portraitHandlerUI;
        [SerializeField]
        [Tooltip("Optional Input Handler. ")]
        protected NullableObject<DialogueInputHandler> _dialogueInput;
        protected string currSentence = "";
        protected bool _isDialogueRunning = false;
        protected bool isCurrentDialInterrupted = false;
        protected bool _isStoryRunning;
        private ScriptableStory currentStory;
        protected bool isInteractable = true;
        protected IEnumerator dialogueCoroutine;
        public bool CanFastfowardText => _canFastfowardText;
        public bool IsStoryRunning => _isStoryRunning;
        public bool IsDialogueRunning => _isDialogueRunning;

        public static event Action StoryEnd;
        public static event Action<bool> DialogueLineEnd;
        public static event Action LetterIsDiscovered;
        public UIDialogueHandler UIHandler { private get; set; }
        public DialogueInputHandler InputHandler { private get; set; }
        public UIPortaitHandler PortaitHandler { private get; set; }

        private static WaitForSeconds _waitForSeconds0_1 = new(0.1f);



        protected virtual void Awake()
        {
            if (_dialogueHandler == null)
            {
                _dialogueHandler = GetComponent<UIDialogueHandler>();
            }
            if (_dialogueInput.IsNull)
            {
                _dialogueInput.value = GetComponent<DialogueInputHandler>();
            }
        }
        protected override void OnEnable()
        {
            base.OnEnable();
            if (!_dialogueInput.IsNull)
            {
                _dialogueInput.value.enabled = false;
            }
            _isStoryRunning = false;
            _dialogueHandler.IsDialoguePanelActive = false;
        }
        public virtual void StartStory(ScriptableStory scriptableStory)
        {
            scriptableStory.Reset();
            StopAllCoroutines(); //From, for example, a bark.
            _dialogueHandler.IsDialoguePanelActive = true;
            currentStory = scriptableStory;
            isInteractable = currentStory.IsInteractable;
            _isStoryRunning = true;
            if (!_dialogueInput.IsNull)
            {
                _dialogueInput.value.enabled = isInteractable;
            }
            ContinueStory();
        }
        public virtual void DoBark(string[] bark, bool isInteractable, string name = "???", int portraitTag = -1, int startBarkIndex = 0, float barkSpeed = 0.01f)
        {
            if (barkSpeed <= 0) barkSpeed = 0.01f; //0.01 is the default!
            if (!_isStoryRunning)
            {
                this.isInteractable = isInteractable;
                _dialogueHandler.IsDialoguePanelActive = true;
                _isStoryRunning = true;
            }
            else
            {
                startBarkIndex++;
            }
            if (startBarkIndex >= bark.Length)
            {
                ExitDialogue(false);
                return;
            }
            _dialogueHandler.CurrentName = name;
            currDialogueSpeed = barkSpeed;
            dialogueCoroutine = WriteDialogue(bark[0], () =>
            {
                DoBark(bark, isInteractable, name, portraitTag, startBarkIndex);
            }, true);
            StartCoroutine(dialogueCoroutine);
        }
        public void ExitDialogue(bool isScriptableStory)
        {
            if (!_dialogueInput.IsNull)
            {
                _dialogueInput.value.enabled = false;
            }
            StopAllCoroutines();
            _dialogueHandler.IsDialoguePanelActive = false;
            isInteractable = false;
            _isStoryRunning = false;
            if (isScriptableStory) StoryEnd?.Invoke();
        }
        public virtual void ContinueStory()
        {
            if (currentStory == null) return;
            if (currentStory.CanContinue)
            {
                if (_dialogueHandler.HasTextDisplay)
                {
                    currSentence = currentStory.Continue();
                    _dialogueHandler.CurrentName = currentStory.GetName();
                    dialogueCoroutine = WriteDialogue(currSentence, ContinueStory, false);
                    StartCoroutine(dialogueCoroutine);
                }
                else Debug.LogWarning("No Text Display Assigned to DialogueHandlerUI");
            }
            else
            {
                currentStory.Reset();
                ExitDialogue(true);
            }
        }

        protected int wordCounter = 0;
        protected float currDialogueSpeed = 0.2f;
        protected bool isFirstLetter = true;

        IEnumerator WriteDialogue(String sentence, Action continueAction, bool isBark)
        {
            bool isPaused = false;
            int letterCounter = 0;
            _isDialogueRunning = true;
            isFirstLetter = true;
            wordCounter = 0;
            _dialogueHandler.CurrentSentence = sentence;
            _dialogueHandler.MaxVisibleCharacters = 0;
            char[] arraySentence = sentence.ToCharArray();
            List<ScriptableStory.DialogueModifier> modifiers;
            if (!isBark) modifiers = new(currentStory.GetModifiers());
            else
            {
                modifiers = new();
            }
            foreach (char letter in arraySentence)
            {
                while (isGamePaused)
                {
                    yield return null;
                }

                _canFastfowardText = true;
                float pauseTimer = 0;
                if (isPaused)
                {
                    isPaused = false;
                    Talk(true);
                }
                if (Char.IsWhiteSpace(letter) && (letterCounter + 1 < arraySentence.Length))
                {
                    if (!Char.IsWhiteSpace(arraySentence[letterCounter + 1]))
                    {
                        isFirstLetter = true;
                        wordCounter++;
                    }
                }
                if (isFirstLetter)
                {
                    isFirstLetter = false;
                    // Parse Modifiers for this sentence.
                    foreach (ScriptableStory.DialogueModifier modifier in modifiers)
                    {
                        if (modifier.triggerOrder == wordCounter)
                        {
                            switch (modifier.type)
                            {
                                case Enums.DialogueModifierType.CHANGEPORTAIT:
                                    SetPortrait((int)modifier.value);
                                    break;
                                case Enums.DialogueModifierType.SETSPEED:
                                    currDialogueSpeed = modifier.value;
                                    break;
                                case Enums.DialogueModifierType.PAUSE:
                                    pauseTimer = modifier.value;
                                    isPaused = true;
                                    break;
                                case Enums.DialogueModifierType.INTERRUPT:
                                    isCurrentDialInterrupted = true;
                                    break;
                            }
                        }
                    }
                }
                if (isPaused) Talk(false);
                else Talk(true);
                _dialogueHandler.MaxVisibleCharacters++;
                LetterIsDiscovered?.Invoke();
                yield return new WaitForSeconds(currDialogueSpeed + pauseTimer);
                letterCounter++;

            }
            Talk(false);
            _isDialogueRunning = false;
            if (!isInteractable)
            {
                yield return new WaitForSeconds(_timeBetweenSentences);
                continueAction?.Invoke();
            }
            if (isCurrentDialInterrupted)
            {
                isCurrentDialInterrupted = false;
                yield return _waitForSeconds0_1;
                continueAction?.Invoke();
            }
            _canFastfowardText = false;
            DialogueLineEnd?.Invoke(false); //Bool is: Did it happen on user fast-fowarding the text?
        }
        public void SkipDialogueCoroutine()
        {
            if (_isDialogueRunning && !isCurrentDialInterrupted)
            {
                _canFastfowardText = false;
                _isDialogueRunning = false;
                StopCoroutine(dialogueCoroutine);
                _dialogueHandler.MaxVisibleCharacters = _dialogueHandler.CharactersInCurrentSentence;
                isCurrentDialInterrupted = false;
                Talk(false);
                DialogueLineEnd?.Invoke(true);
            }
        }
        protected void Talk(bool talk)
        {
            if (!_portraitHandlerUI.IsNull) _portraitHandlerUI.value.SetTalkingAnimation(talk);
        }
        protected void SetPortrait(string name, int layer = 0)
        {
            if (!_portraitHandlerUI.IsNull)
                _portraitHandlerUI.value.SetDialoguePortrait(name, layer);
            else
                Debug.LogWarning("There's no portait handler assigned to DialogueManager. Skipping Tag.");
        }
        protected void SetPortrait(int hash, int layer = 0)
        {
            if (!_portraitHandlerUI.IsNull)
                _portraitHandlerUI.value.SetDialoguePortrait(hash, layer);
            else
                Debug.LogWarning("There's no portait handler assigned to DialogueManager. Skipping Tag.");
        }
    }
}
