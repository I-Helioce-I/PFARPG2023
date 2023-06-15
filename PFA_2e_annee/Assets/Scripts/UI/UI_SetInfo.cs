using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_SetInfo : MonoBehaviour
{
    [SerializeField] private Image actionIcon;
    [SerializeField] private TextMeshProUGUI actionName;
    [SerializeField] private TextMeshProUGUI actionDescription;

    public void SetInfos(Sprite icon, string name, string description)
    {
        actionIcon.sprite = icon;
        actionName.text = name;
        actionDescription.text = description;
    }
}
