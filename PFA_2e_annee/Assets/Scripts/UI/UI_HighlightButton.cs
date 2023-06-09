using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_HighlightButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] private UI_Battle_TabController uiBattleController;
    [SerializeField] private GameObject arrow;
    [SerializeField] private GameObject tab;
    [SerializeField] private GameObject info;

    public void OnSelect(BaseEventData eventData)
    {
        if (!arrow)
        {
            Debug.LogError("There is no arrow in the scene");
        }

        arrow.SetActive(true);
        uiBattleController.SetSelectedTab(this.gameObject);
        if (tab != null)
        {
            tab.SetActive(true);
        }
        else if (info != null)
        {
            info.SetActive(true);
        }
    }

    public void OnDeselect(BaseEventData eventData)
    {
        arrow.SetActive(false);
        if (info != null)
        {
            info.SetActive(false);
        }
    }
}