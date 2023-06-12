using UnityEngine;

public class AudioSourceManager : MonoBehaviour
{
    public static AudioSourceManager Instance;

    [Header("SFX")]
    [SerializeField] private AudioSource theme;
    [SerializeField] public AudioSource sfx;
}
