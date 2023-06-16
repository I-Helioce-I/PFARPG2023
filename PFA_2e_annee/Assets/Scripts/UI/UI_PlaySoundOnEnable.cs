using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_PlaySoundOnEnable : MonoBehaviour
{
    [SerializeField] private AudioClip SFX;

    private void OnEnable()
    {
        SoundManager.instance.PlaySFX(SFX);
    }
}
