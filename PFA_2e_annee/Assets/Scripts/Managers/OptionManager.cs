using System.Collections.Generic;
using UnityEngine;
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
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private GameObject panelHandler;
    [SerializeField] private GameObject noMap;

    [SerializeField] private AudioClip openOptionSFX;
    [SerializeField] private AudioClip closeOptionSFX;

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

    private void Start()
    {
        UpdateSliderValues();

    }

    public void UpdateSliderValues()
    {
        settingsPanel.GetComponent<UI_Settings>().SetSlider();
    }

    private void Update()
    {
        //if (Input.GetButtonDown("Cancel") && GameManager.instance.CurrentState != GameManager.GameState.Combat)
        //{
        //    if (state == optionState.Main)
        //    {
        //        if (optionPanel.activeSelf)
        //        {
        //            CloseOption();
        //            //Player.instance.PlayerInput.enabled = true;
        //        }
        //        else
        //        {
        //            OpenOption();
        //            //Player.instance.PlayerInput.enabled = false;
        //        }
        //    }
        //    else
        //    {
        //        CloseAllPanels();
        //    }
        //}
    }

    //private void SwitchOption(bool activate)
    //{
    //    optionPanel.SetActive(activate);
    //    actualSelect.Select();
    //}

    public void OpenOption()
    {
        optionPanel.SetActive(true);

        actualSelect.Select();
        Player.instance.ChangeActionMap("UI");

        SoundManager.instance.PlaySFX(openOptionSFX);
    }

    public void CloseOption()
    {
        actualSelect.Select();
        Player.instance.ChangeActionMap(Player.instance.PreviousActionMap);

        SoundManager.instance.PlaySFX(closeOptionSFX);

        optionPanel.SetActive(false);
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
        settingsPanel.SetActive(true);
        masterVolumeSlider.Select();
        //Instantiate(settingsPanel, settingHandler.transform);
    }

    public void CloseSettings()
    {
        state = optionState.Main;
        SwitchBackground(true);
        settingsPanel.SetActive(false);
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
