using System;
using DialogueSystem.Editor;
using UnityEngine.UIElements;
using UnityEditor;
using System.Collections.Generic;
using DialogueSystem;

public class InspectorView : VisualElement
{
    public new class UxmlFactory:UxmlFactory<InspectorView, VisualElement.UxmlTraits>{}
    
    Editor editor;

    internal void UpdateSelection(DialogueNode[] nodeViews)
    {
        Clear();
        if(nodeViews.Length == 0){return;}

        UnityEngine.Object.DestroyImmediate(editor);

        editor = Editor.CreateEditor(nodeViews);
        IMGUIContainer container = new IMGUIContainer(delegate {editor.OnInspectorGUI();}); 
 
        Add(container);
    }
}
