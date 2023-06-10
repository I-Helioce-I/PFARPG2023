using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_HighlightButton : MonoBehaviour, ISelectHandler, IDeselectHandler //IPointerEnterHandler
{
    [SerializeField] private UI_Battle_TabController uiBattleController;
    [SerializeField] private GameObject arrow;
    [SerializeField] private GameObject tabContainer;
    [SerializeField] private GameObject info;

    [Header("Action information")]
    [SerializeField] private Image _actionIcon;
    public Image ActionIcon => _actionIcon;
    [SerializeField] private TextMeshProUGUI _actionName;
    public TextMeshProUGUI ActionName => _actionName;

    public void OnSelect(BaseEventData eventData)
    {
        if (!arrow)
        {
            Debug.LogError("There is no arrow in the scene");
        }

        arrow.SetActive(true);
        uiBattleController.SetSelectedButton(this.gameObject);

        Controller();
    }

    //public void OnPointerEnter(PointerEventData pointerEventData)
    //{
    //    if (!arrow)
    //    {
    //        Debug.LogError("There is no arrow in the scene");
    //    }

    //    arrow.SetActive(true);
    //    uiBattleController.SetSelectedButton(this.gameObject);
    //    if (tabContainer != null)
    //    {
    //        tabContainer.SetActive(true);
    //    }
    //    else if (info != null)
    //    {
    //        info.SetActive(true);
    //    }
    //}

    public void OnDeselect(BaseEventData eventData)
    {
        arrow.SetActive(false);
        if (info != null)
        {
            info.SetActive(false);
        }
    }

    private void Controller()
    {
        if (tabContainer != null)
        {
            OpenContainer();
        }
        else if (info)
        {
            OpenInfo();
        }
        else
        {
            CloseInfo();
        }
    }

    public void OpenContainer()
    {
        uiBattleController.CloseAllContainers();
        tabContainer.SetActive(true);
    }

    public void OpenInfo()
    {
        info.SetActive(true);
        SetInfo();
    }

    public void CloseInfo()
    {
        info.SetActive(false);
    }

    public void SetInfo()
    {
        info.GetComponent<UI_InfoHolder>().SetActionInfo(this.gameObject);
    }
}