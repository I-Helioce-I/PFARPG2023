using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GetItemUI : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject background;
    [SerializeField] private GameObject text;
    [SerializeField] private GameObject icon;

    private void Start()
    {
        Activate(false);
    }

    public void GetItem()
    {
        StartCoroutine(Animation());
    }

    private IEnumerator Animation()
    {
        yield return new WaitForSeconds(.5f);

        Activate(true);

        animator.SetBool("Play", true);

        yield return new WaitForSeconds(2f);

        animator.SetBool("Play", false);

        Activate(false);
    }

    private void Activate(bool state)
    {
        background.SetActive(state);
        text.SetActive(state);
        icon.SetActive(state);
    }
}
