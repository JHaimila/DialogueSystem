using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.GraphView;

namespace DialogueSystem.Editor
{
    public class DialogueNodeView : Node
    {
        public DialogueNode node;
        public List<DialogueNode> nodes;
        public Action OnNodeSelected;
        public Action OnNodeDeselected;
        public Port inputPort;
        public Port outputPort;
        public DialogueNodeView(DialogueNode node)
        {
            this.node = node;
            this.title = node.GetLine();
            
            this.viewDataKey = node.GetID();
            style.left = node.GetPosition().x;
            style.top = node.GetPosition().y;

            CreateInputPorts();
            CreateOutputPorts("");
        }
        public DialogueNodeView(List<DialogueNode> nodes)
        {
            this.nodes = nodes;
            this.title = "Player Choice";

            CreateInputPorts();
            foreach(var node in nodes)
            {
                Port tPort = CreateOutputPorts(node.GetLine());
            }
        }

        private void CreateInputPorts()
        {
            inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));

            if(inputPort != null)
            {
                inputPort.portName = node.GetLine();
                inputContainer.Add(inputPort);
            }
        }
        private Port CreateOutputPorts(string portName)
        {
            outputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(bool));

            if(outputPort != null)
            {
                outputPort.portName = portName;
                outputContainer.Add(outputPort);
            }
            return outputPort;
        }

        

        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);
            node.SetPosition(new Vector2(newPos.xMin,newPos.yMin));
            EditorUtility.SetDirty(node);
        }
        public override void OnSelected()
        {
            base.OnSelected();
            OnNodeSelected?.Invoke();
        }
        public override void OnUnselected()
        {
            base.OnUnselected();
            OnNodeDeselected?.Invoke(); 
        }
    }
}