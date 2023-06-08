using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactible_StateConstraints : Interactible
{
    [SerializeField] private List<CharacterTypeState> _restrictedStates = new List<CharacterTypeState>();

    public UnityEvent<InteractibleHandler> OnInteractUnsuccessful;

    public override void Interact()
    {
        CharacterStateHandler characterStateHandler = _currentHandler.CharacterStateHandler;
        if (HasCorrectState(characterStateHandler))
        {
            Debug.Log("Interacted with an interactible!");
            OnInteract?.Invoke(_currentHandler);
        }
        else
        {
            Debug.Log("Couldn't interact with this interactible!");
            OnInteractUnsuccessful?.Invoke(_currentHandler);
        }

    }

    private bool HasCorrectState(CharacterStateHandler character)
    {
        bool canInteractWith = false;
        foreach (CharacterTypeState state in _restrictedStates)
        {
            if (character.CharacterTypeState == state)
            {
                canInteractWith = true;
            }
        }

        return canInteractWith;
    }
}
