using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem
{
    public class DialogueController : MonoBehaviour
    {
        private Dialogue currentDialogue;
        private DialogueNode currentNode;
        private NPCSpeaker npcSpeaker;

        public event Action OnNodeChanged;
        public event Action OnDialogueStarted;
        public event Action OnDialogueEnded;

        public void StartDialogue(Dialogue givenDialogue, NPCSpeaker speaker)
        {
            currentDialogue = givenDialogue;
            currentNode = givenDialogue.GetNodes()[0];
            npcSpeaker = speaker;

            OnNodeChanged?.Invoke();
        }

        public void Next()
        {
            // TODO Filter based on conditions
            ;
        }
        public void OnChoiceSelected(DialogueNode choice)
        {
            currentNode = choice;
            OnNodeChanged?.Invoke();
        }

        public bool HasNext()
        {
            // TODO Filter based on conditions
            return currentNode.GetConnections().Count > 0;
        }
        // public bool IsChoosing()
        // {
        //     // TODO create a function to filter based on condition && check all of the connecting nodes. Not all are going to be choice
        //     return currentNode.GetConnections()[0].IsPlayerChoice();
        // }

        public bool IsActive()
        {
            return currentDialogue != null;
        }
        // public Speaker GetSpeaker()
        // {
        //     return currentNode.GetSpeaker();
        // }
        // public string GetLine()
        // {
        //     if(!currentDialogue)
        //     {
        //         return "";
        //     }
        //     return currentNode.GetLine();
        // }
    }
}