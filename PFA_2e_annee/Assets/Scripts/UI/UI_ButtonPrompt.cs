using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ButtonPrompt : MonoBehaviour
{
    private Animator _anim;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    public void Appear(bool doAppear)
    {
        if (doAppear)
        {
            _anim.Play("Appear");
        }
        else
        {
            _anim.Play("Disappear");
        }
    }

    public void ForceDisappear()
    {
        _anim.Play("Disappear", 0, 1f);
    }
}
