using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UI_Battle_TabController : MonoBehaviour
{
    [SerializeField] private List<GameObject> tabsContainer;
    [SerializeField] private UI_SetArrow setArrow;
    [ReadOnlyInspector][SerializeField] private GameObject selectedTab;
    [SerializeField] private GameObject container;

    private void Start()
    {
        //disable all container
        foreach (var tab in tabsContainer)
        {
            tab.SetActive(false);
        }
    }

    public void SetSelectedTab(GameObject SelectedTab)
    {
        Debug.Log("selected");

        ////disable all container
        //foreach (var tabContainer in tabsContainer)
        //{
        //    tabContainer.SetActive(false);
        //}
        selectedTab = SelectedTab;

        //set arrow
        setArrow.SetArrowPosition(selectedTab.transform.position);
    }

    public void SetDeselectTab()
    {
        foreach (var tabContainer in tabsContainer)
        {
            tabContainer.SetActive(false);
        }
    }

    public void OpenSelectedTab()
    {

    }

    public void Flee()
    {
        Debug.Log("Flee");
    }
}
