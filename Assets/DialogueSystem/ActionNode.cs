using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem
{
    public class ActionNode : DialogueNode
    {
        public List<DialogueNode> PerformActions()
        {
            return GetConnections();
        }
    }
}