using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using System;
using UnityEngine.Rendering.Universal;
using static System.TimeZoneInfo;

public class UI_Transitioner : MonoBehaviour
{
    [Header("PostProcessing")]
    [SerializeField] private Volume PPVolume;
    private ChromaticAberration ChromaAberration;
    [SerializeField] private Animator TransitionBlurAnimator;

    public Image TransitionIMG;

    [Header("Parameters")]
    public float OnFullTransitionWaitTime = .5f;

    private IEnumerator _transitionCoroutine;

    private void Awake()
    {
        PPVolume.profile.TryGet<ChromaticAberration>(out ChromaAberration);
    }

    public void MainMenuStartGameTransition(float waitTime, Action onWaitComplete)
    {
        if (_transitionCoroutine != null) StopCoroutine(_transitionCoroutine);

        TransitionBlurAnimator.SetTrigger("Fill");
        _transitionCoroutine = Wait(waitTime, onWaitComplete);
        StartCoroutine(Wait(waitTime, onWaitComplete));
    }

    public void WaitAndThen(float waitTime, Action onWaitComplete)
    {
        if (_transitionCoroutine != null) StopCoroutine(_transitionCoroutine);

        _transitionCoroutine = Wait(waitTime, onWaitComplete);
        StartCoroutine(Wait(waitTime, onWaitComplete));
    }

    public void Transition(float transitionTime, Action onTransitionComplete)
    {
        if (_transitionCoroutine != null) StopCoroutine(_transitionCoroutine);

        _transitionCoroutine = ScreenTransition(transitionTime, onTransitionComplete);
        StartCoroutine(ScreenTransition(transitionTime, onTransitionComplete));
    }

    public void TransitionFade(float from, float to, float transitionTime, Action onTransitionComplete)
    {
        if (_transitionCoroutine != null) StopCoroutine(_transitionCoroutine);

        _transitionCoroutine = ScreenTransitionFade(from, to, transitionTime, onTransitionComplete);
        StartCoroutine(ScreenTransitionFade(from, to, transitionTime, onTransitionComplete));
    }

    public void TransitionIntoCombat(float transitionTime, Action onTransitionComplete)
    {
        if (_transitionCoroutine != null) StopCoroutine(_transitionCoroutine);

        _transitionCoroutine = ScreenTransitionIntoCombat(transitionTime, onTransitionComplete);
        StartCoroutine(ScreenTransitionIntoCombat(transitionTime, onTransitionComplete));
    }

    public void TransitionOutOfCombat(float transitionTime, Action onTransitionComplete)
    {
        if (_transitionCoroutine != null) StopCoroutine(_transitionCoroutine);

        _transitionCoroutine = ScreenTransitionOutOfCombat(transitionTime, onTransitionComplete);
        StartCoroutine(ScreenTransitionOutOfCombat(transitionTime, onTransitionComplete));
    }

    private IEnumerator ScreenTransition(float transitionTime, Action onTransitionComplete)
    {
        float timer = 0f;

        TransitionIMG.fillClockwise = true;
        TransitionIMG.fillAmount = 0f;
        TransitionIMG.color = Color.black;

        while (timer < transitionTime)
        {
            timer += Time.deltaTime;
            TransitionIMG.fillAmount = Mathf.Lerp(0f, 1f, timer / transitionTime);
            yield return null;
        }

        timer = 0f;

        onTransitionComplete();

        TransitionIMG.fillClockwise = false;
        TransitionIMG.fillAmount = 1f;

        while (timer < transitionTime)
        {
            timer += Time.deltaTime;
            TransitionIMG.fillAmount = Mathf.Lerp(1f, 0f, timer / transitionTime);
            yield return null;
        }

        TransitionIMG.fillAmount = 0f;
    }

    private IEnumerator ScreenTransitionIntoCombat(float transitionTime, Action onTransitionComplete)
    {
        float timer = 0f;
        float chromaticTimer = 0f;
        float chromaticTransition = .5f;

        TransitionIMG.fillAmount = 1f;
        TransitionIMG.color = new Color(1f, 1f, 1f, 0f);
        ChromaAberration.intensity.value = 0f;

        CameraManager.instance.SmoothCurrentCameraFov(60f, 30f, transitionTime+chromaticTransition, null);

        while (chromaticTimer < chromaticTransition)
        {
            float chromaticLerp = Mathf.Lerp(0f, 1f, chromaticTimer / chromaticTransition);
            ChromaAberration.intensity.value = chromaticLerp;
            chromaticTimer += Time.deltaTime;
            yield return null;
        }
        ChromaAberration.intensity.value = 1f;

        while (timer < transitionTime)
        {
            float alphaLerp = Mathf.Lerp(0f, 1f, timer / transitionTime);
            TransitionIMG.color = new Color(1f, 1f, 1f, alphaLerp);

            timer += Time.deltaTime;
            yield return null;
        }

        timer = 0f;
        TransitionIMG.color = new Color(1f, 1f, 1f, 1f);
        ChromaAberration.intensity.value = 0f;

        onTransitionComplete();

        while (timer < .5f)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        timer = 0f;

        while (timer < transitionTime)
        {
            float alphaLerp = Mathf.Lerp(1f, 0f, timer / transitionTime);
            TransitionIMG.color = new Color(1f, 1f, 1f, alphaLerp);

            timer += Time.deltaTime;
            yield return null;
        }

        TransitionIMG.color = new Color(1f, 1f, 1f, 0f);
    }

    private IEnumerator ScreenTransitionOutOfCombat(float transitionTime, Action onTransitionComplete)
    {
        float timer = 0f;

        TransitionIMG.fillAmount = 1f;
        TransitionIMG.color = new Color(1f, 1f, 1f, 0f);

        CameraManager.instance.SmoothCurrentCameraRotation(new Vector3(60f, 0, 0), Vector3.zero, 1.5f, null);

        while (timer < transitionTime)
        {
            float alphaLerp = Mathf.Lerp(0f, 1f, timer / transitionTime);
            TransitionIMG.color = new Color(1f, 1f, 1f, alphaLerp);

            timer += Time.deltaTime;
            yield return null;
        }

        timer = 0f;
        TransitionIMG.color = new Color(1f, 1f, 1f, 1f);

        onTransitionComplete();

        while (timer < .5f)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        timer = 0f;

        while (timer < transitionTime)
        {
            float alphaLerp = Mathf.Lerp(1f, 0f, timer / transitionTime);
            TransitionIMG.color = new Color(1f, 1f, 1f, alphaLerp);

            timer += Time.deltaTime;
            yield return null;
        }

        TransitionIMG.color = new Color(1f, 1f, 1f, 0f);
        yield return null;
    }

    private IEnumerator ScreenTransitionFade(float from, float to, float transitionTime, Action onTransitionComplete)
    {
        float timer = 0f;

        TransitionIMG.fillClockwise = true;
        TransitionIMG.fillAmount = 1f;
        TransitionIMG.color = new Color (0, 0, 0, from);

        while (timer < transitionTime)
        {
            timer += Time.deltaTime;
            float lerpdAlpha = Mathf.Lerp(from, to, timer / transitionTime);
            TransitionIMG.color = new Color(0, 0, 0, lerpdAlpha);
            yield return null;
        }

        timer = 0f;
        TransitionIMG.color = new Color(0, 0, 0, to);

        onTransitionComplete();
    }
    private IEnumerator Wait(float waitTime, Action onTransitionComplete)
    {
        float timer = 0f;

        while (timer < waitTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        onTransitionComplete();
    }
}
