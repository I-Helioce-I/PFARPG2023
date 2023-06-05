using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_CombatTimelapse : MonoBehaviour
{
    [Header("Battle manager")]
    public BattleManager BattleManager;

    [Header("Prefab")]
    public UI_CombatTimelapseCharacterIcon Prefab;

    private List<UI_CombatTimelapseCharacterIcon> _characterIcons = new List<UI_CombatTimelapseCharacterIcon>();
    private List<Character> _charactersInCombat = new List<Character>();

    //private void OnEnable()
    //{
    //    BattleManager.TurnOrderSet -= OnTurnOrderSet;
    //    BattleManager.TurnOrderSet += OnTurnOrderSet;

    //    BattleManager.TurnOrderAltered -= OnTurnOrderAltered;
    //    BattleManager.TurnOrderAltered += OnTurnOrderAltered;

    //    BattleManager.TurnOrderCreated -= OnTurnOrderCreated;
    //    BattleManager.TurnOrderCreated += OnTurnOrderCreated;
    //}

    //private void OnDisable()
    //{
    //    BattleManager.TurnOrderSet -= OnTurnOrderSet;

    //    BattleManager.TurnOrderAltered -= OnTurnOrderAltered;

    //    BattleManager.TurnOrderCreated -= OnTurnOrderCreated;
    //}

    public void SetEventListening()
    {
        BattleManager.TurnOrderSet -= OnTurnOrderSet;
        BattleManager.TurnOrderSet += OnTurnOrderSet;

        BattleManager.TurnOrderAltered -= OnTurnOrderAltered;
        BattleManager.TurnOrderAltered += OnTurnOrderAltered;

        BattleManager.TurnOrderCreated -= OnTurnOrderCreated;
        BattleManager.TurnOrderCreated += OnTurnOrderCreated;
    }

    private void OnTurnOrderCreated(List<Character> characters)
    {
        _charactersInCombat = characters;

        foreach (Character character in characters)
        {
            UI_CombatTimelapseCharacterIcon newIcon = Instantiate<UI_CombatTimelapseCharacterIcon>(Prefab, this.transform);
            newIcon.RepresentedCharacter = character;
            newIcon.ForceSetValue(0f);
            newIcon.CharacterIcon.color = character.Color;
            _characterIcons.Add(newIcon);
        }
    }

    private void OnTurnOrderSet(List<Character> order)
    {
        for (int i = 0; i < order.Count; i++)
        {
            foreach (UI_CombatTimelapseCharacterIcon characterIcon in _characterIcons)
            {
                if (characterIcon.RepresentedCharacter == order[i])
                {
                    characterIcon.SetDestinationValue(1 - (0.1f * i));
                }
            }
        }
        //Dictionary<int, Character> indexToCharacter = new Dictionary<int, Character>();
        //order[n] has a slider value of 1-(0.1f*n). Up to order[9] is shown this way - the rest is hidden; but our battle system is not gonna have more than 9 characters at once,
        //although certain characters can have multiple actions and so appear multiple times on the initiative order.
    }

    private void OnTurnOrderAltered(List<Character> order)
    {
        //Check for every character in order if they are present. If they don't have icon, create it. If they do, show it.
        foreach (Character character in order)
        {
            UI_CombatTimelapseCharacterIcon icon = null;
            foreach(UI_CombatTimelapseCharacterIcon characterIcon in _characterIcons)
            {
                if (characterIcon.RepresentedCharacter == character)
                {
                    icon = characterIcon;
                }
            }

            if (!icon)
            {
                UI_CombatTimelapseCharacterIcon newIcon = Instantiate<UI_CombatTimelapseCharacterIcon>(Prefab, this.transform);
                newIcon.RepresentedCharacter = character;
                newIcon.ForceSetValue(0f);
                newIcon.CharacterIcon.color = character.Color;
                _characterIcons.Add(newIcon);
            }
            else
            {
                icon.SetIconVisible(true);
            }
        }

        //Check for every character in characterIcon if they are in order list
        foreach(UI_CombatTimelapseCharacterIcon characterIcon in _characterIcons)
        {
            bool isInOrder = false;
            foreach(Character character in order)
            {
                if (characterIcon.RepresentedCharacter == character)
                {
                    isInOrder = true;
                }
            }

            if (!isInOrder)
            {
                characterIcon.SetIconVisible(false);
            }
        }

        for (int i = 0; i < order.Count; i++)
        {
            foreach (UI_CombatTimelapseCharacterIcon characterIcon in _characterIcons)
            {
                if (characterIcon.RepresentedCharacter == order[i])
                {
                    characterIcon.SetDestinationValue(1 - (0.1f * i));
                }
            }
        }

        //Whenever a turn is moved, removed, etc, the entire turn order is checked.
        //If a character isn't in the order anymore, hide their icon.
        //If a character from the order doesn't have a characterIcon, create it.
        //Create new List<UI_whaterv>.
        //make a for i etc, and check through each UI_CombatCharacterWhatever to see if the character is equal to the order[i]. If yes, put it in new List.
        //use this new List as _characterIcons.
    }

    public void GenerateTimelapse(List<Character> characters)
    {
        _charactersInCombat = characters;

        foreach(Character character in characters)
        {
            UI_CombatTimelapseCharacterIcon newIcon = Instantiate<UI_CombatTimelapseCharacterIcon>(Prefab, this.transform);
            newIcon.RepresentedCharacter = character;
            _characterIcons.Add(newIcon);
        }
    }

    private void RemoveCharacter(Character character)
    {
        foreach(UI_CombatTimelapseCharacterIcon characterIcon in _characterIcons)
        {
            if (characterIcon.RepresentedCharacter == character)
            {
                _characterIcons.Remove(characterIcon);
                Destroy(characterIcon.gameObject);
            }
        }
        if (_charactersInCombat.Contains(character))
        {
            _charactersInCombat.Remove(character);
        }
    }

    private void AddCharacter(Character character)
    {
        UI_CombatTimelapseCharacterIcon newIcon = Instantiate<UI_CombatTimelapseCharacterIcon>(Prefab, this.transform);
        newIcon.RepresentedCharacter = character;
        _characterIcons.Add(newIcon);
        _charactersInCombat.Add(character);
    }

    private void SortTimelapse()
    {
        List<Character> order = BattleManager.GetTurnOrder;
        List<Character> allCharactersInBattle = BattleManager.GetAllCharactersInBattle();
        foreach(UI_CombatTimelapseCharacterIcon characterIcon in _characterIcons)
        {
            bool isInOrder = false;
            foreach(Character character in order)
            {
                if (characterIcon.RepresentedCharacter == character)
                {
                    isInOrder = true;
                }
            }
            bool isInBattle = false;
            foreach (Character character in allCharactersInBattle)
            {
                if (characterIcon.RepresentedCharacter == character)
                {
                    isInBattle = true;
                }
            }


        }
        //If character is in order and is in battle, then ConvertIndexToTimelapsePosition.
        //If character is not in order and is in battle, they can't act anymore. Hide their icon.
        //If character is not in order and is not in battle, they are dead. Destroy their icon.
    }
}
