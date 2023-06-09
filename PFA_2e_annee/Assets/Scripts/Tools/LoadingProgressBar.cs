using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingProgressBar : MonoBehaviour
{
    private Slider LoadingBar;

    private void Awake()
    {
        LoadingBar = transform.GetComponent<Slider>();
    }

    private void Update()
    {
        LoadingBar.value = Loader.GetLoadingProgress();
    }
}
