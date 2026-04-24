using UnityEngine;

namespace JPA_DialogueSystem.Input
{
    [RequireComponent(typeof(DialogueManager))]
    public class DialogueInputHandler : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField]
        private DialogueManager _dialogueManager;
        public enum DialogueInputType
        {
            DIALOGUE_FOWARD,
            DIALOGUE_SKIP
        }
        void Awake()
        {
            if (_dialogueManager == null)
            {
                _dialogueManager = GetComponent<DialogueManager>();
                if (_dialogueManager == null)
                {
                    Debug.LogError("No Dialogue Manager was assigned or is part of GameObject for DialogueInputHandler. Disabling.");
                    this.enabled = false;
                }
            }
        }

        public void HandleDialogue(DialogueInputType dialogueInputType)
        {
            if (dialogueInputType == DialogueInputType.DIALOGUE_FOWARD)
            {
                if (!_dialogueManager.CanFastfowardText && !_dialogueManager.IsDialogueRunning)
                {
                    _dialogueManager.ContinueStory();
                }
                else if (_dialogueManager.CanFastfowardText && _dialogueManager.IsDialogueRunning)
                {
                    _dialogueManager.SkipDialogueCoroutine();
                }
            }
            if (dialogueInputType == DialogueInputType.DIALOGUE_SKIP)
            {
                _dialogueManager.SkipDialogueCoroutine();
            }
        }
    }
}
