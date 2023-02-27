using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem
{
    public class ConditionNode : DialogueNode
    {
        public List<DialogueNode> Check()
        {
            return GetConnections();
        }
    }
}