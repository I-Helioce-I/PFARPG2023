using System.Collections.Generic;
using UnityEngine;

public class UI_Battle_TabController : MonoBehaviour
{
    [SerializeField] private List<GameObject> tabs;
    [SerializeField] private GameObject selectedTab;
    [SerializeField] private UI_Arrow arrow;
    [SerializeField] private GameObject selectedButton;

    private void Start()
    {
        foreach (var tab in tabs)
        {
            tab.SetActive(false);
        }
    }

    public void OpenTab(GameObject SelectedTab)
    {
        foreach (var tab in tabs)
        {
            tab.SetActive(false);
        }
        selectedTab = SelectedTab;
        selectedTab.SetActive(true);
    }

    public void SetSelectedButton(GameObject SelectedButton)
    {
        selectedButton = SelectedButton;
        SetArrowPosition();
    }

    public void Flee()
    {
        Debug.Log("Flee");
    }

    public void SetArrowPosition()
    {
        arrow.SetArrowPosition(selectedButton.transform.position);
    }
}
