using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UI_Battle_TabController : MonoBehaviour
{
    [SerializeField] private List<GameObject> tabsContainer;
    [SerializeField] private UI_Arrow setArrow;
    [ReadOnlyInspector][SerializeField] private GameObject selectedButton;
    [SerializeField] private GameObject container;

    private void Start()
    {
        //disable all container
        foreach (var tab in tabsContainer)
        {
            tab.SetActive(false);
        }
    }

    public void SetSelectedButton(GameObject SelectedButton)
    {
        selectedButton = SelectedButton;

        //set arrow
        setArrow.SetArrowRectTransform(selectedButton);
    }

    public void CloseAllContainers()
    {
        //disable all container
        foreach (var tab in tabsContainer)
        {
            tab.SetActive(false);
        }
    }

    public void Flee()
    {
        Debug.Log("Flee");
    }
}
