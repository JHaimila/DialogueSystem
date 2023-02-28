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
        private bool isChoosing;

        public event Action OnNodeChanged;
        public event Action OnDialogueStarted;
        public event Action OnDialogueEnded;

        public void StartDialogue(Dialogue givenDialogue, NPCSpeaker speaker)
        {
            if(IsActive()){return;}
            
            currentDialogue = givenDialogue;
            currentNode = givenDialogue.GetNodes()[0];
            npcSpeaker = speaker;

            OnDialogueStarted?.Invoke();
        }

        public void Next()
        {
            if(!HasNext())
            {
                EndDialogue();
                return;
            }

            List<DialogueNode> nextNodes = FilterOnCondition(currentNode.GetConnections());
            if(nextNodes[0].IsPlayerChoice())
            {
                isChoosing = true;
            }
            else
            {
                isChoosing = false;
                // TODO add support for branching on non player choice nodes
                currentNode = nextNodes[0];
            }
            OnNodeChanged?.Invoke();
        }
        public void OnChoiceSelected(DialogueNode choice)
        {
            currentNode = choice;
            OnNodeChanged?.Invoke();
            Next();
        }

        public bool HasNext()
        {
            return FilterOnCondition(currentNode.GetConnections()).Count > 0;
        }
        public DialogueNode GetCurrentNode()
        {
            return currentNode;
        }
        public bool IsChoosing()
        {
            return isChoosing;
        }
        public List<DialogueNode> GetChoices()
        {
            return FilterOnCondition(currentNode.GetConnections());
        }

        public bool IsActive()
        {
            return currentDialogue != null;
        }
        public void EndDialogue()
        {
            currentDialogue = null;
            currentNode = null;
            OnDialogueEnded?.Invoke();
        }
        private List<DialogueNode> FilterOnCondition(List<DialogueNode> checkNodes)
        {
            List<DialogueNode> returnNodes = new List<DialogueNode>();
            foreach(var node in checkNodes)
            {
                if(node.CheckCondition())
                {
                    returnNodes.Add(node);
                }
            }
            return returnNodes;
        }
    }
}