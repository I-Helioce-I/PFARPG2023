using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class OptionManager : MonoBehaviour
{
    public static OptionManager instance;

    private enum optionState
    {
        Main,
        Sub
    }

    [ReadOnlyInspector][SerializeField] private optionState state;

    [Header("Panels")]
    [SerializeField] private GameObject optionPanel;
    [SerializeField] private GameObject buttonsPanel;
    [SerializeField] private GameObject questPanel;
    [SerializeField] private GameObject charaInfos;
    [SerializeField] private GameObject partyPanel;
    [SerializeField] private GameObject settingsPanel;

    [Header("Containers")]
    [SerializeField] private List<Button> buttons;
    [Space(20)]
    [SerializeField] private List<GameObject> optionPlayerStats;
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

        actualSelect = buttons[0];
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (state == optionState.Main)
            {
                if (optionPanel.activeSelf)
                {
                    CloseOption();
                    Player.instance.PlayerInput.enabled = true;
                }
                else
                {
                    OpenOption();
                    Player.instance.PlayerInput.enabled = false;
                }
            }
            else
            {
                CloseAllPanels();
            }
        }
    }

    //private void SwitchOption(bool activate)
    //{
    //    optionPanel.SetActive(activate);
    //    actualSelect.Select();
    //}

    public void OpenOption()
    {
        actualSelect = buttons[0];
        optionPanel.SetActive(true);
        actualSelect.Select();
    }

    public void CloseOption()
    {
        optionPanel.SetActive(false);
        actualSelect = buttons[0];
    }

    public void OpenParty()
    {
        state = optionState.Sub;
        Instantiate(partyPanel, panelHandler.transform);
    }

    public void CloseParty()
    {
        state = optionState.Main;
        actualSelect.Select();
    }

    public void Map()
    {
        Instantiate(noMap, settingHandler.transform);
    }

    public void OpenSettings()
    {
        state = optionState.Sub;
        SwitchBackground(false);
        Instantiate(settingsPanel, settingHandler.transform);
    }

    public void CloseSettings()
    {
        state = optionState.Main;
        SwitchBackground(true);
        actualSelect.Select();
    }

    public void CloseAllPanels()
    {
        CloseParty();
        CloseSettings();
    }

    public void SetActualSelect(Button button)
    {
        actualSelect = button;
    }

    public void SwitchBackground(bool activate)
    {
        questPanel.SetActive(activate);
        buttonsPanel.SetActive(activate);
        charaInfos.SetActive(activate);
    }
}
