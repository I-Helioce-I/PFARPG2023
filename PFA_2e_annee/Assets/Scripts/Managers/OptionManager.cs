using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionManager : MonoBehaviour
{
    public static OptionManager instance;

    [Header("Panels")]
    [SerializeField] private GameObject questPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject partyPanel;

    [Header("Containers")]
    [SerializeField] private GameObject buttons;
    [SerializeField] private GameObject charaInfos;
    [Space(10)]
    [SerializeField] private Button actualSelect;
    [SerializeField] private GameObject settingHandler;
    [SerializeField] private GameObject panelHandler;
    [SerializeField] private GameObject noMap;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(gameObject);
        }

        actualSelect.Select();
    }

    public void OpenParty()
    {
        Instantiate(partyPanel, panelHandler.transform);
    }

    public void CloseParty()
    {
        actualSelect.Select();
    }

    public void Map()
    {
        Instantiate(noMap, settingHandler.transform);
    }

    public void OpenSettings()
    {
        SwitchBackground(false);

        Instantiate(settingsPanel, settingHandler.transform);
    }

    public void CloseSettings()
    {
        SwitchBackground(true);

        actualSelect.Select();
    }

    public void SetActualSelect(Button button)
    {
        actualSelect = button;
    }

    public void SwitchBackground(bool activate)
    {
        questPanel.SetActive(activate);
        buttons.SetActive(activate);
        charaInfos.SetActive(activate);
    }
}
