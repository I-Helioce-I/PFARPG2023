using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactible : MonoBehaviour
{
    [SerializeField] private GameObject _interactButtonPrompt;

    [SerializeField] private float _range;
    private SphereCollider _collider;
    private InteractibleHandler _currentHandler;

    public UnityEvent<InteractibleHandler> OnInteract;

    private void Start()
    {
        _collider = GetComponent<SphereCollider>();
        _collider.isTrigger = true;
        _collider.radius = _range;

        _interactButtonPrompt.SetActive(false);
    }

    public void Interact()
    {
        Debug.Log("Interacted with an interactible!");
        OnInteract?.Invoke(_currentHandler);
    }

    private void ShowPrompt(bool show)
    {
        _interactButtonPrompt.SetActive(show);
    }

    private void OnTriggerEnter(Collider other)
    {
        InteractibleHandler character = other.GetComponent<InteractibleHandler>();
        if (character)
        {
            character.SetInteractible(this);
            _currentHandler = character;
            ShowPrompt(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        InteractibleHandler character = other.GetComponent<InteractibleHandler>();
        if (character)
        {
            character.NullInteractible();
            _currentHandler = null;
            ShowPrompt(false);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _range);
    }
}
