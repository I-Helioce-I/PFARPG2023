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
        SelectingAction,
        Targeting,
        Busy,
        Sliding,
    }

    private BattleState _state;
    public BattleState CurrentState
    {
        get
        {
            return _state;
        }
    }

    [Header("Object references")]
    public KRB_CharacterController CharacterController;

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
    //CharacterActions refers to a UI prefab.
    public CharacterActions CharacterActions;
    //Change below to have a potentially 'infinite' number of actions. See how UI will mix with this.
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
    private List<GameObject> _currentTargetingIndicators = new List<GameObject>();
    [SerializeField] [ReadOnlyInspector] private ActionDescription _currentSelectedAction;
    [SerializeField] [ReadOnlyInspector] private List<Character> _viableTargets = new List<Character>();
    private bool _targetingAllViableTargets = false;

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
            case BattleState.SelectingAction:
                break;
            case BattleState.Targeting:
                //Alter this with UI ActionMap controls for battle.
                //if (Input.GetAxis("Mouse ScrollWheel") > 0) //Up/Down scrolling
                //{
                //    _currentTargetingIndex += 1;
                //    if (_currentTargetingIndex > _viableTargets.Count - 1)
                //    {
                //        _currentTargetingIndex = 0;
                //    }
                //    _currentTargetingIndicator.transform.position = SetTargetingIndicatorPosition(_currentTargetingIndex);
                //}
                //if (Input.GetAxis("Mouse ScrollWheel") < 0)
                //{
                //    _currentTargetingIndex -= 1;
                //    if (_currentTargetingIndex < 0)
                //    {
                //        _currentTargetingIndex = _viableTargets.Count - 1;
                //    }
                //    _currentTargetingIndicator.transform.position = SetTargetingIndicatorPosition(_currentTargetingIndex);
                //}
                if (Input.GetMouseButtonDown(0)) //Select
                {
                    if (_viableTargets.Count < 1 || _currentSelectedAction == null) return;
                    List<Character> selectedTargets = new List<Character>();
                    if (_targetingAllViableTargets)
                    {
                        foreach (Character target in _viableTargets)
                        {
                            selectedTargets.Add(target);
                        }
                    }
                    else
                    {
                        selectedTargets.Add(_viableTargets[_currentTargetingIndex]);
                    }
                    ActionConfirm(_currentSelectedAction, selectedTargets);
                }
                break;
            case BattleState.Busy:
                break;
            case BattleState.Sliding:
                Vector3 tmpPos = Vector3.Lerp(_slideOriginalPosition, _slideTargetPosition, _slideTimer / _slideDuration);
                CharacterController.Motor.SetPosition(tmpPos);
                _slideTimer += Time.deltaTime;
                if (_slideTimer > _slideDuration)
                {
                    _slideTimer = 0f;
                    CharacterController.Motor.SetPosition(_slideTargetPosition);
                    _onSlideComplete();
                }
                break;
            default:
                break;
        }
    }

    public void SelectTarget()
    {
        if (_viableTargets.Count < 1 || _currentSelectedAction == null) return;
        List<Character> selectedTargets = new List<Character>();
        if (_targetingAllViableTargets)
        {
            foreach (Character target in _viableTargets)
            {
                selectedTargets.Add(target);
            }
        }
        else
        {
            selectedTargets.Add(_viableTargets[_currentTargetingIndex]);
        }
        ActionConfirm(_currentSelectedAction, selectedTargets);
    }
    public void ScrollViableTargetForward()
    {
        _currentTargetingIndex += 1;
        if (_currentTargetingIndex > _viableTargets.Count - 1)
        {
            _currentTargetingIndex = 0;
        }
        _currentTargetingIndicators[0].transform.position = SetTargetingIndicatorPosition(_currentTargetingIndex);
    }
    public void ScrollViableTargetBackward()
    {
        _currentTargetingIndex -= 1;
        if (_currentTargetingIndex < 0)
        {
            _currentTargetingIndex = _viableTargets.Count - 1;
        }
        _currentTargetingIndicators[0].transform.position = SetTargetingIndicatorPosition(_currentTargetingIndex);
    }

    public void TransitionToState(BattleState fromstate, BattleState state)
    {
        BattleState fromState = _state;
        switch (_state)
        {
            case BattleState.Idle:
                break;
            case BattleState.SelectingAction:
                CloseActionsMenu();
                break;
            case BattleState.Targeting:
                _currentTargetingIndex = 0;
                foreach (GameObject indicator in _currentTargetingIndicators)
                {
                    Destroy(indicator);
                }
                _currentTargetingIndicators.Clear();
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
            case BattleState.SelectingAction:
                OpenActionsMenu();
                break;
            case BattleState.Targeting:
                _currentTargetingIndex = 0;
                foreach(GameObject indicator in _currentTargetingIndicators)
                {
                    Destroy(indicator);
                }
                _currentTargetingIndicators.Clear();

                if (_targetingAllViableTargets)
                {
                    for (int i = 0; i < _viableTargets.Count; i++)
                    {
                        GameObject newIndicator = Instantiate<GameObject>(TargetingIndicator, SetTargetingIndicatorPosition(0), Quaternion.identity);
                        _currentTargetingIndicators.Add(newIndicator);
                        _currentTargetingIndicators[i].transform.position = SetTargetingIndicatorPosition(i);
                    }
                }
                else
                {
                    GameObject newIndicator = Instantiate<GameObject>(TargetingIndicator, SetTargetingIndicatorPosition(0), Quaternion.identity);
                    _currentTargetingIndicators.Add(newIndicator);
                    _currentTargetingIndicators[0].transform.position = SetTargetingIndicatorPosition(0);
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

    private void ActionConfirm(ActionDescription action, List<Character> targets)
    {
        //CharacterActions.CloseAll();
        _currentSelectedAction = null;
        TransitionToState(_state, BattleState.Busy);
        List<CharacterBattle> targetsBattle = new List<CharacterBattle>();
        foreach(Character character in targets)
        {
            targetsBattle.Add(character.Battle);
        }
        UseActionOn(targetsBattle, action, () =>
        {
            BattleManager.instance.GetNextInitiative();
        });
    }

    private void PurgeAllActionSlots()
    {
        for (int i = _actions.Count-1; i > 0; i--)
        {
            _actions[i].ActionSelected -= OnActionSelected;
            _actions.Remove(_actions[i]);
        }
    }

    public void RollInitiative(float speed)
    {
        float dieRoll = UnityEngine.Random.Range(0f, 0f);
        Initiative =  dieRoll + speed;
        TransitionToState(_state, BattleState.Idle);
    }

    public void UseActionOn(List<CharacterBattle> targets, ActionDescription action, Action onActionComplete)
    {
        foreach(CharacterBattle target in targets)
        {
            Debug.Log(this + " used " + action.Name + " on " + target + "!");
        }

        if (action.doesSlide)
        {
            Vector3 finalPosition = Vector3.zero;
            foreach(CharacterBattle target in targets)
            {
                Vector3 slideTargetPosition = target.GetPosition() + (GetPosition() - target.GetPosition()).normalized * 2f;
                finalPosition += slideTargetPosition;
            }
            finalPosition = finalPosition / (targets.Count);

            SlideToPosition(finalPosition, () =>
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
        _targetingAllViableTargets = action.TargetsAllViableTargets;
        return _viableTargets;
    }

    public Character GetRandomTargetFromViableTargets()
    {
        List<Character> viableTargets = _viableTargets;
        int randomIndex = UnityEngine.Random.Range(0, viableTargets.Count);
        return viableTargets[randomIndex];
    }

    private void OpenActionsMenu()
    {
        if (!CharacterActions) return;
        CharacterActions.OpenAttacks();
    }

    private void CloseActionsMenu()
    {
        if (!CharacterActions) return;
        CharacterActions.CloseAll();
    }
}
