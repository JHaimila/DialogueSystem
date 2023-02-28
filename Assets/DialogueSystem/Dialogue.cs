using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace DialogueSystem
{
    [CreateAssetMenu]
    public class Dialogue : ScriptableObject
    {
        [SerializeField] private List<DialogueNode> nodes = new List<DialogueNode>();
        [SerializeField] private bool repeatable;
        private bool played;

        
        
        public List<DialogueNode> GetNodes()
        {
            return nodes;
        }

        public DialogueNode AddNode(System.Type type)
        {
            DialogueNode dialogueNode = ScriptableObject.CreateInstance(type) as DialogueNode;
            dialogueNode.SetDefault();
            AssetDatabase.AddObjectToAsset(dialogueNode, this);
            nodes.Add(dialogueNode);
            AssetDatabase.SaveAssets();
            return dialogueNode;
        }
        public void DeleteNode(DialogueNode deleteNode)
        {
            if(!nodes.Contains(deleteNode)){return;}

            AssetDatabase.RemoveObjectFromAsset(deleteNode);
            nodes.Remove(deleteNode);
            AssetDatabase.SaveAssets();
        }
        public void AddChild(DialogueNode parent, DialogueNode child)
        {   
            if(parent.GetConnections().Contains(child)){return;}
            
            parent.GetConnections().Add(child);
        }
        public void RemoveChild(DialogueNode parent, DialogueNode child)
        {
            if(!parent.GetConnections().Contains(child)){return;}
            
            parent.GetConnections().Remove(child);
        }
    }
}