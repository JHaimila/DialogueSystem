using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace DialogueSystem.Editor
{
    public class DialogueEditor : EditorWindow
    {
        GridView gridView;
        InspectorView inspectorView;
        private Button refreshButton;

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

            OnSelectionChange();
            gridView.OnNodeSelected += OnNodeSelectionChanged;

        }
        private void OnSelectionChange() 
        {
            Dialogue dialogue = Selection.activeObject as Dialogue;
            if(dialogue)
            {
                gridView.PopulateView(dialogue);
            }
        }

        void OnNodeSelectionChanged(DialogueNodeView node)
        {
            inspectorView.UpdateSelection(node);
        }
    }
}
