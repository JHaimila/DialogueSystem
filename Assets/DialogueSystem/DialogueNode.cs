using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace  DialogueSystem
{
    public abstract class DialogueNode : ScriptableObject
    {
        
        internal List<DialogueNode> connections;
        
        [SerializeField] internal string guid;
        
        private Vector2 nodePosition;
        public event System.Action ValueChanged;
        
        public List<DialogueNode> GetConnections()
        {
            List<DialogueNode> returnConnections = new List<DialogueNode>();
            foreach(var connection in connections)
            {
                if(connection.GetType() == typeof(ActionNode))
                {
                    // returnConnections.Add((connection as ActionNode).PerformActions());
                }
            }
            return returnConnections;
        }

        public virtual void SetDefault()
        {
            connections = new List<DialogueNode>();
            guid = GUID.Generate().ToString();
            name = guid;
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
        private void OnValidate() {
            ValueChanged?.Invoke();
        }
    }
}

