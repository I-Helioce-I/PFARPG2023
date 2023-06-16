using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Settings : MonoBehaviour
{
    [Header("Settings components")]
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider soundVolumeSlider;

    private void Awake()
    {
        SetSlider();
    }


    public void ClosePanel()
    {
        if (MainMenuManager.instance != null)
        {
            MainMenuManager.instance.CloseSettings();
        }
        else if (OptionManager.instance != null)
        {
            OptionManager.instance.CloseSettings();
        }
        Destroy(this.gameObject);
    }

    public void SetSlider()
    {
        masterVolumeSlider.value = SoundManager.instance.MasterVolume;
        musicVolumeSlider.value = SoundManager.instance.MusicVolume;
        soundVolumeSlider.value = SoundManager.instance.SFXVolume;
    }
}
