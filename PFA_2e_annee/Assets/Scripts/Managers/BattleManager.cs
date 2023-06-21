using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public enum BattleState
    {
        None,
        WaitingForAction,
        Busy,
        Starting,
        Victory,
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
    [SerializeField][ReadOnlyInspector] private int _roundsSinceStartOfBattle;

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
    public UI_PlayerCharacterCombatSheet PlayerCharacterCombatSheet;
    public UI_EnemyCharacterCombatSheet EnemyCharacterCombatSheet;
    public Transform PlayerCharacterCombatSheetParent;
    public Transform EnemyCharacterCombatSheetParent;
    private UI_CombatTimelapse _instantiatedTimelapse;
    private List<UI_PlayerCharacterCombatSheet> _instantiatedPCSheets = new List<UI_PlayerCharacterCombatSheet>();
    private List<UI_EnemyCharacterCombatSheet> _instantiatedEnemySheets = new List<UI_EnemyCharacterCombatSheet>();
    public List<UI_EnemyCharacterCombatSheet> EnemySheets => _instantiatedEnemySheets;
    public List<UI_PlayerCharacterCombatSheet> PlayerSheets => _instantiatedPCSheets;
    public GameObject FightScreen;
    public UI_CombatLootScreen LootScreen;
    public UI_FloatingText FloatingTextPrefab;

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
        CombatCanvas.gameObject.SetActive(false);

        TransitionToState(_state);

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
                if (ActiveCharacterIsStunned())
                {
                    _activeCharacter.Battle.ConditionHandler.IsStunned = false;
                    GetNextInitiative();
                }
                else
                {
                    if (_activeCharacter.Battle.ConditionHandler.IsDefending)
                    {
                        _activeCharacter.Battle.ConditionHandler.IsDefending = false;
                        _activeCharacter.Battle.CharacterAnimatorHandler.Animator.SetBool("isDefending", false);
                    }

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
                            foreach (Character character in viableTargets)
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
                }
                break;
            case BattleState.Busy:
                break;
            case BattleState.Starting:
                FightScreen.SetActive(true);
                CameraManager.instance.SmoothCurrentCameraRotation(Vector3.zero, new Vector3(30f, 0, 0), 2.5f, () =>
                {
                    StartCombatGameplay();
                });
                break;
            case BattleState.Victory:
                LootScreen.gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }

    private bool ActiveCharacterIsStunned()
    {
        return _activeCharacter.Battle.ConditionHandler.IsStunned;
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
        UIManager.instance.CurrentState = UIManager.UIState.Combat;
        CombatCanvas.gameObject.SetActive(true);
        _roundsSinceStartOfBattle = 0;

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
            character.CharacterDowned -= OnCharacterDowned;
            character.CharacterDowned += OnCharacterDowned;
            if (character.Battle.CharacterAnimatorHandler) character.Battle.CharacterAnimatorHandler.Animator.SetBool("isInCombat", true);
        }

        foreach(Character character in _playerCharactersInBattle)
        {
            UI_PlayerCharacterCombatSheet characterCombatSheet = Instantiate<UI_PlayerCharacterCombatSheet>(PlayerCharacterCombatSheet, PlayerCharacterCombatSheetParent);
            characterCombatSheet.InitializeSheet(character, character.Stats, character.battleSprite);
            _instantiatedPCSheets.Add(characterCombatSheet);
        }

        foreach(Character character in _enemyCharactersInBattle)
        {
            UI_EnemyCharacterCombatSheet characterCombatSheet = Instantiate<UI_EnemyCharacterCombatSheet>(EnemyCharacterCombatSheet, EnemyCharacterCombatSheetParent);
            characterCombatSheet.InitializeSheet(character, character.Stats, character.battleSprite);
            _instantiatedEnemySheets.Add(characterCombatSheet);
        }

        UI_CombatTimelapse timelapse = Instantiate<UI_CombatTimelapse>(CombatTimelapse, CombatCanvas);
        _instantiatedTimelapse = timelapse;
        _instantiatedTimelapse.BattleManager = this;
        _instantiatedTimelapse.SetEventListening();

        BattleStarted?.Invoke();
        TurnOrderCreated?.Invoke(allCharacters);
        RollAllInitiatives();
        _instantiatedTimelapse.gameObject.SetActive(false);
        TransitionToState(BattleState.Starting);
        //_state = BattleState.WaitingForAction;
    }

    public void StartCombatGameplay()
    {
        FightScreen.SetActive(false);
        _instantiatedTimelapse.gameObject.SetActive(true);
        _instantiatedTimelapse.SetEventListening();
        SetActiveCharacter();
        TransitionToState(BattleState.WaitingForAction);
    }

    public void TransitionOutOfBattle()
    {
        _instantiatedTimelapse.gameObject.SetActive(true);
        UIManager.instance.Transitioner.TransitionOutOfCombat(1f, () =>
        {
            EndBattle();
        });
    }

    private void EndBattle()
    {
        //Get the experience and loot screens.
        //Make victorious characters play their victory anim.
        //Once last screen is validated, ask Loader to transition (normally) back to exploration.
        //Transition to BattleState.none.

        Destroy(_instantiatedTimelapse.gameObject);
        _instantiatedTimelapse = null;
        foreach(UI_PlayerCharacterCombatSheet PCCombatSheet in _instantiatedPCSheets)
        {
            Destroy(PCCombatSheet.gameObject);
        }
        _instantiatedPCSheets.Clear();
        foreach (UI_EnemyCharacterCombatSheet EnemyCombatSheet in _instantiatedEnemySheets)
        {
            Destroy(EnemyCombatSheet.gameObject);
        }
        _instantiatedEnemySheets.Clear();
        //Destroy instantiated characters.
        foreach (Character character in GetAllCharactersInBattle())
        {
            RemoveCharacter(character);
        }

        //for (int i = CombatCanvas.childCount - 1; i > 0; i--)
        //{
        //    Destroy(CombatCanvas.GetChild(i));
        //}
        TransitionToState(BattleState.None);
        CombatCanvas.gameObject.SetActive(false);
        GameManager.instance.CurrentState = GameManager.GameState.Exploration;
        UIManager.instance.CurrentState = UIManager.UIState.HUD;
        GameManager.instance.DragonCombat.SetActive(false);
    }

    private void OnCharacterDowned(Character character)
    {
        RemoveCharacter(character);
    }

    private void RollAllInitiatives()
    {
        List<Character> allCharacters = GetAllCharactersInBattle();
        foreach (Character character in allCharacters)
        {
            character.Battle.RollInitiative(character.Stats.Speed.CurrentValue);
            if (character.Battle.ConditionHandler.InitiativeFirstInLine)
            {
                character.Battle.Initiative += 999f;
                character.Battle.ConditionHandler.InitiativeFirstInLine = false;
            }
        }
        allCharacters.Sort(CompareInitiativeOrder);
        _turnOrder = allCharacters;

        TurnOrderSet?.Invoke(_turnOrder);
    }

    public void GetNextInitiative()
    {
        //Check if there are still enemies on enemy side.
        if (_enemyCharactersInBattle.Count <= 0)
        {

            foreach(Character playerCharacter in _playerCharactersInBattle)
            {
                if (playerCharacter != _playerCharactersInBattle[0]) playerCharacter.Battle.CharacterAnimatorHandler.PlayAnim("Victory");
                else playerCharacter.Battle.CharacterAnimatorHandler.PlayAnimThenAction("Victory", () =>
                {
                    TransitionToState(BattleState.Victory);
                    _instantiatedTimelapse.gameObject.SetActive(false);
                });
            }
         
            return;
        }

        if (_turnOrder.Count > 1)
        {
            _turnOrder.Remove(_turnOrder[0]);
            SetActiveCharacter();
        }
        else
        {
            _turnOrder.Clear();
            _roundsSinceStartOfBattle += 1;
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
        if (character.CharacterConditions.IsStunned)
        {
            character.CharacterConditions.IsStunned = false;
        }

        if (_playerCharactersInBattle.Contains(character))
        {
            if (_turnOrder.Contains(character))
            {
                _turnOrder.Remove(character);
                TurnOrderAltered?.Invoke(_turnOrder);
            }
            _playerCharactersInBattle.Remove(character);
            character.CharacterDowned -= OnCharacterDowned;
            character.Battle.CharacterAnimatorHandler.PlayAnimThenAction("Die", () =>
            {
                Destroy(character.gameObject);
            });
            
            //Make character have death animation, and then destroy it later.
        }
        else if (_enemyCharactersInBattle.Contains(character))
        {
            if (_turnOrder.Contains(character))
            {
                _turnOrder.Remove(character);
                TurnOrderAltered?.Invoke(_turnOrder);
            }
            _enemyCharactersInBattle.Remove(character);
            character.CharacterDowned -= OnCharacterDowned;
            character.Battle.CharacterAnimatorHandler.PlayAnimThenAction("Die", () =>
            {
                Destroy(character.gameObject);
            });
            //Remove character from play, and then later destroy it.
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
