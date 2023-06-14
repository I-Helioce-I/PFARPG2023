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

    public List<CharacterTypeState> PossibleStates = new List<CharacterTypeState>();

    public GameObject SolidCharacterMesh;
    public GameObject LiquidCharacterMesh;
    public GameObject GasCharacterMesh;

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

    private void Awake()
    {
        CharacterTypeState = StartingState;
        TransitionToState(StartingState);
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
                SolidCharacterMesh.SetActive(false);
                break;
            case CharacterTypeState.Liquid:
                LiquidCharacterMesh.SetActive(false);
                break;
            case CharacterTypeState.Gas:
                GasCharacterMesh.SetActive(false);
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
                SolidCharacterMesh.SetActive(true);
                break;
            case CharacterTypeState.Liquid:
                LiquidCharacterMesh.SetActive(true);
                break;
            case CharacterTypeState.Gas:
                GasCharacterMesh.SetActive(true);
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

    public void SwitchStateForward()
    {
        int currentIndex = -1;
        for (int i = 0; i < PossibleStates.Count; i++)
        {
            if (_internalState == PossibleStates[i])
            {
                currentIndex = i;
            }
        }

        currentIndex++;

        if (currentIndex > PossibleStates.Count - 1)
        {
            TransitionToState(PossibleStates[0]);
        }
        else
        {
            TransitionToState(PossibleStates[currentIndex]);
        }
    }

    public void SwitchStateBackward()
    {
        int currentIndex = -1;
        for (int i = 0; i < PossibleStates.Count; i++)
        {
            if (_internalState == PossibleStates[i])
            {
                currentIndex = i;
            }
        }

        currentIndex--;

        if (currentIndex < 0)
        {
            TransitionToState(PossibleStates[PossibleStates.Count-1]);
        }
        else
        {
            TransitionToState(PossibleStates[currentIndex]);
        }
    }

    public void SwitchStateTo(CharacterTypeState toState)
    {
        CharacterTypeState = toState;
    }
}
