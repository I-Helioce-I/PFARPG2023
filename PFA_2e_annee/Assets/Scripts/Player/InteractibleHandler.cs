using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractibleHandler : MonoBehaviour
{
    [SerializeField][ReadOnlyInspector] private Interactible _currentInteractible;

    public void SetInteractible(Interactible interactible)
    {
        _currentInteractible = interactible;
    }

    public void NullInteractible()
    {
        _currentInteractible = null;
    }

    public void Interact()
    {
        if (_currentInteractible)
        {
            _currentInteractible.Interact();
        }
    }
}
