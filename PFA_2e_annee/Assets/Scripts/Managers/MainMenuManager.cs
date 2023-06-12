using System.Collections;
using System.Collections.Generic;
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

    private void Start()
    {
        startButton.Select();
    }

    public void StartGame()
    {
        // set scene to load
        Debug.Log("Start");
    }

    public void OpenSettings()
    {
        // open settings panel
        Debug.Log("Settings");
    }

    public void QuitGame()
    {
        // quit application
        Debug.Log("Quit");
        Application.Quit();
    }
}
