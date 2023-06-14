using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SoundSetter : MonoBehaviour
{
    enum VolumeType
    {
        Master,
        Music,
        SFX
    }

    [Header("Icons")]
    [SerializeField] private Image soundIcon;
    [SerializeField] private Sprite muteSprite;
    [SerializeField] private Sprite midSprite;
    [SerializeField] private Sprite soundSprite;

    [Header("SFX")]
    [SerializeField] private AudioClip clip;

    [SerializeField] private VolumeType volumeType;

    private void Start()
    {
        SliderUpdate();
    }

    public void SliderUpdate()
    {
        Slider slider = this.GetComponent<Slider>();
        slider.value = Mathf.Round(slider.value * 10.0f) * 0.1f;

        SoundManager.instance.PlaySFX(clip);

        if (slider.value > 0.5f)
        {
            soundIcon.sprite = soundSprite;
        }
        else if (slider.value < 0.1f)
        {
            soundIcon.sprite = muteSprite;
        }
        else
        {
            soundIcon.sprite = midSprite;
        }

        switch (volumeType)
        {
            case VolumeType.Master:
                SoundManager.instance.SetMasterVolume(slider.value);
                break;
            case VolumeType.Music:
                SoundManager.instance.SetMusicVolume(slider.value);
                break;
            case VolumeType.SFX:
                SoundManager.instance.SetSFXVolume(slider.value);
                break;
            default:
                break;
        }
    }
}
