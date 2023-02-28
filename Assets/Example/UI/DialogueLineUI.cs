using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DialogueSystem
{
    public class DialogueLineUI : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private TMPro.TextMeshProUGUI nameText;
        [SerializeField] private TMPro.TextMeshProUGUI lineText;

        public void Initialize(Sprite iconImage, string nameText, string lineText)
        {
            this.nameText.text = nameText;
            this.lineText.text = lineText;
            icon.sprite = iconImage;
        }
    }
}