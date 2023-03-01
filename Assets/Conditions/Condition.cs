using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Conditions
{
    [System.Serializable]
    public class Condition
    {
        [SerializeField] private ConditionType conditionType;
        [SerializeField] private string reference;
        [SerializeField] private bool negate;

        public ConditionType GetConditionType()
        {
            return conditionType;
        }
        public string GetReference()
        {
            return reference;
        }
        public bool Negates()
        {
            return negate;
        }
    }
}

