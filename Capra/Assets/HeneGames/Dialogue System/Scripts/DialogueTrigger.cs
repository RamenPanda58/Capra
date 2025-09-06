using System.Collections.Generic;
using UnityEngine;

namespace HeneGames.DialogueSystem
{
    public class DialogueTrigger : MonoBehaviour
    {
        [Header("Dialogue Variants")]
        [SerializeField] private List<NPC_Centence> dialoguePart1; // default
        [SerializeField] private List<NPC_Centence> dialoguePart2; // after quest

        private bool usePart2 = false;

        public UnityEngine.Events.UnityEvent startDialogueEvent;
        public UnityEngine.Events.UnityEvent nextSentenceDialogueEvent;
        public UnityEngine.Events.UnityEvent endDialogueEvent;

        // Called by DialogueManager to get the current dialogue
        public List<NPC_Centence> GetDialogue()
        {
            return usePart2 ? dialoguePart2 : dialoguePart1;
        }

        // Called when a quest finishes
        public void SwitchToPart2()
        {
            usePart2 = true;
        }
    }
}
