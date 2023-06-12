using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_SetArrow : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] private GameObject arrowHolder;

    public void OnSelect(BaseEventData eventData)
    {
        Instantiate(arrowHolder);

        //arrowHolder.SetActive(true);
        //UI_Arrow arrow = arrowHolder.GetComponent<UI_Arrow>();
        //arrow.SetArrowPosition(this.transform.position);
        //arrow.SetArrowRectTransform(this.gameObject);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        arrowHolder.SetActive(false);
    }
}
