using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DialogueSystem;

public class DialogueSelector : MonoBehaviour
{
    [SerializeField] private GameObject dialogueButtonPrefab;
    [SerializeField] private Transform container;
    private void Start() 
    {
        object[] loadedDialogues = Resources.LoadAll<Dialogue>("");
        foreach(var loadedDialogue in loadedDialogues)
        {
            Dialogue dialogue = loadedDialogue as Dialogue;
            if(dialogue)
            {
                Debug.Log(dialogue.GetNodes().Count);
                GameObject newDialogue = Instantiate(dialogueButtonPrefab, container);
                newDialogue.GetComponent<Button>().onClick.AddListener(delegate{newDialogue.GetComponent<NPCSpeaker>().StartDialogue();});
                newDialogue.GetComponent<NPCSpeaker>().SetDialogue(dialogue);
                newDialogue.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = dialogue.name;
            }
        }
    }
}
