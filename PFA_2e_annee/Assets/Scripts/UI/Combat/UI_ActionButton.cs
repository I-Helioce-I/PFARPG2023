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
    public ActionDescription RepresentedAction
    {
        get
        {
            return Action;
        }
    }
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
        UIManager.instance.CombatMenu.ActionInfo.SetActive(true);
        UIManager.instance.CombatMenu.ActionInfo.GetComponent<UI_SetInfo>().SetInfos(Icon.sprite, Name.text, Action.DescriptionText);
    }
    public void OnDeselect(BaseEventData eventData)
    {
        //Hide action info.
        UIManager.instance.CombatMenu.ActionInfo.SetActive(false);
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
