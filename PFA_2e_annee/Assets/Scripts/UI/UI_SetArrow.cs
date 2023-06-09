using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SetArrow : MonoBehaviour
{
    [SerializeField] private UI_Arrow arrow;

    public void SetArrowPosition(Vector3 selectedTab)
    {
        arrow.SetArrowPosition(selectedTab);
    }
}
