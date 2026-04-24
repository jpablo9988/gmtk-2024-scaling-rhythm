using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using JPA_DialogueSystem.Enums;
using UnityEngine;

namespace JPA_DialogueSystem.Utils
{
    [CreateAssetMenu(fileName = "ScriptableStory", menuName = "StoryObjects/ScriptableStory", order = 0)]
    public class ScriptableStory : ScriptableObject
    {
        [Serializable]
        public struct DialogueModifier
        {
            public DialogueModifierType type;
            public float value;
            public float triggerOrder;
            public DialogueModifier(DialogueModifierType type, float value, float triggerOrder)
            {
                this.type = type;
                this.value = value;
                this.triggerOrder = triggerOrder;
            }
        }
        [Serializable]
        internal struct DialogueLine
        {
            public string name;
            public string sentence;
            public List<DialogueModifier> modifiers;
        }
        [SerializeField]
        private string _name;
        [SerializeField]
        private bool _isInteractable;
        [SerializeField]
        private List<DialogueLine> _lines;

        public string Name => _name;
        /// <summary>
        /// Does this story use any controls, or will it advance automatically. 
        /// </summary>
        public bool IsInteractable => _isInteractable;
        private int currentLine = -1;
        /// <summary>
        /// Checks if 
        /// </summary>
        public bool CanContinue
        {
            get
            {
                return currentLine < _lines.Count - 1;
            }
        }
        /// <summary>
        /// Gets the next line in the story. If there's no lines left, it will throw an error and return an empty string.
        /// Before using this line, be sure to check this function with CanContinue.
        /// </summary>
        /// <returns></returns>
        public string Continue()
        {
            currentLine++;
            if (currentLine >= _lines.Count || currentLine < 0)
            {
                Debug.LogError("Story overflow, please check if this story can continue before using this method.");
                return "This shouldn't be happening...!";
            }
            return _lines[currentLine].sentence;
        }
        public void Reset()
        {
            currentLine = -1;
        }
        /// <summary>
        /// Returns the name assigned to the current line of dialogue. 
        /// </summary>
        /// <returns></returns>
        public string GetName()
        {
            return _lines[currentLine].name;
        }
        /// <summary>
        /// Gets the modifiers affecting the current line of dialogue.
        /// </summary>
        /// <returns></returns>
        public List<DialogueModifier> GetModifiers()
        {
            return _lines[currentLine].modifiers;
        }
    }
}

