using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Clickable : MonoBehaviour
{
    public UnityEvent OnClicked;

    public virtual void OnClick()
    {
        OnClicked?.Invoke();
    }
}
