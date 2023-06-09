using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public delegate void GameStateEvent(GameState fromState, GameState toState);
    public event GameStateEvent TransitionedFromTo = null;

    public enum GameState
    {
        Menu,
        Exploration,
        Combat,
    }

    public static GameManager instance;

    [Header("Universal variables")]
    public UniversalVariables UniversalVariables;

    [Header("GameState")]
    [SerializeField][ReadOnlyInspector] private GameState _internalState;

    [Header("Scene")]
    [ReadOnlyInspector] public List<Loader.Scene> CurrentScenes = new List<Loader.Scene>();
    public GameState CurrentState
    {
        get
        {
            return _internalState;
        }
        set
        {
            if (value == _internalState) return;
            TransitionToState(value);
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
    private void TransitionToState(GameState toState)
    {
        GameState fromState = _internalState;
        _internalState = toState;

        OnGameStateTransition(fromState, toState);

        TransitionedFromTo?.Invoke(fromState, toState);
    }

    private void OnGameStateTransition(GameState fromState, GameState toState)
    {
        switch (fromState)
        {
            case GameState.Menu:
                break;
            case GameState.Exploration:
                break;
            case GameState.Combat:
                break;
            default:
                break;
        }

        switch (toState)
        {
            case GameState.Menu:
                break;
            case GameState.Exploration:
                break;
            case GameState.Combat:
                break;
            default:
                break;
        }
    }

    private void Update()
    {
        switch (_internalState)
        {
            case GameState.Menu:
                break;
            case GameState.Exploration:
                break;
            case GameState.Combat:
                break;
            default:
                break;
        }
    }
}
