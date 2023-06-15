using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager instance;

    [SerializeField] private int sceneToLoadOnStart;

    [Header("Main Buttons")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button quitButton;

    [Header("Settings Panel")]
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject settingsHolder;

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
    }

    public void StartGame()
    {
        SceneManager.LoadScene(sceneToLoadOnStart);
    }

    public void OpenSettings()
    {
        MainButtonInteractableSwitch(false);
        Instantiate(settingsPanel, settingsHolder.transform);
    }

    public void CloseSettings()
    {
        MainButtonInteractableSwitch(true);
        startButton.Select();
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
