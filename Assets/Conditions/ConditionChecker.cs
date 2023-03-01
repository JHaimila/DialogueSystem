using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Conditions
{
    public static class ConditionChecker
    {
        public static bool CheckConditions(List<Condition> conditions, List<ICondition> conditionals)
        {
            foreach(var condition in conditions)
            {
                foreach(var conditional in conditionals)
                {
                    bool? result = conditional.CheckCondition(condition.GetConditionType(), condition.GetReference());
                    if(result != null && result == condition.Negates())
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}