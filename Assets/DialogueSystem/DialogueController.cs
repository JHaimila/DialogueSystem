using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Conditions;
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

        private List<ICondition> conditionals;

        private void Start() {
            GetConditionals();
        }

        public void StartDialogue(Dialogue givenDialogue, NPCSpeaker speaker)
        {
            if(IsActive()){return;}
            
            currentDialogue = givenDialogue;
            currentNode = givenDialogue.GetNodes()[0];
            npcSpeaker = speaker;
            PerformAction(currentNode.GetEnterAction());

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
                PerformAction(currentNode.GetExitAction());
            }
            else
            {
                isChoosing = false;
                // TODO add support for branching on non player choice nodes
                PerformAction(currentNode.GetExitAction());
                currentNode = nextNodes[0];
                PerformAction(currentNode.GetEnterAction());
            }
            OnNodeChanged?.Invoke();
        }
        public void OnChoiceSelected(DialogueNode choice)
        {
            currentNode = choice;
            OnNodeChanged?.Invoke();
            PerformAction(choice.GetEnterAction());
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
            PerformAction(currentNode.GetExitAction());
            currentDialogue = null;
            currentNode = null;
            OnDialogueEnded?.Invoke();
        }
        private void PerformAction(string actionRef)
        {
            
            if(String.IsNullOrEmpty(actionRef)){return;}
            npcSpeaker.GetComponent<ActionExecutioner>().PerformAction(actionRef);
        }
        private List<DialogueNode> FilterOnCondition(List<DialogueNode> checkNodes)
        {
            if(conditionals.Count == 0)
            {
                GetConditionals();
            }
            List<DialogueNode> returnNodes = new List<DialogueNode>();
            foreach(var node in checkNodes)
            {
                if(Conditions.ConditionChecker.CheckConditions(node.GetConditions(), conditionals))
                {
                    returnNodes.Add(node);
                }
            }
            return returnNodes;
        }
        private void GetConditionals()
        {
            conditionals = new List<ICondition>();
            ICondition[] tConditions = GetComponents<ICondition>();
            foreach(var tcondition in tConditions)
            {
                conditionals.Add(tcondition);
            }
        }
    }
}