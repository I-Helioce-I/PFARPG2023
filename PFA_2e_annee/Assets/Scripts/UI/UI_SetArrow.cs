using UnityEngine;
using UnityEngine.EventSystems;

public class UI_SetArrow : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [Header("Put Arrow prefab")]
    [SerializeField] private GameObject arrow;

    [Header("SFX")]
    [SerializeField] private AudioClip clip;

    private GameObject arrowContainer;

    public void OnSelect(BaseEventData eventData)
    {
        Debug.Log(this.name + " is active");
        CreateArrow();
        //SoundManager.instance.PlaySFX(clip);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        SoundManager.instance.PlaySFX(clip);
        DestroyArrow();
    }

    public void CreateArrow()
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

    public void DestroyArrow()
    {
        Destroy(arrowContainer);
    }
}
