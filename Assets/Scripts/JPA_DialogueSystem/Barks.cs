using UnityEngine;

namespace JPA_DialogueSystem.Utils
{
    [CreateAssetMenu(fileName = "Barks", menuName = "StoryObjects/Barks", order = 1)]
    public class Barks : ScriptableObject
    {
        public string[] barks;
        public string dialogueName;
        public float dialogueSpeed;
    }

}