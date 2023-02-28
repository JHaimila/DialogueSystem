using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;

namespace DialogueSystem.Editor
{
    public class DialogueNodeView : Node
    {
        public DialogueNode node;
        public Action<DialogueNodeView> OnNodeSelected;
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
            CreateOutputPorts();
        }

        private void CreateInputPorts()
        {
            inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));

            if(inputPort != null)
            {
                inputPort.portName = "";
                inputContainer.Add(inputPort);
            }
        }
        private void CreateOutputPorts()
        {
            outputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(bool));

            if(outputPort != null)
            {
                outputPort.portName = "";
                outputContainer.Add(outputPort);
            }
        }

        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);

            node.SetPosition(new Vector2(newPos.xMin,newPos.yMin));

        }

        public override void OnSelected()
        {
            base.OnSelected();
            OnNodeSelected?.Invoke(this);
        }
    }
}