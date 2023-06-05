using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public enum BattleState
    {
        WaitingForAction,
        Busy,
    }
    public delegate void BattleEvent();
    public event BattleEvent BattleStarted = null;
    public event BattleEvent BattleEnded = null;
    public delegate void TurnOrderEvent(List<Character> order);
    public event TurnOrderEvent TurnOrderCreated = null;
    public event TurnOrderEvent TurnOrderSet = null;
    public event TurnOrderEvent TurnOrderAltered = null;

    public static BattleManager instance;

    [Header("State")]
    [SerializeField][ReadOnlyInspector] private BattleState _state;

    [Header("Player Team")]
    [SerializeField] private List<Character> _playerCharactersInBattle = new List<Character>();
    public List<Character> GetPlayerCharactersInBattle
    {
        get
        {
            return _playerCharactersInBattle;
        }
    }

    [Header("Player UI")]
    public Transform Canvas;
    public CharacterActions CharacterActionsUI;
    public UI_CombatTimelapse CombatTimelapse;

    [Header("Enemy Team")]
    [SerializeField] private List<Character> _enemyCharactersInBattle = new List<Character>();
    public List<Character> GetEnemyCharactersInBattle
    {
        get
        {
            return _enemyCharactersInBattle;
        }
    }

    [Header("Turn order")]
    [SerializeField] private List<Character> _turnOrder = new List<Character>();
    [SerializeField][ReadOnlyInspector] private Character _activeCharacter;
    public List<Character> GetTurnOrder
    {
        get
        {
            return _turnOrder;
        }
    }

    [Header("DEBUG")]
    [SerializeField] public Character _player;
    [SerializeField] public Character _enemy;
    public List<Character> PlayerCharactersInBattle
    {
        get
        {
            return _playerCharactersInBattle;
        }
    }
    public List<Character> EnemyCharactersInBattle
    {
        get
        {
            return _enemyCharactersInBattle;
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
        StartBattle(_playerCharactersInBattle, _enemyCharactersInBattle);
    }

    public void TransitionToState(BattleState state)
    {
        _state = state;
        switch (_state)
        {
            case BattleState.WaitingForAction:
                if (_playerCharactersInBattle.Contains(_activeCharacter))
                {
                    _activeCharacter.Battle.OpenActionsMenu();

                }
                else if (_enemyCharactersInBattle.Contains(_activeCharacter))
                {
                    TransitionToState(BattleState.Busy);
                    ActionDescription action = _activeCharacter.Battle.GetRandomAction();
                    List<Character> viableTargets = _activeCharacter.Battle.GetViableTargets(action);
                    Character target = _activeCharacter.Battle.GetRandomTargetFromViableTargets();

                    _activeCharacter.Battle.UseActionOn(target.Battle, action, () =>
                    {
                        GetNextInitiative();
                    });
                }
                break;
            case BattleState.Busy:
                break;
            default:
                break;
        }
    }

    public void StartBattle(List<Character> playerCharacters, List<Character> enemyCharacters)
    {
        _playerCharactersInBattle = playerCharacters;
        foreach(Character playerCharacter in _playerCharactersInBattle)
        {
            CharacterActions characterActionsUI = Instantiate<CharacterActions>(CharacterActionsUI, Canvas);
            characterActionsUI.CloseAll();
            characterActionsUI.CharacterBattle = playerCharacter.Battle;
            playerCharacter.Battle.CharacterActions = characterActionsUI;
            playerCharacter.Battle.InitializeCharacterActions();
        }
        _enemyCharactersInBattle = enemyCharacters;
        List<Character> allCharacters = GetAllCharactersInBattle();
        foreach(Character character in allCharacters)
        {
            character.Battle.BattleManager = this;
        }

        UI_CombatTimelapse timelapse = Instantiate<UI_CombatTimelapse>(CombatTimelapse, Canvas);
        timelapse.BattleManager = this;
        timelapse.SetEventListening();

        BattleStarted?.Invoke();
        TurnOrderCreated?.Invoke(allCharacters);

        RollAllInitiatives();
        SetActiveCharacter();
        TransitionToState(BattleState.WaitingForAction);
        //_state = BattleState.WaitingForAction;

    }

    private void RollAllInitiatives()
    {
        List<Character> allCharacters = GetAllCharactersInBattle();
        foreach (Character character in allCharacters)
        {
            character.Battle.RollInitiative(character.Stats.Speed.CurrentValue);
        }
        allCharacters.Sort(CompareInitiativeOrder);
        _turnOrder = allCharacters;

        TurnOrderSet?.Invoke(_turnOrder);
    }

    public void GetNextInitiative()
    {
        if (_turnOrder.Count > 1)
        {
            _turnOrder.Remove(_turnOrder[0]);
            SetActiveCharacter();
        }
        else
        {
            _turnOrder.Clear();
            RollAllInitiatives();
            SetActiveCharacter();
        }

        TurnOrderAltered?.Invoke(_turnOrder);
        TransitionToState(BattleState.WaitingForAction);
        //_state = BattleState.WaitingForAction;
    }

    private int CompareInitiativeOrder(Character characterA, Character characterB)
    {
        if (characterA.Battle.Initiative > characterB.Battle.Initiative) return -1;
        else if (characterA.Battle.Initiative < characterB.Battle.Initiative) return 1;
        else return 0;
    }

    public List<Character> GetAllCharactersInBattle()
    {
        List<Character> allCharacters = new List<Character>();

        foreach(Character playerCharacter in _playerCharactersInBattle)
        {
            allCharacters.Add(playerCharacter);
        }
        foreach(Character enemyCharacter in _enemyCharactersInBattle)
        {
            allCharacters.Add(enemyCharacter);
        }

        return allCharacters;
    }

    private void SetActiveCharacter(int index = 0)
    {
        _activeCharacter = _turnOrder[index];
    }

    private void RemoveCharacter(Character character)
    {
        if (_playerCharactersInBattle.Contains(character))
        {
            _playerCharactersInBattle.Remove(character);
            _turnOrder.Remove(character);
            TurnOrderAltered?.Invoke(_turnOrder);
        }
        else if (_enemyCharactersInBattle.Contains(character))
        {
            _enemyCharactersInBattle.Remove(character);
            _turnOrder.Remove(character);
            TurnOrderAltered?.Invoke(_turnOrder);
        }
        else
        {
            Debug.Log(character + " has not been found in the lists of player characters or enemy characters.");
            return;
        }
    }

    private void AddCharacter(Character character, bool isPlayer)
    {
        if (isPlayer)
        {
            if (!_playerCharactersInBattle.Contains(character))
            {
                _playerCharactersInBattle.Add(character);
            }
        }
        else
        {
            if (!_enemyCharactersInBattle.Contains(character))
            {
                _enemyCharactersInBattle.Add(character);
            }
        }
    }
}
