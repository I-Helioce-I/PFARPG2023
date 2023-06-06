using UnityEngine;

// Script made by Kevin
public class UI_Navigation_FX : MonoBehaviour
{
    [SerializeField] private AudioSource _buttonFx;
    [SerializeField] private AudioClip _hoverFx;

    public void OnHoverSound()
    {
        _buttonFx.PlayOneShot(_hoverFx);
    }
}
