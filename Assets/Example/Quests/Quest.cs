using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Quest : ScriptableObject
{
    [SerializeField] private List<Objective> objectives;
    [SerializeField] private bool completed;
    [SerializeField] private string questName;

    public string GetName()
    {
        return questName;
    }
    public bool Completed()
    {
        return completed;
    }
    public Objective GetObjective(string reference)
    {
        foreach(var objective in objectives)
        {
            if(objective.objectiveName == reference)
            {
                return objective;
            }
        }
        return null;
    }
}

public class Objective
{
    public bool completed;
    public string objectiveName;
}
