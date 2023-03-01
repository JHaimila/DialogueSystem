using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Conditions;

namespace  DialogueSystem
{
    public class DialogueNode : ScriptableObject
    {
        
        [SerializeField] internal List<DialogueNode> connections;
        
        [SerializeField] internal string guid;
        
        private Vector2 nodePosition;
        public event System.Action ValueChanged;
        [SerializeField, TextArea(3, 10)] private string line;
        [SerializeField] private Speaker speaker;
        [SerializeField] private bool playerChoice;
        [SerializeField] private List<Condition> conditions;
        [SerializeField] private string enterAction;
        [SerializeField] private string exitAction;

        public void SetDefault()
        {
            line = "[Unwritten Dialogue]";
            guid = System.Guid.NewGuid().ToString();
            name = guid;
            connections = new List<DialogueNode>();
        }
        public string GetLine()
        {
            return line;
        }
        public Speaker GetSpeaker()
        {
            return speaker;
        }
        public bool IsPlayerChoice()
        {
            return playerChoice;
        }
        
        public List<DialogueNode> GetConnections()
        {
            return connections;
        }
        
        public string GetID()
        {
            return guid;
        }
        public Vector2 GetPosition()
        {
            return nodePosition;
        }
        public void SetPosition(Vector2 newPosition)
        {
            nodePosition = newPosition;
        }
        public string GetEnterAction()
        {
            return enterAction;
        }
        public string GetExitAction()
        {
            return exitAction;
        }
        public List<Condition> GetConditions()
        {
            // TODO interface with the ConditionChecker to actually check the condition
            return conditions;
        }
        private void OnValidate() {
            ValueChanged?.Invoke();
        }
        
    }
}

