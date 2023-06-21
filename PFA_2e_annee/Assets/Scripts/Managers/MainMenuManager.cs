using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager instance;

    [SerializeField] private int sceneToLoadOnStart;

    [Header("Transitioner")]
    public UI_Transitioner Transitioner;

    [Header("Main Buttons")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button creditButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button quitButton;

    [Header("Settings Panel")]
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject settingsHolder;

    [Header("Credit Panel")]
    [SerializeField] private GameObject creditPanel;

    [Header("SliderSettings")]
    [SerializeField] private Slider sliderMasterVolume;

    [Header("Main menu music")]
    [SerializeField] private AudioClip MainMenuMusic;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        startButton.Select();
        UpdateSliderValues();

        SoundManager.instance.PlayMusic(MainMenuMusic);
        SoundManager.instance.Fade(SoundManager.instance.MusicSource, 1f, true);
        SoundManager.instance.Fade(SoundManager.instance.SFXSource, 1f, true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton1) && creditPanel.activeSelf)
        {
            ForceCloseCredit();
        }
    }

    public void StartGame()
    {
        SoundManager.instance.Fade(SoundManager.instance.MusicSource, 1f, false);
        SoundManager.instance.Fade(SoundManager.instance.SFXSource, 1f, false);
        Transitioner.MainMenuStartGameTransition(1.5f, () =>
        {
            SceneManager.LoadScene(sceneToLoadOnStart);
        });

    }

    public void OpenCredit()
    {
        creditPanel.SetActive(true);
        MainButtonInteractableSwitch(false);
        CloseCredit();
    }

    public void CloseCredit()
    {
        StartCoroutine(Credit());
    }

    public void ForceCloseCredit()
    {
        creditPanel.SetActive(false);
        StopAllCoroutines();
        MainButtonInteractableSwitch(true);
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
        MainButtonInteractableSwitch(false);
        sliderMasterVolume.Select();
        //Instantiate(settingsPanel, settingsHolder.transform);
    }

    public void UpdateSliderValues()
    {
        settingsPanel.GetComponent<UI_Settings>().SetSlider();
    }

    public void CloseSettings()
    {
        MainButtonInteractableSwitch(true);
        settingsButton.Select();
        settingsPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void MainButtonInteractableSwitch(bool interactable)
    {
        startButton.gameObject.SetActive(interactable);
        creditButton.gameObject.SetActive(interactable);
        settingsButton.gameObject.SetActive(interactable);
        quitButton.gameObject.SetActive(interactable);
    }

    IEnumerator Credit()
    {
        yield return new WaitForSeconds(20f);
        MainButtonInteractableSwitch(true);
        ForceCloseCredit();
    }

}
