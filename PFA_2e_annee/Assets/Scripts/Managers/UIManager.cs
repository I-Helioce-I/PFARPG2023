using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public delegate void UIStateEvent(UIState fromState, UIState toState);
    public event UIStateEvent TransitionedFromTo = null;

    [Header("Object references")]
    [SerializeField] private DialogueUI _dialogueUI;

    public enum UIState
    {
        None,
        HUD,
        Combat,
        Dialogue,
        Pause,
    }
    public UIState StartingState;
    [SerializeField][ReadOnlyInspector] private UIState _internalState;
    public UIState CurrentState
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
    private UIState _previousState;
    public UIState PreviousState
    {
        get
        {
            return _previousState;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        TransitionToState(StartingState);
    }

    private void TransitionToState(UIState toState)
    {
        _previousState = _internalState;
        UIState fromState = _internalState;
        _internalState = toState;

        OnStateTransition(fromState, toState);

        TransitionedFromTo?.Invoke(fromState, toState);
    }

    private void OnStateTransition(UIState fromState, UIState toState)
    {
        switch (fromState)
        {
            case UIState.None:
                break;
            case UIState.HUD:
                break;
            case UIState.Combat:
                break;
            case UIState.Dialogue:
                break;
            case UIState.Pause:
                break;
            default:
                break;
        }

        switch (toState)
        {
            case UIState.None:
                break;
            case UIState.HUD:
                break;
            case UIState.Combat:
                break;
            case UIState.Dialogue:
                break;
            case UIState.Pause:
                break;
            default:
                break;
        }
    }

    internal void Select()
    {
        switch (_internalState)
        {
            case UIState.None:
                break;
            case UIState.HUD:
                break;
            case UIState.Combat:
                break;
            case UIState.Dialogue:
                if (_dialogueUI.CurrentWritingCoroutine != null)
                {
                    Debug.Log("Force write all text");
                    _dialogueUI.ForceWriteAllText();
                }
                else
                {
                    Debug.Log("Check next line!");
                    _dialogueUI.CheckNextDialogueLine();
                }
                break;
            case UIState.Pause:
                break;
            default:
                break;
        }
    }
}
