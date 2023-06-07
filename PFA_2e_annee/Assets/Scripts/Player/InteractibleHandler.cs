using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractibleHandler : MonoBehaviour
{
    [SerializeField][ReadOnlyInspector] private Interactible _currentInteractible;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && _currentInteractible != null)
        {
            _currentInteractible.Interact();
        }
    }

    public void SetInteractible(Interactible interactible)
    {
        _currentInteractible = interactible;
    }

    public void NullInteractible()
    {
        _currentInteractible = null;
    }
}
