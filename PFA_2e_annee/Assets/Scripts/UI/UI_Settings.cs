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

    private void Start()
    {
        masterVolumeSlider.Select();
        masterVolumeSlider.value = SoundManager.instance.MasterVolume;
        musicVolumeSlider.value = SoundManager.instance.MusicVolume;
        soundVolumeSlider.value = SoundManager.instance.SFXVolume;
    }

    public void ClosePanel()
    {
        MainMenuManager.instance.CloseSettings();
        Destroy(this.gameObject);
    }
}
