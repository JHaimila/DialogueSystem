using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace DialogueSystem
{
    public class LineNode : DialogueNode
    {
        [SerializeField, TextArea(3, 10)] private string line;
        [SerializeField] private Speaker speaker;
        [SerializeField] private bool playerChoice;

        public override void SetDefault()
        {
            base.SetDefault();
            line = "[Unwritten Dialogue]";
        }
        public string GetLine()
        {
            return line;
        }
        public Speaker GetSpeaker()
        {
            return speaker;
        }
        public bool IsPlayerChoice()
        {
            return playerChoice;
        }
    }

}
