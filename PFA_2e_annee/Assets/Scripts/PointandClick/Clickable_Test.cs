using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clickable_Test : Clickable
{
    public override void OnClick()
    {
        base.OnClick();
        Debug.Log("Clicked!");
    }
}
