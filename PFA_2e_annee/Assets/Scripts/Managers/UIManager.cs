using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public delegate void UIStateEvent(UIState fromState, UIState toState);
    public event UIStateEvent TransitionedFromTo = null;

    [Header("Object references")]
    [SerializeField] private DialogueUI _dialogueUI;
    [SerializeField] private BattleManager _battleManager;
    public UI_CombatMenu CombatMenu;

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
                if (_battleManager.PlayerCharactersInBattle.Contains(_battleManager.ActiveCharacter))
                {
                    CharacterBattle.BattleState state = _battleManager.ActiveCharacter.Battle.CurrentState;
                    switch (state)
                    {
                        case CharacterBattle.BattleState.Idle:
                            break;
                        case CharacterBattle.BattleState.SelectingAction:
                            break;
                        case CharacterBattle.BattleState.Targeting:
                            Debug.Log(4);
                            _battleManager.ActiveCharacter.Battle.SelectTarget();
                            break;
                        case CharacterBattle.BattleState.Busy:
                            break;
                        case CharacterBattle.BattleState.Sliding:
                            break;
                        default:
                            break;
                    }

                }
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

    internal void NavigateUp()
    {
        Debug.Log("NavigateUp.");

        switch (_internalState)
        {
            case UIState.None:
                break;
            case UIState.HUD:
                break;
            case UIState.Combat:
                if (_battleManager.PlayerCharactersInBattle.Contains(_battleManager.ActiveCharacter))
                {
                    CharacterBattle.BattleState state = _battleManager.ActiveCharacter.Battle.CurrentState;
                    switch (state)
                    {
                        case CharacterBattle.BattleState.Idle:
                            break;
                        case CharacterBattle.BattleState.SelectingAction:
                            break;
                        case CharacterBattle.BattleState.Targeting:
                            Debug.Log(5);
                            _battleManager.ActiveCharacter.Battle.ScrollViableTargetForward();
                            break;
                        case CharacterBattle.BattleState.Busy:
                            break;
                        case CharacterBattle.BattleState.Sliding:
                            break;
                        default:
                            break;
                    }                  
                }
                break;
            case UIState.Dialogue:
                break;
            case UIState.Pause:
                break;
            default:
                break;
        }
    }
    internal void NavigateDown()
    {
        Debug.Log("NavigateDown.");

        switch (_internalState)
        {
            case UIState.None:
                break;
            case UIState.HUD:
                break;
            case UIState.Combat:
                if (_battleManager.PlayerCharactersInBattle.Contains(_battleManager.ActiveCharacter))
                {
                    CharacterBattle.BattleState state = _battleManager.ActiveCharacter.Battle.CurrentState;
                    switch (state)
                    {
                        case CharacterBattle.BattleState.Idle:
                            break;
                        case CharacterBattle.BattleState.SelectingAction:
                            break;
                        case CharacterBattle.BattleState.Targeting:
                            Debug.Log(6);
                            _battleManager.ActiveCharacter.Battle.ScrollViableTargetBackward();
                            break;
                        case CharacterBattle.BattleState.Busy:
                            break;
                        case CharacterBattle.BattleState.Sliding:
                            break;
                        default:
                            break;
                    }
                }
                break;
            case UIState.Dialogue:
                break;
            case UIState.Pause:
                break;
            default:
                break;
        }
    }

    internal void Return()
    {
        Debug.Log("Return button done.");

        switch (_internalState)
        {
            case UIState.None:
                break;
            case UIState.HUD:
                break;
            case UIState.Combat:
                if (_battleManager.PlayerCharactersInBattle.Contains(_battleManager.ActiveCharacter))
                {
                    CharacterBattle.BattleState state = _battleManager.ActiveCharacter.Battle.CurrentState;
                    switch (state)
                    {
                        case CharacterBattle.BattleState.Idle:
                            break;
                        case CharacterBattle.BattleState.SelectingAction:
                            Debug.Log("Return to selecting tab");
                            switch (CombatMenu.CurrentState)
                            {
                                case UI_CombatMenu.UICombatMenuState.Closed:
                                    break;
                                case UI_CombatMenu.UICombatMenuState.WaitingInstruction:
                                    break;
                                case UI_CombatMenu.UICombatMenuState.NavigateTabs:
                                    break;
                                case UI_CombatMenu.UICombatMenuState.NavigateActions:
                                    CombatMenu.TransitionToState(UI_CombatMenu.UICombatMenuState.NavigateTabs);
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case CharacterBattle.BattleState.Targeting:
                            Debug.Log("Return from targeting");
                            _battleManager.ActiveCharacter.Battle.TransitionToState(_battleManager.ActiveCharacter.Battle.CurrentState, CharacterBattle.BattleState.SelectingAction);
                            CombatMenu.ReturnToNavigateFromTargeting();
                            break;
                        case CharacterBattle.BattleState.Busy:
                            break;
                        case CharacterBattle.BattleState.Sliding:
                            break;
                        default:
                            break;
                    }
                }
                break;
            case UIState.Dialogue:
                break;
            case UIState.Pause:
                break;
            default:
                break;
        }
    }
}
