using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager instance;

    [Header("Main Buttons")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button quitButton;

    [Header("Settings Panel")]
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private Toggle volumeToggle;
    [SerializeField] private Slider volumeSlider;

    private void Start()
    {
        startButton.Select();
        settingsPanel.SetActive(false);
    }

    public void StartGame()
    {
        Debug.Log("Oui oui baguette");
    }

    public void OpenSettings()
    {
        MainButtonInteractableSwitch(false);

        settingsPanel.SetActive(true);
        volumeToggle.Select();
    }

    public void CloseSettings()
    {
        MainButtonInteractableSwitch(true);

        settingsPanel.SetActive(false);
        startButton.Select();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void MainButtonInteractableSwitch(bool interactable)
    {
        startButton.interactable = interactable;
        settingsButton.interactable = interactable;
        quitButton.interactable = interactable;
    }
}
