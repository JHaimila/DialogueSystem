using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem
{
    public class NPCSpeaker : MonoBehaviour
    {
        [SerializeField] private Dialogue dialogue;

        public void StartDialogue()
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<DialogueController>().StartDialogue(dialogue, this);
        }
        public void EndDialogue()
        {

        }
        public void SetDialogue(Dialogue dialogue)
        {
            this.dialogue = dialogue;
        }
        
    }
}