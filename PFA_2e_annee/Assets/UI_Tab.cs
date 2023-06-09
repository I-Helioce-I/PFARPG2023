using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Tab : MonoBehaviour
{
    public Button Self;
    public GameObject LinkedActionsParent;
    public List<Button> LinkedActions = new List<Button>();

    private void Awake()
    {
        Self = GetComponent<Button>();
    }

    public void OpenTab(bool open)
    {
        Self.interactable = !open;
        foreach(Button linkedAction in LinkedActions)
        {
            linkedAction.interactable = open;
        }
        if (open)
        {
            LinkedActions[0].Select();
            UIManager.instance._currentUITab = this;
        }
        else
        {
            Self.Select();
            UIManager.instance._currentUITab = null;
        }
    }

    public void ShowLinkedActions(bool show)
    {
        if (LinkedActionsParent) LinkedActionsParent.SetActive(show);
    }
}
