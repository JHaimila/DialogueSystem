using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem
{
    [CreateAssetMenu]
    public class Speaker : ScriptableObject
    {
        public string displayName;
        public Sprite icon;
    }
}