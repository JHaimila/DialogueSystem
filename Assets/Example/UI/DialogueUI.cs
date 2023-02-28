using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DialogueSystem
{
    public class DialogueUI : MonoBehaviour
    {
        [SerializeField] private GameObject dialogueLinePrefab;
        [SerializeField] private GameObject choicePrefab;
        [SerializeField] private Transform conversationContainer;
        [SerializeField] private Transform choiceContainer;
        [SerializeField] private ScrollRect scrollRect;
        [SerializeField] private Button exitButton;
        [SerializeField] private Button nextButton;
        [SerializeField] private GameObject appear;

        private DialogueController dialogueController;

        private void Awake() 
        {
            dialogueController = GameObject.FindGameObjectWithTag("Player").GetComponent<DialogueController>();
            dialogueController.OnDialogueStarted += Initialize;
            dialogueController.OnNodeChanged += UpdateUI;
        }
        private void OnDestroy() 
        {
            dialogueController.OnDialogueStarted -= Initialize;
            dialogueController.OnNodeChanged -= UpdateUI;
        }
        public void Initialize()
        {
            ClearList(conversationContainer);
            ClearList(choiceContainer);
            DialogueNode node = dialogueController.GetCurrentNode();
            appear.SetActive(true);
            UpdateUI();
        }
        public void Next()
        {
            dialogueController.Next();
        }
        public void UpdateUI()
        {
            //If choice
            if(dialogueController.IsChoosing())
            {
                nextButton.gameObject.SetActive(false);
                List<DialogueNode> choices = dialogueController.GetChoices();
                foreach(var choice in choices)
                {
                    GameObject newChoice = Instantiate(choicePrefab, choiceContainer);
                    newChoice.GetComponent<Button>().onClick.AddListener(delegate{ChoiceSelected(choice);});
                    newChoice.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = choice.GetLine();
                }
            }
            //If regular line
            else
            {
                GameObject newConversation = Instantiate(dialogueLinePrefab, conversationContainer);
                newConversation.GetComponent<DialogueLineUI>().Initialize(dialogueController.GetCurrentNode().GetSpeaker().icon, 
                    dialogueController.GetCurrentNode().GetSpeaker().displayName, 
                    dialogueController.GetCurrentNode().GetLine());
            }
            if(!dialogueController.HasNext())
            {
                nextButton.gameObject.SetActive(false);
                exitButton.gameObject.SetActive(true);
            }
            else
            {
                if(!nextButton.IsActive() && !dialogueController.IsChoosing())
                {
                    nextButton.gameObject.SetActive(true);
                }
                if(exitButton.gameObject.activeSelf)
                {
                    exitButton.gameObject.SetActive(false);
                }
            }
            ScrollToBottom();
        }
        public void ScrollToBottom()
        {
            Canvas.ForceUpdateCanvases();

            conversationContainer.GetComponent<VerticalLayoutGroup>().CalculateLayoutInputVertical() ;
            conversationContainer.GetComponent<ContentSizeFitter>().SetLayoutVertical() ;

            scrollRect.content.GetComponent<VerticalLayoutGroup>().CalculateLayoutInputVertical() ;
            scrollRect.content.GetComponent<ContentSizeFitter>().SetLayoutVertical() ;

            scrollRect.verticalNormalizedPosition = 0 ;
        }
        public void EndConversation()
        {
            dialogueController.EndDialogue();
            appear.SetActive(false);
        }
        private void ChoiceSelected(DialogueNode choice)
        {
            GameObject newConversation = Instantiate(dialogueLinePrefab, conversationContainer);
            newConversation.GetComponent<DialogueLineUI>().Initialize(choice.GetSpeaker().icon, 
                choice.GetSpeaker().displayName, 
                choice.GetLine());
            dialogueController.OnChoiceSelected(choice);
            ClearList(choiceContainer);
        }
        private void ClearList(Transform parent)
        {
            if(parent.childCount == 0){return;}

            for(int i = 0; i < parent.childCount; i++)
            {
                Destroy(parent.GetChild(i).gameObject);
            }
        }

    }
}