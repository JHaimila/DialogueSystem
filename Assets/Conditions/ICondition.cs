using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Conditions
{
    public interface ICondition
    {
        bool? CheckCondition(ConditionType type, string reference);
    }
}

