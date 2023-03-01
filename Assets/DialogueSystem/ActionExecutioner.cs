using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DialogueSystem
{
    public class ActionExecutioner : MonoBehaviour
    {
        [SerializeField] private List<ExecuteAction> actions;

        public void PerformAction(string reference)
        {
            foreach(var action in actions)
            {
                Debug.Log(action.reference+" : "+reference);
                if(action.reference == reference)
                {
                    action.triggers.Invoke();
                    return;
                }
            }
        }
    }
    [System.Serializable]
    public class ExecuteAction
    {
        public string reference;
        public UnityEvent triggers;
    }
}