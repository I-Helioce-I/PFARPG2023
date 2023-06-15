using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Tab : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public UI_ActionButton ActionButtonPrefab;

    public Button Self;
    public GameObject LinkedActionsParent;
    public List<Button> LinkedActions = new List<Button>();

    private void Awake()
    {
        Self = GetComponent<Button>();
    }

    public void OpenTab(bool open)
    {
        foreach(Button linkedAction in LinkedActions)
        {
            linkedAction.interactable = open;
        }
        if (open)
        {
            if (LinkedActions.Count > 0)
            {
                UIManager.instance.CombatMenu.TransitionToState(UI_CombatMenu.UICombatMenuState.NavigateActions);
                //LinkedActions[0].Select();
                ShowLinkedActions(true);
                LinkedActions[0].Select();
            }
            else
            {
                Debug.Log("There are no such actions!");
            }

        }
    }

    public void ShowLinkedActions(bool show)
    {
        if (LinkedActionsParent) LinkedActionsParent.SetActive(show);
    }

    public void OnSelect(BaseEventData eventData)
    {
        UIManager.instance.CombatMenu.LastSelectedTab = this;
        ShowLinkedActions(true);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        ShowLinkedActions(false);
    }
}
