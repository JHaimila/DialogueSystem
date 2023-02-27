using System;
using DialogueSystem.Editor;
using UnityEngine.UIElements;
using UnityEditor;
public class InspectorView : VisualElement
{
    public new class UxmlFactory:UxmlFactory<InspectorView, VisualElement.UxmlTraits>{}
    
    Editor editor;

    internal void UpdateSelection(DialogueNodeView nodeView)
    {
        Clear();

        UnityEngine.Object.DestroyImmediate(editor);

        editor = Editor.CreateEditor(nodeView.node);
        IMGUIContainer container = new IMGUIContainer(delegate {editor.OnInspectorGUI();});

        Add(container);
    }
}
