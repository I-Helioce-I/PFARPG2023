using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using System;

public class UI_Transitioner : MonoBehaviour
{
    [Header("PostProcessing")]
    private Volume PPVolume;

    public Image TransitionIMG;

    [Header("Parameters")]
    public float OnFullTransitionWaitTime = .5f;

    private IEnumerator _transitionCoroutine;

    public void Transition(float transitionTime, Action onTransitionComplete)
    {
        if (_transitionCoroutine != null) StopCoroutine(_transitionCoroutine);

        _transitionCoroutine = ScreenTransition(transitionTime, onTransitionComplete);
        StartCoroutine(ScreenTransition(transitionTime, onTransitionComplete));
    }

    private IEnumerator ScreenTransition(float transitionTime, Action onTransitionComplete)
    {
        float timer = 0f;

        TransitionIMG.fillClockwise = true;
        TransitionIMG.fillAmount = 0f;

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
}
