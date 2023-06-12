using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Arrow : MonoBehaviour
{
    [SerializeField] private GameObject arrow;

    public void SetArrowPosition(Vector3 target)
    {
        transform.position = target;
    }

    public void SetArrowRectTransform(GameObject target)
    {
        RectTransform rt = this.gameObject.GetComponent<RectTransform>();
        float targetWidth = target.GetComponent<RectTransform>().rect.width;
        float targetHeight = target.GetComponent<RectTransform>().rect.height;

        rt.sizeDelta = new Vector2(targetWidth, targetHeight);

        Vector3 positionTarget = target.GetComponent<RectTransform>().position;
        rt.position = new Vector3(positionTarget.x, positionTarget.y, positionTarget.z);
    }
}
