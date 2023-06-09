using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_ActionButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public delegate void ActionEvent(ActionDescription action);
    public event ActionEvent ActionSet = null;
    public event ActionEvent ActionRemoved = null;
    public event ActionEvent ActionSelected = null;

    private ActionDescription Action;
    public Image Icon;
    public TextMeshProUGUI Name;

    public void InitializeActionButton(ActionDescription action)
    {
        Action = action;
        Icon.sprite = action.Icon;
        Name.text = action.Name;
    }

    public void OnSelect(BaseEventData eventData)
    {
        //Show action info.
    }
    public void OnDeselect(BaseEventData eventData)
    {
        //Hide action info.
    }

    public void Click()
    {
        ActionSelected?.Invoke(Action);
    }
}
