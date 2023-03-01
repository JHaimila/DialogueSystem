using System.Collections;
using System.Collections.Generic;
using Conditions;
using UnityEngine;

public class QuestList : MonoBehaviour, ICondition
{
    [SerializeField] private List<Quest> quests;

    public bool? CheckCondition(ConditionType type, string reference)
    {
        switch(type)
        {
            case ConditionType.HasQuest:
            {
                foreach(var quest in quests)
                {
                    if(reference == quest.GetName())
                    {
                        return true;
                    }
                }
                return false;
            }
            case ConditionType.CompletedQuest:
            {
                foreach(var quest in quests)
                {
                    if(reference == quest.GetName())
                    {
                        return quest.Completed();
                    }
                }
                return null;
            }
            case ConditionType.CompletedObjective:
            {
                foreach(var quest in quests)
                {
                    Objective tObjective = quest.GetObjective(reference);
                    if(tObjective != null)
                    {
                        return tObjective.completed;
                    }
                }
                return null;
            }
        }
        return null;
    }
}
