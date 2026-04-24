using TMPro;
using UnityEngine;
namespace JPA_DialogueSystem.UI
{
    public class UIDialogueHandler : MonoBehaviour
    {

        [Header("Panels")]
        [Tooltip("Reference to the Parent that contains all dialogue elements. ")]
        [SerializeField]
        private GameObject u_dialoguePanel;
        [Header("Text Display")]
        [SerializeField]
        [Tooltip("Reference to TextMeshPro that contains the contents of the dialogue. ")]
        private TextMeshProUGUI u_textDisplay;
        [SerializeField]
        [Tooltip("Reference to TextMeshPro that contains the name of the dialogue. (Optional)")]
        private TextMeshProUGUI u_nameDisplay;

        private int _charactersInSentence = 0;

        public bool IsDialoguePanelActive
        {
            get => u_dialoguePanel.activeInHierarchy;
            set => u_dialoguePanel.SetActive(value);
        }
        public bool HasTextDisplay => u_textDisplay != null;
        public bool HasNameDisplay => u_nameDisplay != null;
        public string CurrentSentence
        {
            get { if (u_textDisplay != null) return u_textDisplay.text; else return ""; }
            set
            {
                if (u_textDisplay != null)
                {
                    u_textDisplay.text = value;
                    _charactersInSentence = value.Length;
                }
                else
                {
                    Debug.LogWarning("Failed Trying to Write a Sentence as there's no Dialogue Display Assigned to UIDialogueHandler. ");
                }
            }
        }
        public string CurrentName
        {
            get
            {
                if (u_nameDisplay != null)
                    return u_nameDisplay.text;
                else return "";
            }
            set
            {
                if (u_nameDisplay != null)
                {
                    u_nameDisplay.text = value;
                }
                else
                {
                    Debug.LogWarning("Failed Trying to Write a Name as there's no Name Display Assigned to UIDialogueHandler. ");
                }
            }
        }
        public int CharactersInCurrentSentence => _charactersInSentence;
        public int MaxVisibleCharacters
        {
            get { return u_textDisplay.maxVisibleCharacters; }
            set
            {
                u_textDisplay.maxVisibleCharacters = value;
            }
        }
        void OnDisable()
        {
            IsDialoguePanelActive = false;
        }
    }
}