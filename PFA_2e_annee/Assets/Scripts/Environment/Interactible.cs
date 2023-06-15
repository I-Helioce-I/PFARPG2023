using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactible : MonoBehaviour
{
    [SerializeField] protected GameObject _interactButtonPrompt;

    [SerializeField] protected float _range;
    [SerializeField] protected bool _forcedInteraction;
    protected SphereCollider _collider;
    protected InteractibleHandler _currentHandler;
    private CharacterExplorationStateHandler _currentCESH;

    public UnityEvent<InteractibleHandler> OnInteract;

    private void Start()
    {
        _collider = GetComponent<SphereCollider>();
        _collider.isTrigger = true;
        _collider.radius = _range;

        _interactButtonPrompt.SetActive(false);
    }

    private void OnDisable()
    {
        ShowPrompt(false);
    }

    public virtual void Interact()
    {
        Debug.Log("Interacted with an interactible!");
        OnInteract?.Invoke(_currentHandler);
    }

    public void ShowPrompt(bool show)
    {
        _interactButtonPrompt.SetActive(show);
    }

    private void OnHandlerStateTransition(CharacterTypeState from, CharacterTypeState to)
    {
        switch (to)
        {
            case CharacterTypeState.None:
                break;
            case CharacterTypeState.Solid:
                break;
            case CharacterTypeState.Liquid:
                break;
            case CharacterTypeState.Gas:
                if (_forcedInteraction)
                {
                    Interact();
                }
                break;
            case CharacterTypeState.TriplePoint:
                break;
            default:
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        InteractibleHandler character = other.GetComponent<InteractibleHandler>();
        if (character)
        {
            character.SetInteractible(this);
            _currentHandler = character;
            CharacterExplorationStateHandler CESH = _currentHandler.GetComponent<CharacterExplorationStateHandler>();
            if (CESH)
            {
                _currentCESH = CESH;
                _currentCESH.TransitionedFromTo -= OnHandlerStateTransition;
                _currentCESH.TransitionedFromTo += OnHandlerStateTransition;
            }
        }

        if (_forcedInteraction)
        {
            Interact();
        }
        else
        {
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
            _currentCESH.TransitionedFromTo -= OnHandlerStateTransition;
            _currentCESH = null;
            ShowPrompt(false);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _range);
    }
}
