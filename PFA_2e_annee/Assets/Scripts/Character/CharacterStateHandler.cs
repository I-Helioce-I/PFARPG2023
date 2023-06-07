using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum CharacterTypeState
{
    None,
    Solid,
    Liquid,
    Gas,
    TriplePoint,
}
public class CharacterStateHandler : MonoBehaviour
{
    public delegate void CharacterTypeStateEvent(CharacterTypeState fromState, CharacterTypeState toState);
    public event CharacterTypeStateEvent TransitionedFromTo = null;

    public CharacterTypeState StartingState;

    [SerializeField][ReadOnlyInspector] private CharacterTypeState _internalState;
    public CharacterTypeState CharacterTypeState
    {
        get
        {
            return _internalState;
        }
        set
        {
            if (_internalState == value) return;
            TransitionToState(value);
        }
    }

    private void Start()
    {
        ForceTransitionNoCalls(StartingState);
    }

    private void TransitionToState(CharacterTypeState toState)
    {
        CharacterTypeState fromState = _internalState;
        _internalState = toState;

        OnCharacterTransition(fromState, toState);

        TransitionedFromTo?.Invoke(fromState, toState);
    }

    private void OnCharacterTransition(CharacterTypeState fromState, CharacterTypeState toState)
    {
        switch (fromState)
        {
            case CharacterTypeState.None:
                break;
            case CharacterTypeState.Solid:
                break;
            case CharacterTypeState.Liquid:
                break;
            case CharacterTypeState.Gas:
                break;
            case CharacterTypeState.TriplePoint:
                break;
            default:
                break;
        }

        switch (toState)
        {
            case CharacterTypeState.None:
                break;
            case CharacterTypeState.Solid:
                break;
            case CharacterTypeState.Liquid:
                break;
            case CharacterTypeState.Gas:
                break;
            case CharacterTypeState.TriplePoint:
                break;
            default:
                break;
        }
    }

    private void ForceTransitionNoCalls(CharacterTypeState toState)
    {
        CharacterTypeState fromState = _internalState;
        _internalState = toState;
        OnCharacterTransition(fromState, toState);
    }
}
