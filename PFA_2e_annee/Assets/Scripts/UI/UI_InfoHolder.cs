using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_InfoHolder : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI nameTxt;
    [SerializeField] private TextMeshProUGUI damageTxt;
    [SerializeField] private Image damageIcon;
    [SerializeField] private Image targetIcon;
    [SerializeField] private TextMeshProUGUI etherTxt;

    public void SetActionInfo(GameObject action)
    {
        UI_HighlightButton actionButtonRef = action.GetComponent<UI_HighlightButton>();
        iconImage.sprite = actionButtonRef.ActionIcon.sprite;
        nameTxt.text = actionButtonRef.ActionName.text;
    }
}