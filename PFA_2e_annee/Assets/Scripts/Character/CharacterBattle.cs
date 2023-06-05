using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterBattle : MonoBehaviour
{
    public enum BattleState
    {
        Idle,
        Targeting,
        Busy,
        Sliding,
    }

    private BattleState _state;

    [Header("Current battle")]
    [SerializeField] private BattleManager _battle;
    public BattleManager BattleManager
    {
        get
        {
            return _battle;
        }
        set
        {
            _battle = value;
        }
    }

    [Header("Character Actions")]
    public CharacterActions CharacterActions;
    public ActionDescription[] Actions = new ActionDescription[11];
    private List<UI_ActionSlot> _actions = new List<UI_ActionSlot>();

    [Header("Initiative")]
    [ReadOnlyInspector] public float Initiative = 0f;

    [Header("Position")]
    public bool IsEnemy = false;
    public bool IsFrontRow = true;

    [Header("Targeting")]
    public GameObject TargetingIndicator;
    [SerializeField] [ReadOnlyInspector] private int _currentTargetingIndex = 0;
    private GameObject _currentTargetingIndicator;
    [SerializeField] [ReadOnlyInspector] private ActionDescription _currentSelectedAction;
    [SerializeField] [ReadOnlyInspector] private List<Character> _viableTargets = new List<Character>();

    private Vector3 _slideTargetPosition;
    private Vector3 _slideOriginalPosition;
    private Action _onSlideComplete;


    private float _slideDuration = .5f;
    private float _slideTimer = 0f;

    public void InitializeCharacterActions()
    {
        this.CharacterActions.SetActions(Actions);
        SetCharacterActionListening();
    }
    private void Update()
    {
        switch (_state)
        {
            case BattleState.Idle:
                break;
            case BattleState.Targeting:
                if (Input.GetAxis("Mouse ScrollWheel") > 0)
                {
                    _currentTargetingIndex += 1;
                    if (_currentTargetingIndex > _viableTargets.Count-1)
                    {
                        _currentTargetingIndex = 0;
                    }
                    _currentTargetingIndicator.transform.position = SetTargetingIndicatorPosition(_currentTargetingIndex);
                }
                if (Input.GetAxis("Mouse ScrollWheel") < 0)
                {
                    _currentTargetingIndex -= 1;
                    if (_currentTargetingIndex < 0)
                    {
                        _currentTargetingIndex = _viableTargets.Count-1;
                    }
                    _currentTargetingIndicator.transform.position = SetTargetingIndicatorPosition(_currentTargetingIndex);
                }
                if (Input.GetMouseButtonDown(0))
                {
                    if (_viableTargets.Count < 1 || _currentSelectedAction == null) return;
                    ActionConfirm(_currentSelectedAction, _viableTargets[_currentTargetingIndex]);
                }
                break;
            case BattleState.Busy:
                break;
            case BattleState.Sliding:
                transform.position = Vector3.Lerp(_slideOriginalPosition, _slideTargetPosition, _slideTimer / _slideDuration);
                _slideTimer += Time.deltaTime;
                if (_slideTimer > _slideDuration)
                {
                    _slideTimer = 0f;
                    transform.position = _slideTargetPosition;
                    _onSlideComplete();
                }
                break;
            default:
                break;
        }
    }

    public void TransitionToState(BattleState fromstate, BattleState state)
    {
        BattleState fromState = _state;
        switch (_state)
        {
            case BattleState.Idle:
                break;
            case BattleState.Targeting:
                if (_currentTargetingIndicator)
                {
                    Destroy(_currentTargetingIndicator);
                    _currentTargetingIndicator = null;
                }
                break;
            case BattleState.Busy:
                break;
            case BattleState.Sliding:
                break;
            default:
                break;
        }
        _state = state;
        switch (_state)
        {
            case BattleState.Idle:
                break;
            case BattleState.Targeting:
                _currentTargetingIndex = 0;
                if (!_currentTargetingIndicator)
                {
                    GameObject newIndicator = Instantiate<GameObject>(TargetingIndicator, SetTargetingIndicatorPosition(0), Quaternion.identity);
                    _currentTargetingIndicator = newIndicator;
                }
                else
                {
                    _currentTargetingIndicator.transform.position = SetTargetingIndicatorPosition(0);
                }
                break;
            case BattleState.Busy:
                break;
            case BattleState.Sliding:
                break;
            default:
                break;
        }
    }

    private Vector3 SetTargetingIndicatorPosition(int index)
    {
        return _viableTargets[index].transform.position + (Vector3.up * 3f);
    }

    public void SetCharacterActionListening()
    {
        if (!CharacterActions) return;

        PurgeAllActionSlots();

        for (int i = 0; i < CharacterActions.Attacks.Actions.Length; i++)
        {
            UI_ActionSlot actionSlot = CharacterActions.Attacks.Actions[i];
            if (actionSlot.GetAction)
            {
                actionSlot.ActionSelected -= OnActionSelected;
                actionSlot.ActionSelected += OnActionSelected;

                _actions.Add(actionSlot);
            }
        }

        for (int i = 0; i < CharacterActions.Spells.Actions.Length; i++)
        {
            UI_ActionSlot actionSlot = CharacterActions.Spells.Actions[i];
            if (actionSlot.GetAction)
            {
                actionSlot.ActionSelected -= OnActionSelected;
                actionSlot.ActionSelected += OnActionSelected;

                _actions.Add(actionSlot);
            }
        }

        for (int i = 0; i < CharacterActions.Items.Actions.Length; i++)
        {
            UI_ActionSlot actionSlot = CharacterActions.Items.Actions[i];
            if (actionSlot.GetAction)
            {
                actionSlot.ActionSelected -= OnActionSelected;
                actionSlot.ActionSelected += OnActionSelected;

                _actions.Add(actionSlot);
            }
        }
    }

    private void OnActionSelected(ActionDescription action)
    {
        _currentSelectedAction = action;
        //Choose target, which can only potentially target potentials according to ActionDescription.
        _viableTargets = GetViableTargets(action);
        TransitionToState(_state, BattleState.Targeting);

        //For debug purposes, go directly for attack.
        //CharacterActions.CloseAll();
        //UseActionOn(BattleManager.instance._enemy.Battle, action, () =>
        //{
        //    BattleManager.instance.GetNextInitiative();
        //});
    }

    private void ActionConfirm(ActionDescription action, Character target)
    {
        CharacterActions.CloseAll();
        _currentSelectedAction = null;
        TransitionToState(_state, BattleState.Busy);
        UseActionOn(target.Battle, action, () =>
        {
            BattleManager.instance.GetNextInitiative();
        });
    }

    private void PurgeAllActionSlots()
    {
        foreach (UI_ActionSlot actionSlot in _actions)
        {
            actionSlot.ActionSelected -= OnActionSelected;
            _actions.Remove(actionSlot);
        }
    }

    public void RollInitiative(float speed)
    {
        int dieRoll = UnityEngine.Random.Range(1, GameManager.instance.UniversalVariables.InitiativeRollSize + 1);
        Initiative =  dieRoll + speed;
        TransitionToState(_state, BattleState.Idle);
    }

    public void UseActionOn(CharacterBattle target, ActionDescription action, Action onActionComplete)
    {
        Debug.Log(this + " used " + action.Name + " on " + target + "!");

        if (action.doesSlide)
        {
            Vector3 slideTargetPosition = target.GetPosition() + (GetPosition() - target.GetPosition()).normalized * 2f;

            SlideToPosition(slideTargetPosition, () =>
            {
                TransitionToState(_state, BattleState.Busy);
                //Play attacking animation.
                SlideToPosition(_slideOriginalPosition, () =>
                {
                    TransitionToState(_state, BattleState.Idle);
                    onActionComplete();
                });
            });
        }
        else
        {
            onActionComplete();
        }
    }

    private void SlideToPosition(Vector3 slideTargetPosition, Action onSlideComplete)
    {
        _slideTargetPosition = slideTargetPosition;
        _slideOriginalPosition = GetPosition();
        _onSlideComplete = onSlideComplete;
        TransitionToState(_state, BattleState.Sliding);
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public ActionDescription GetRandomAction()
    {
        List<ActionDescription> potentialActions = new List<ActionDescription>();
        for (int i = 0; i < Actions.Length; i++)
        {
            ActionDescription action = Actions[i];
            if (action) potentialActions.Add(action);
        }
        if (potentialActions.Count == 0)
        {
            Debug.Log(this + " has no potential actions!");
            return null;
        }

        int randomIndex = UnityEngine.Random.Range(0, potentialActions.Count);
        _currentSelectedAction = potentialActions[randomIndex];
        return potentialActions[randomIndex];
    }

    public List<Character> GetViableTargets(ActionDescription action)
    {
        List<Character> potentialTargetCollection = new List<Character>();
        foreach (Character character in _battle.GetAllCharactersInBattle())
        {
            switch (action.ViableTargetRow)
            {
                case ActionDescription.TargetRow.Both:
                    if (!IsEnemy)
                    {
                        if (action.TargetsAllies)
                        {
                            if (_battle.GetPlayerCharactersInBattle.Contains(character))
                            {
                                potentialTargetCollection.Add(character);
                            }
                        }
                        else
                        {
                            if (_battle.GetEnemyCharactersInBattle.Contains(character))
                            {
                                potentialTargetCollection.Add(character);
                            }
                        }
                    }
                    else
                    {
                        if (action.TargetsAllies)
                        {
                            if (_battle.GetEnemyCharactersInBattle.Contains(character))
                            {
                                potentialTargetCollection.Add(character);
                            }
                        }
                        else
                        {
                            if (_battle.GetPlayerCharactersInBattle.Contains(character))
                            {
                                potentialTargetCollection.Add(character);
                            }
                        }
                    }

                    break;
                case ActionDescription.TargetRow.Front:
                    if (!IsEnemy)
                    {
                        if (action.TargetsAllies)
                        {
                            if (_battle.GetPlayerCharactersInBattle.Contains(character))
                            {
                                if (character.Battle.IsFrontRow)
                                {
                                    potentialTargetCollection.Add(character);
                                }
                            }
                        }
                        else
                        {
                            if (_battle.GetEnemyCharactersInBattle.Contains(character))
                            {
                                if (character.Battle.IsFrontRow)
                                {
                                    potentialTargetCollection.Add(character);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (action.TargetsAllies)
                        {
                            if (_battle.GetEnemyCharactersInBattle.Contains(character))
                            {
                                if (character.Battle.IsFrontRow)
                                {
                                    potentialTargetCollection.Add(character);
                                }
                            }
                        }
                        else
                        {
                            if (_battle.GetPlayerCharactersInBattle.Contains(character))
                            {
                                if (character.Battle.IsFrontRow)
                                {
                                    potentialTargetCollection.Add(character);
                                }
                            }
                        }
                    }

                    break;
                case ActionDescription.TargetRow.Back:
                    if (!IsEnemy)
                    {
                        if (action.TargetsAllies)
                        {
                            if (_battle.GetPlayerCharactersInBattle.Contains(character))
                            {
                                if (!character.Battle.IsFrontRow)
                                {
                                    potentialTargetCollection.Add(character);
                                }
                            }
                        }
                        else
                        {
                            if (_battle.GetEnemyCharactersInBattle.Contains(character))
                            {
                                if (!character.Battle.IsFrontRow)
                                {
                                    potentialTargetCollection.Add(character);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (action.TargetsAllies)
                        {
                            if (_battle.GetEnemyCharactersInBattle.Contains(character))
                            {
                                if (!character.Battle.IsFrontRow)
                                {
                                    potentialTargetCollection.Add(character);
                                }
                            }
                        }
                        else
                        {
                            if (_battle.GetPlayerCharactersInBattle.Contains(character))
                            {
                                if (!character.Battle.IsFrontRow)
                                {
                                    potentialTargetCollection.Add(character);
                                }
                            }
                        }
                    }

                    break;
                case ActionDescription.TargetRow.Self:
                    if (character.Battle == this)
                    {
                        potentialTargetCollection.Add(character);
                    }
                    break;
                default:
                    break;
            }
        }
        _viableTargets = potentialTargetCollection;
        return _viableTargets;
    }

    public Character GetRandomTargetFromViableTargets()
    {
        List<Character> viableTargets = _viableTargets;
        int randomIndex = UnityEngine.Random.Range(0, viableTargets.Count);
        return viableTargets[randomIndex];
    }

    public void OpenActionsMenu()
    {
        if (!CharacterActions) return;
        CharacterActions.OpenAttacks();
    }
}
