using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEditor;
using System;
using System.Linq;
using System.Collections.Generic;

namespace DialogueSystem.Editor
{
    public class GridView : GraphView
    {
        public new class UxmlFactory:UxmlFactory<GridView, GraphView.UxmlTraits>{}

        Dialogue dialogue;
        public Action<DialogueNodeView> OnNodeSelected;

        public GridView()
        {
            Insert(0, new GridBackground());
        
            //This is how you add in the manipulation to the grid
            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
        
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/DialogueSystem/Editor/DialogueEditor.uss");
            styleSheets.Add(styleSheet);
        }
        private void PopulateView()
        {
            PopulateView(dialogue);
        }
        internal void PopulateView(Dialogue selectedDialogue)
        {
            if(dialogue)
            {
                foreach(var node in dialogue.GetNodes())
                {
                    node.ValueChanged -= PopulateView;
                }
            }
            
            dialogue = selectedDialogue;


            graphViewChanged -= OnGraphViewChanged;
            DeleteElements(graphElements);
            graphViewChanged += OnGraphViewChanged;

            // Creates node views in graph
            dialogue.GetNodes().ForEach(n => CreateNodeView(n));

            //Creates the lines (edges)
            dialogue.GetNodes().ForEach(n => {
                var children = n.GetConnections();
                children.ForEach(c =>{
                    DialogueNodeView parentView =  FindNodeView(n);
                    DialogueNodeView childView =  FindNodeView(c);
                    
                    AddElement(parentView.outputPort.ConnectTo(childView.inputPort));
                });
            });
        }

        private DialogueNodeView FindNodeView(DialogueNode node)
        {
            return GetNodeByGuid(node.GetID()) as DialogueNodeView;
        }

        private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
        {
            if(graphViewChange.elementsToRemove != null)
            {
                graphViewChange.elementsToRemove.ForEach(elem => {
                    DialogueNodeView nodeView = elem as DialogueNodeView;
                    if(nodeView != null)
                    {
                        dialogue.DeleteNode(nodeView.node);
                    }
                    Edge edge = elem as Edge;
                    if(edge != null)
                    {
                        DialogueNodeView parentView = edge.output.node as DialogueNodeView;
                        DialogueNodeView childView = edge.input.node as DialogueNodeView;
                        dialogue.RemoveChild(parentView.node, childView.node);
                    }
                });
            }
            if(graphViewChange.edgesToCreate != null)
            {
                graphViewChange.edgesToCreate.ForEach(edge => {
                    DialogueNodeView parentView = edge.output.node as DialogueNodeView;
                    DialogueNodeView childView = edge.input.node as DialogueNodeView;
                    dialogue.AddChild(parentView.node, childView.node);
                });
            }
            return graphViewChange;
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            // base.BuildContextualMenu(evt);
            var types = TypeCache.GetTypesDerivedFrom<DialogueNode>();
            foreach(var type in types)
            {
                evt.menu.AppendAction($"{type.Name}", delegate{CreateNode(type);});
            }
            
        }
        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            return ports.ToList().Where(endPort => 
            endPort.direction != startPort.direction &&
            endPort.node != startPort.node).ToList();
        }
        
        private void CreateNode(System.Type type)
        {
            CreateNodeView(dialogue.AddNode(type));
        }

        private void CreateNodeView(DialogueNode node)
        {
            DialogueNodeView nodeView = new DialogueNodeView(node);
            nodeView.OnNodeSelected = OnNodeSelected;
            nodeView.node.ValueChanged += PopulateView;
            AddElement(nodeView);
        }
    }
}