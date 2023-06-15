using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_CombatMenu : MonoBehaviour
{
    public enum UICombatMenuState
    {
        Closed,
        WaitingInstruction,
        NavigateTabs,
        NavigateActions,
    }
    [SerializeField][ReadOnlyInspector] private UICombatMenuState _internalState = UICombatMenuState.Closed;
    public UICombatMenuState CurrentState
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

    public GameObject CombatMenu;
    public GameObject ActionInfo;
    public List<UI_Tab> Tabs = new List<UI_Tab>();
    [Header("Last selected tab")]
    [ReadOnlyInspector] public UI_Tab LastSelectedTab = null;

    [Header("Action info")]
    private int hello = 0;
    private bool _isOpen = false;

    public void TransitionToState(UICombatMenuState state)
    {
        _internalState = state;

        if (state != UICombatMenuState.Closed)
        {
            CombatMenu.SetActive(true);
        }
        else
        {
            CombatMenu.SetActive(false);
        }

        switch (state)
        {
            case UICombatMenuState.Closed:
                break;
            case UICombatMenuState.WaitingInstruction:
                break;
            case UICombatMenuState.NavigateTabs:
                foreach(UI_Tab tab in Tabs)
                {
                    tab.Self.interactable = true;
                    foreach(Button button in tab.LinkedActions)
                    {
                        button.interactable = false;
                    }
                }
                if (LastSelectedTab != null)
                {
                    LastSelectedTab.Self.Select();               
                }
                else
                {
                    Tabs[0].Self.Select();
                }
                break;
            case UICombatMenuState.NavigateActions:
                foreach (UI_Tab tab in Tabs)
                {
                    tab.Self.interactable = false;
                }
                break;
            default:
                break;
        }
    }

    public void SelectTab(int tabIndex)
    {
        Tabs[tabIndex].Self.Select();
    }

    public void SelectTab(UI_Tab tab)
    {
        tab.Self.Select();
    }

    public void ShowActionInfo(ActionDescription action)
    {

    }

    public void OpenCombatMenu(CharacterBattle character)
    {
        if (_isOpen) return;
        _isOpen = true;
        //When you open the combat menu, all of the tabs of the combat menu will instantiate their ui_actionbuttons, and initialize them to correspond to the according ActionDescription. All of this according
        //to the character's currentstate.
        TransitionToState(UICombatMenuState.WaitingInstruction);

        CharacterTypeState currentCharacterState = character.CharacterStateHandler.CharacterTypeState;
        Debug.Log(character.CharacterStateHandler.CharacterTypeState);
        switch (currentCharacterState)
        {
            case CharacterTypeState.None:
                Debug.Log("CHARACTER HAS NO STATE WTF DO I DO");
                break;
            case CharacterTypeState.Solid:
                List<ActionDescription> SolidAttacks = new List<ActionDescription>();
                List<ActionDescription> SolidSpells = new List<ActionDescription>();
                List<ActionDescription> SolidItems = new List<ActionDescription>();
                foreach (ActionSet actionSet in character.ActionSets)
                {
                    switch (actionSet.SolidAction.Type)
                    {
                        case ActionDescription.ActionType.Attack:
                            SolidAttacks.Add(actionSet.SolidAction);
                            break;
                        case ActionDescription.ActionType.Spell:
                            SolidSpells.Add(actionSet.SolidAction);
                            break;
                        case ActionDescription.ActionType.Item:
                            SolidItems.Add(actionSet.SolidAction);
                            break;
                        default:
                            break;
                    }
                }
                
                List<UI_ActionButton> allSolidActionButtons = new List<UI_ActionButton>();
                foreach(UI_ActionButton actionButton in InstantiateAttacks(SolidAttacks))
                {
                    allSolidActionButtons.Add(actionButton);
                }
                foreach (UI_ActionButton actionButton in InstantiateSpells(SolidSpells))
                {
                    allSolidActionButtons.Add(actionButton);
                }
                foreach (UI_ActionButton actionButton in InstantiateItems(SolidItems))
                {
                    allSolidActionButtons.Add(actionButton);
                }

                character.SetCharacterActionListening(allSolidActionButtons);
                break;
            case CharacterTypeState.Liquid:
                List<ActionDescription> LiquidAttacks = new List<ActionDescription>();
                List<ActionDescription> LiquidSpells = new List<ActionDescription>();
                List<ActionDescription> LiquidItems = new List<ActionDescription>();
                foreach (ActionSet actionSet in character.ActionSets)
                {
                    switch (actionSet.SolidAction.Type)
                    {
                        case ActionDescription.ActionType.Attack:
                            LiquidAttacks.Add(actionSet.LiquidAction);
                            break;
                        case ActionDescription.ActionType.Spell:
                            LiquidSpells.Add(actionSet.LiquidAction);
                            break;
                        case ActionDescription.ActionType.Item:
                            LiquidItems.Add(actionSet.LiquidAction);
                            break;
                        default:
                            break;
                    }
                }
                List<UI_ActionButton> allLiquidActionButtons = new List<UI_ActionButton>();
                foreach (UI_ActionButton actionButton in InstantiateAttacks(LiquidAttacks))
                {
                    allLiquidActionButtons.Add(actionButton);
                }
                foreach (UI_ActionButton actionButton in InstantiateSpells(LiquidSpells))
                {
                    allLiquidActionButtons.Add(actionButton);
                }
                foreach (UI_ActionButton actionButton in InstantiateItems(LiquidItems))
                {
                    allLiquidActionButtons.Add(actionButton);
                }

                character.SetCharacterActionListening(allLiquidActionButtons);
                break;
            case CharacterTypeState.Gas:
                List<ActionDescription> GasAttacks = new List<ActionDescription>();
                List<ActionDescription> GasSpells = new List<ActionDescription>();
                List<ActionDescription> GasItems = new List<ActionDescription>();
                foreach (ActionSet actionSet in character.ActionSets)
                {
                    switch (actionSet.SolidAction.Type)
                    {
                        case ActionDescription.ActionType.Attack:
                            GasAttacks.Add(actionSet.GasAction);
                            break;
                        case ActionDescription.ActionType.Spell:
                            GasSpells.Add(actionSet.GasAction);
                            break;
                        case ActionDescription.ActionType.Item:
                            GasItems.Add(actionSet.GasAction);
                            break;
                        default:
                            break;
                    }
                }
                List<UI_ActionButton> allGasActionButtons = new List<UI_ActionButton>();
                foreach (UI_ActionButton actionButton in InstantiateAttacks(GasAttacks))
                {
                    allGasActionButtons.Add(actionButton);
                }
                foreach (UI_ActionButton actionButton in InstantiateSpells(GasSpells))
                {
                    allGasActionButtons.Add(actionButton);
                }
                foreach (UI_ActionButton actionButton in InstantiateItems(GasItems))
                {
                    allGasActionButtons.Add(actionButton);
                }

                character.SetCharacterActionListening(allGasActionButtons);
                break;
            case CharacterTypeState.TriplePoint:
                break;
            default:
                break;
        }     

        TransitionToState(UICombatMenuState.NavigateTabs);
    }

    private List<UI_ActionButton> InstantiateAttacks(List<ActionDescription> attacks)
    {
        List<UI_ActionButton> attackButtons = new List<UI_ActionButton>();
        foreach(ActionDescription action in attacks)
        {
            UI_ActionButton newUIActionButton = Instantiate<UI_ActionButton>(Tabs[0].ActionButtonPrefab, Tabs[0].LinkedActionsParent.transform);
            newUIActionButton.InitializeActionButton(action);
            Button newButton = newUIActionButton.GetComponent<Button>();
            Tabs[0].LinkedActions.Add(newButton);
            attackButtons.Add(newUIActionButton);
        }

        return attackButtons;
    }

    private List<UI_ActionButton> InstantiateSpells(List<ActionDescription> spells)
    {
        List<UI_ActionButton> spellButtons = new List<UI_ActionButton>();
        foreach (ActionDescription action in spells)
        {
            UI_ActionButton newUIActionButton = Instantiate<UI_ActionButton>(Tabs[1].ActionButtonPrefab, Tabs[1].LinkedActionsParent.transform);
            newUIActionButton.InitializeActionButton(action);
            Button newButton = newUIActionButton.GetComponent<Button>();
            Tabs[1].LinkedActions.Add(newButton);
            spellButtons.Add(newUIActionButton);
        }

        return spellButtons;
    }
    private List<UI_ActionButton> InstantiateItems(List<ActionDescription> items)
    {
        List<UI_ActionButton> itemButtons = new List<UI_ActionButton>();
        foreach (ActionDescription action in items)
        {
            UI_ActionButton newUIActionButton = Instantiate<UI_ActionButton>(Tabs[2].ActionButtonPrefab, Tabs[2].LinkedActionsParent.transform);
            newUIActionButton.InitializeActionButton(action);
            Button newButton = newUIActionButton.GetComponent<Button>();
            Tabs[2].LinkedActions.Add(newButton);
            itemButtons.Add(newUIActionButton);
        }

        return itemButtons;
    }

    public void CloseCombatMenu()
    {
        foreach(UI_Tab tab in Tabs)
        {
            foreach(Button button in tab.LinkedActions)
            {
                Destroy(button.gameObject);
            }
            tab.LinkedActions.Clear();
        }

        _isOpen = false;
        TransitionToState(UICombatMenuState.Closed);
    }

    public void ReturnToNavigateFromTargeting()
    {
        if (LastSelectedTab != null)
        {
            LastSelectedTab.OpenTab(true);
        }
        else
        {
            Tabs[0].OpenTab(true);
        }
    }
}
