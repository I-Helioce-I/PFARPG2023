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
        if (HasEnoughEther())
        {
            ActionSelected?.Invoke(Action);
        }
    }

    private bool HasEnoughEther()
    {
        if (BattleManager.instance.ActiveCharacter.Stats.Ether.CurrentValue >= Action.EtherCost)
        {
            return true;
        }
        else
        {
            Debug.Log("You don't have enough Ether for this ability!");
            return false;
        }
    }
}
