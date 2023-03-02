using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEditor;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem.Editor
{
    public class GridView : GraphView
    {
        public new class UxmlFactory:UxmlFactory<GridView, GraphView.UxmlTraits>{}

        Dialogue dialogue;
        public Action OnNodeSelected;
        public Action OnNodeDeselected;
        private List<DialogueNodeView> nodeViews;

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
            nodeViews = new List<DialogueNodeView>();
        }
        public void SaveDialogue()
        {
            if(dialogue == null){return;}

            EditorUtility.SetDirty(dialogue);
            foreach(var nodeView in nodeViews)
            {
                EditorUtility.SetDirty(nodeView.node);
            }
            AssetDatabase.SaveAssets();
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
            dialogue.GetNodes().ForEach(n => CreateNodeView(n, n.GetPosition()));

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
            VisualElement contentViewContainer = ElementAt(1);
            Vector3 screenMousePosition = evt.localMousePosition;
            Vector2 worldMousePosition = screenMousePosition - contentViewContainer.transform.position;
            worldMousePosition *= 1 / contentViewContainer.transform.scale.x;
            // base.BuildContextualMenu(evt);
            var types = TypeCache.GetTypesDerivedFrom<DialogueNode>();
            foreach(var type in types)
            {
                evt.menu.AppendAction($"{type.Name}", delegate{CreateNode(type, worldMousePosition);});
            }
            
        }
        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            return ports.ToList().Where(endPort => 
            endPort.direction != startPort.direction &&
            endPort.node != startPort.node).ToList();
        }
        
        
        private void CreateNode(System.Type type, Vector2 mousePosition)
        {
            CreateNodeView(dialogue.AddNode(type), mousePosition);
        }

        private void CreateNodeView(DialogueNode node, Vector2 mousePosition)
        {
            node.SetPosition(mousePosition);
            Debug.Log(node.GetPosition() +" "+ mousePosition);
            DialogueNodeView nodeView = new DialogueNodeView(node);
            nodeViews.Add(nodeView);
            Rect nodePosition = new Rect();
            nodePosition.position = node.GetPosition();
            nodeView.SetPosition(nodePosition);
            nodeView.OnNodeSelected = OnNodeSelected;
            nodeView.OnNodeDeselected = OnNodeDeselected;
            nodeView.node.ValueChanged += PopulateView;
            AddElement(nodeView);
        }
    }
}