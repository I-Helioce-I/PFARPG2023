using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance { get; private set; }

    [Header("Mixers")]
    [SerializeField] AudioMixer MasterMixer;

    [Header("Parameters")]
    public float MasterVolume = 0.7f;
    public float MusicVolume = 0.5f;
    public float SFXVolume = 0.5f;

    [Header("Sounds")]
    public AudioSource MusicSource, SFXSource;

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

        SetMasterVolume(MasterVolume);
        SetMusicVolume(MusicVolume);
        SetSFXVolume(SFXVolume);
    }

    public void SetMasterVolume(float volume)
    {
        MasterVolume = volume;
        MasterMixer.SetFloat("masterVolume", Mathf.Log10(volume) * 20);
    }

    public void SetMusicVolume(float volume)
    {
        MusicVolume = volume;
        MasterMixer.SetFloat("musicVolume", Mathf.Log10(volume) * 20);
    }

    public void SetSFXVolume(float volume)
    {
        SFXVolume = volume;
        MasterMixer.SetFloat("sfxVolume", Mathf.Log10(volume) * 20);
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.Log("Clip that needed to be played is nonexistent!");
            return;
        }
        SFXSource.PlayOneShot(clip);
    }

    public void PlayRandomSFX(List<AudioClip> clips)
    {
        if (clips.Count <= 0)
        {
            Debug.Log("No clips in clip list sent!");
            return;
        }

        int randomIndex = Random.Range(0, clips.Count);
        AudioClip chosenClip = clips[randomIndex];

        SFXSource.PlayOneShot(chosenClip);
    }

    public void PlayMusic(AudioClip music)
    {
        if (music == null)
        {
            Debug.Log("Music that needed to be played is nonexistent!");
            return;
        }
        MusicSource.clip = music;
        MusicSource.loop = true;
        MusicSource.Play();
    }

    public void StopMusic()
    {
        MusicSource.Stop();
    }
}
