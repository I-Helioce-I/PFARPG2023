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
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button quitButton;

    [Header("Settings Panel")]
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject settingsHolder;

    [Header("SliderSettings")]
    [SerializeField] private Slider sliderMasterVolume;

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
    }

    public void StartGame()
    {
        Transitioner.MainMenuStartGameTransition(1.5f, () =>
        {
            SceneManager.LoadScene(sceneToLoadOnStart);
        });

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
        settingsButton.gameObject.SetActive(interactable);
        quitButton.gameObject.SetActive(interactable);
    }
}
