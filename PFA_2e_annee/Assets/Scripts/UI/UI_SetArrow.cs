using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_SetArrow : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [Header("Arrow")][SerializeField] private GameObject arrow;
    
    private GameObject arrowContainer;

    public void OnSelect(BaseEventData eventData)
    {
        CreateArrow();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        DestroyArrow();
    }

    private void CreateArrow()
    {
        // instantiate the arrow
        arrowContainer = Instantiate(arrow, this.transform);

        // set container size
        RectTransform rt = this.GetComponent<RectTransform>();
        RectTransform arrowContainerSize = arrowContainer.GetComponent<RectTransform>();

        float targetWidth = rt.rect.width;
        float targetHeight = rt.rect.height;

        arrowContainerSize.sizeDelta = new Vector2(targetWidth, targetHeight);

    }

    private void DestroyArrow()
    {
        Destroy(arrowContainer);
    }
}
