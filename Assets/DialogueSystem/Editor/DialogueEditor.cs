using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.Collections.Generic;

namespace DialogueSystem.Editor
{
    public class DialogueEditor : EditorWindow
    {
        GridView gridView;
        InspectorView inspectorView;
        private Button refreshButton;
        private Button saveButton;
        private Button focusButton;

        [MenuItem("Window/UI Toolkit/DialogueEditor")]
        public static void ShowExample()
        {
            DialogueEditor wnd = GetWindow<DialogueEditor>();
            wnd.titleContent = new GUIContent("Dialogue Editor");
        }

        public void CreateGUI()
        {
            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;

            // Import UXML
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/DialogueSystem/Editor/DialogueEditor.uxml");
            VisualElement labelFromUXML = visualTree.Instantiate();
            root.Add(labelFromUXML);

            // A stylesheet can be added to a VisualElement.
            // The style will be applied to the VisualElement and all of its children.
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/DialogueSystem/Editor/DialogueEditor.uss");
            root.styleSheets.Add(styleSheet);

            gridView = root.Q<GridView>();

            inspectorView = root.Q<InspectorView>();

            refreshButton = root.Q<Button>("refreshBtn");
            refreshButton.clicked += OnSelectionChange;

            saveButton = root.Q<Button>("saveBtn");
            saveButton.clicked += SaveDialogue;

            focusButton = root.Q<Button>("focusBtn");
            focusButton.clicked += FocusContent;

            OnSelectionChange();
            gridView.OnNodeSelected += OnNodeSelectionChanged;
            gridView.OnNodeDeselected += ClearInspector;

        }
        private void OnSelectionChange() 
        {
            SaveDialogue();
            Dialogue dialogue = Selection.activeObject as Dialogue;
            if(dialogue)
            {
                gridView.PopulateView(dialogue);
            }
        }
        private void SaveDialogue()
        {
            gridView.SaveDialogue();
        }
        private void FocusContent()
        {
            gridView.FrameAll();
        }

        void OnNodeSelectionChanged()
        {
            
            List<DialogueNode> giveNodes = new List<DialogueNode>();
            foreach(var select in gridView.selection)
            {
                DialogueNodeView tnode = select as DialogueNodeView;
                if(tnode != null)
                {
                    giveNodes.Add(tnode.node);
                }
            }
            inspectorView.UpdateSelection(giveNodes.ToArray());
            SaveDialogue();
        }
        private void ClearInspector()
        {
            if(gridView.selection.Count > 0)
            {
                inspectorView.Clear();
            }
        }
        void OnNodeDeselected()
        {
            OnNodeSelectionChanged();
        }
    }
}
