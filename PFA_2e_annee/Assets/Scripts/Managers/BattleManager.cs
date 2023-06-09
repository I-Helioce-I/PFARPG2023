using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public enum BattleState
    {
        None,
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
    [SerializeField] private Transform _playerParent;
    [SerializeField] private List<Character> _playerCharactersInBattle = new List<Character>();
    public List<Character> GetPlayerCharactersInBattle
    {
        get
        {
            return _playerCharactersInBattle;
        }
    }
    [SerializeField] private List<Transform> _allyTransforms = new List<Transform>();

    [Header("Player UI")]
    public Transform CombatCanvas;
    public CharacterActions CharacterActionsUI;
    public UI_CombatTimelapse CombatTimelapse;

    [Header("Enemy Team")]
    [SerializeField] private Transform _enemyParent;
    [SerializeField] private List<Character> _enemyCharactersInBattle = new List<Character>();
    public List<Character> GetEnemyCharactersInBattle
    {
        get
        {
            return _enemyCharactersInBattle;
        }
    }
    [SerializeField] private List<Transform> _enemyTransforms = new List<Transform>();

    [Header("Turn order")]
    [SerializeField] private List<Character> _turnOrder = new List<Character>();
    [SerializeField][ReadOnlyInspector] private Character _activeCharacter;
    public Character ActiveCharacter
    {
        get
        {
            return _activeCharacter;
        }
    }
    public List<Character> GetTurnOrder
    {
        get
        {
            return _turnOrder;
        }
    }
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

    [Header("DEBUG")]
    [SerializeField] private bool _startBattleOnStartGame = true;

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
        if (_startBattleOnStartGame) StartBattle(_playerCharactersInBattle, _enemyCharactersInBattle);
    }

    public void TransitionToState(BattleState state)
    {
        _state = state;
        switch (_state)
        {
            case BattleState.None:
                _playerCharactersInBattle.Clear();
                _enemyCharactersInBattle.Clear();
                _turnOrder.Clear();
                break;
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
                    List<CharacterBattle> chosenTargets = new List<CharacterBattle>();
                    if (action.TargetsAllViableTargets)
                    {
                        foreach(Character character in viableTargets)
                        {
                            chosenTargets.Add(character.Battle);
                        }
                    }
                    else
                    {
                        Character target = _activeCharacter.Battle.GetRandomTargetFromViableTargets();
                        chosenTargets.Add(target.Battle);
                    }


                    _activeCharacter.Battle.UseActionOn(chosenTargets, action, () =>
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
        //Transition GameManager state to Combat.
        //Ask for loader to load a new scene according to the environment in which the player is in.
        //Use BattleFadeOut
        //Once scene is loaded, 
        //Instantiate all characters in their positions + rotations depending on their team.
        //Transition to battlestate.busy.
        //Then do the following.

        GameManager.instance.CurrentState = GameManager.GameState.Combat;

        //Instantiate characters on arena
        List<Character> createdPlayers = new List<Character>();
        float allyLineDistance = Vector3.Distance(_allyTransforms[0].position, _allyTransforms[1].position);
        int allyCount = playerCharacters.Count;
        float allyLineDistanceStep = allyLineDistance / allyCount;
        float allyLineDistanceStepFirst = allyLineDistanceStep / 2f;

        for (int i = 0; i < playerCharacters.Count; i++)
        {
            Character newCharacter = Instantiate<Character>(playerCharacters[i], _playerParent);
            Vector3 instantiatedPos = _allyTransforms[0].position + (_playerParent.right * allyLineDistanceStepFirst) + (_playerParent.right * (i * allyLineDistanceStep));
            newCharacter.CharacterController.Motor.SetPositionAndRotation(instantiatedPos, _playerParent.rotation);
            createdPlayers.Add(newCharacter);
        }

        List<Character> createdEnemies = new List<Character>();
        float enemyLineDistance = Vector3.Distance(_enemyTransforms[0].position, _enemyTransforms[1].position);
        int enemyCount = enemyCharacters.Count;
        float enemyLineDistanceStep = enemyLineDistance / enemyCount;
        float enemyLineDistanceStepFirst = enemyLineDistanceStep / 2f;

        for (int i = 0; i < enemyCharacters.Count; i++)
        {
            Character newCharacter = Instantiate<Character>(enemyCharacters[i], _enemyParent);
            Vector3 instantiatedPos = _enemyTransforms[1].position + (_enemyParent.right * enemyLineDistanceStepFirst) + (_enemyParent.right * (i * enemyLineDistanceStep));
            newCharacter.CharacterController.Motor.SetPositionAndRotation(instantiatedPos, _enemyParent.rotation);
            createdEnemies.Add(newCharacter);
        }

        _playerCharactersInBattle = createdPlayers;
        //foreach(Character playerCharacter in _playerCharactersInBattle)
        //{
        //    CharacterActions characterActionsUI = Instantiate<CharacterActions>(CharacterActionsUI, CombatCanvas);
        //    characterActionsUI.CloseAll();
        //    characterActionsUI.CharacterBattle = playerCharacter.Battle;
        //    playerCharacter.Battle.CharacterActions = characterActionsUI;
        //    playerCharacter.Battle.InitializeCharacterActions();
        //}
        _enemyCharactersInBattle = createdEnemies;

        List<Character> allCharacters = GetAllCharactersInBattle();
        foreach(Character character in allCharacters)
        {
            character.Battle.BattleManager = this;
        }

        UI_CombatTimelapse timelapse = Instantiate<UI_CombatTimelapse>(CombatTimelapse, CombatCanvas);
        timelapse.BattleManager = this;
        timelapse.SetEventListening();

        BattleStarted?.Invoke();
        TurnOrderCreated?.Invoke(allCharacters);

        RollAllInitiatives();
        SetActiveCharacter();
        TransitionToState(BattleState.WaitingForAction);
        //_state = BattleState.WaitingForAction;
    }

    public void EndBattle()
    {
        //Get the experience and loot screens.
        //Make victorious characters play their victory anim.
        //Once last screen is validated, ask Loader to transition (normally) back to exploration.
        //Transition to BattleState.none.

        for (int i = CombatCanvas.childCount - 1; i > 0; i--)
        {
            Destroy(CombatCanvas.GetChild(i));
        }
        GameManager.instance.CurrentState = GameManager.GameState.Exploration;
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
