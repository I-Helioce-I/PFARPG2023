using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_CInematicBars : MonoBehaviour
{
    public RectTransform TopBar;
    public RectTransform BottomBar;

    private IEnumerator _currentTransition;

    public void TransitionToCinematic(bool into)
    {
        if (_currentTransition != null) StopCoroutine(_currentTransition);
        _currentTransition = SwitchTransition(into);
        StartCoroutine(SwitchTransition(into));
    }

    private IEnumerator SwitchTransition(bool intoCinematic)
    {
        float timer = 0f;
        float duration = 2f;

        if (intoCinematic)
        {
            TopBar.transform.localPosition = new Vector3(0, 600, 0);
            BottomBar.transform.localPosition = new Vector3(0, -600, 0);
        }
        else
        {
            TopBar.transform.localPosition = new Vector3(0, 525, 0);
            BottomBar.transform.localPosition = new Vector3(0, -525, 0);
        }

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float lerpdPosBottom = 0f;
            float lerpdPosTop = 0f;
            if (intoCinematic)
            {
                lerpdPosTop = Mathf.Lerp(600, 525, timer / duration);
                lerpdPosBottom = Mathf.Lerp(-600, -525, timer / duration);
            }
            else
            {
                lerpdPosTop = Mathf.Lerp(525, 600, timer / duration);
                lerpdPosBottom = Mathf.Lerp(-525, -600, timer / duration);
            }

            TopBar.transform.localPosition = new Vector3(0, lerpdPosTop, 0);
            BottomBar.transform.localPosition = new Vector3(0, lerpdPosBottom, 0);
            yield return null;
        }

        if (intoCinematic)
        {
            TopBar.transform.localPosition = new Vector3(0, 525, 0);
            BottomBar.transform.localPosition = new Vector3(0, -525, 0);
        }
        else
        {
            TopBar.transform.localPosition = new Vector3(0, 600, 0);
            BottomBar.transform.localPosition = new Vector3(0, -600, 0);
        }
    }
}
