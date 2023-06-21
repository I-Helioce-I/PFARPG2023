using Cinemachine;
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
    public DialogueUI DialogueUI;
    public BattleManager BattleManager;
    public UI_CombatMenu CombatMenu;
    public UI_Transitioner Transitioner;
    public UI_SetCharaExplo CharaExploration;
    public Animator InitialTutorial;
    public Animator GetUrielDialogueBox;
    public UI_CInematicBars CinematicBars;

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
                CinematicBars.TransitionToCinematic(false);
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
                CinematicBars.TransitionToCinematic(true);
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
                if (BattleManager.PlayerCharactersInBattle.Contains(BattleManager.ActiveCharacter))
                {
                    CharacterBattle.BattleState state = BattleManager.ActiveCharacter.Battle.CurrentState;
                    switch (state)
                    {
                        case CharacterBattle.BattleState.Idle:
                            break;
                        case CharacterBattle.BattleState.SelectingAction:
                            break;
                        case CharacterBattle.BattleState.Targeting:
                            BattleManager.ActiveCharacter.Battle.SelectTarget();
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
                if (DialogueUI.CurrentWritingCoroutine != null)
                {
                    Debug.Log("Force write all text");
                    DialogueUI.ForceWriteAllText();
                }
                else
                {
                    Debug.Log("Check next line!");
                    DialogueUI.CheckNextDialogueLine();
                }
                break;
            case UIState.Pause:
                if (InitialTutorial.gameObject.activeInHierarchy)
                {
                    InitialTutorial.gameObject.SetActive(false);
                    TransitionToState(UIState.HUD);
                    
                    CameraManager.instance.SmoothCurrentCameraFov(60f, 50f, 2f, () =>
                    {
                        Player.instance.ChangeActionMap("Exploration");
                        CinemachineVirtualCamera vcam = CameraManager.instance.CurrentCamera.GetComponent<CinemachineVirtualCamera>();
                        vcam.m_Lens.FieldOfView = 50f;
                    });
                }

                if (GetUrielDialogueBox.gameObject.activeInHierarchy)
                {
                    GetUrielDialogueBox.gameObject.SetActive(false);
                    TransitionToState(UIState.HUD);
                    Player.instance.ChangeActionMap("Exploration");
                }
                break;
            default:
                break;
        }
    }

    internal void NavigateUp()
    {
        switch (_internalState)
        {
            case UIState.None:
                break;
            case UIState.HUD:
                break;
            case UIState.Combat:
                if (BattleManager.PlayerCharactersInBattle.Contains(BattleManager.ActiveCharacter))
                {
                    CharacterBattle.BattleState state = BattleManager.ActiveCharacter.Battle.CurrentState;
                    switch (state)
                    {
                        case CharacterBattle.BattleState.Idle:
                            break;
                        case CharacterBattle.BattleState.SelectingAction:
                            break;
                        case CharacterBattle.BattleState.Targeting:
                            BattleManager.ActiveCharacter.Battle.ScrollViableTargetBackward();
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
        switch (_internalState)
        {
            case UIState.None:
                break;
            case UIState.HUD:
                break;
            case UIState.Combat:
                if (BattleManager.PlayerCharactersInBattle.Contains(BattleManager.ActiveCharacter))
                {
                    CharacterBattle.BattleState state = BattleManager.ActiveCharacter.Battle.CurrentState;
                    switch (state)
                    {
                        case CharacterBattle.BattleState.Idle:
                            break;
                        case CharacterBattle.BattleState.SelectingAction:
                            break;
                        case CharacterBattle.BattleState.Targeting:
                            BattleManager.ActiveCharacter.Battle.ScrollViableTargetForward();
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
        switch (_internalState)
        {
            case UIState.None:
                break;
            case UIState.HUD:
                break;
            case UIState.Combat:
                if (BattleManager.PlayerCharactersInBattle.Contains(BattleManager.ActiveCharacter))
                {
                    CharacterBattle.BattleState state = BattleManager.ActiveCharacter.Battle.CurrentState;
                    switch (state)
                    {
                        case CharacterBattle.BattleState.Idle:
                            break;
                        case CharacterBattle.BattleState.SelectingAction:
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
                            BattleManager.ActiveCharacter.Battle.TransitionToState(BattleManager.ActiveCharacter.Battle.CurrentState, CharacterBattle.BattleState.SelectingAction);
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

    public void PopUpInitialTutorialDialogue()
    {
        InitialTutorial.gameObject.SetActive(true);
        InitialTutorial.Play("FadeIn");
        TransitionToState(UIState.Pause);
        Player.instance.ChangeActionMap("UI");
    }

    public void PopUpGetUrielDialogue()
    {
        GetUrielDialogueBox.gameObject.SetActive(true);
        GetUrielDialogueBox.Play("FadeIn");
        TransitionToState(UIState.Pause);
        Player.instance.ChangeActionMap("UI");
    }
}
