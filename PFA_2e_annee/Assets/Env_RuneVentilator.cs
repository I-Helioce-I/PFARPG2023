using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Env_RuneVentilator : MonoBehaviour
{
    public Renderer RuneRenderer;
    public float LightOverTime = 8f;
    public Color EmissiveTint = Color.white;
    [Range(0.0f, 1.0f)]
    public float PulsateIntensity = .2f;
    public float PulsateRate = 5f;

    public ParticleSystem VFX_Wind;

    private bool _isActivated = false;
    private bool _pulsate = false;
    private float _baseIntensity;

    public void Activate()
    {
        if (!_isActivated)
        {
            _baseIntensity = RuneRenderer.material.GetFloat("_emmisiveIntensity");
            _isActivated = true;
            StartCoroutine(ActivateCoroutine());
        }
    }

    private void Update()
    {
        if (_pulsate)
        {
            float pulsate = 1f - Mathf.PingPong(Time.time / PulsateRate, PulsateIntensity);
            RuneRenderer.material.SetFloat("_emmisiveIntensity", _baseIntensity * pulsate);
        }
    }

    private IEnumerator ActivateCoroutine()
    {
        float timer = 0f;
        RuneRenderer.material.SetColor("_emmisiveTint", Color.black);
        VFX_Wind.Play();

        while (timer < LightOverTime)
        {
            timer += Time.deltaTime;
            Color lerpdColor = Color.Lerp(Color.black, EmissiveTint, timer / LightOverTime);
            RuneRenderer.material.SetColor("_emmisiveTint", lerpdColor);
            Debug.Log(lerpdColor);
            yield return null;
        }

        RuneRenderer.material.SetColor("_emmisiveTint", EmissiveTint);
        _pulsate = true;
    }
}
