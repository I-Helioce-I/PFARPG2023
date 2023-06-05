using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterActions : MonoBehaviour
{
    [Header("References")]
    public CharacterBattle CharacterBattle;

    [Header("Action quads")]
    public UI_ActionQuad Attacks;
    public UI_ActionQuad Spells;
    public UI_ActionQuad Items;

    public void SetActions(ActionDescription[] actions)
    {
        for (int i = 0; i < actions.Length; i++)
        {
            ActionDescription action = actions[i];
            if (i >= 0 && i <= 3)
            {
                int index = i;
                Items.Actions[index].SetAction(action);
                //Set actions in appropriate Items actions
            }
            else if (i >= 4 && i <= 7)
            {
                int index = i - 4;
                Attacks.Actions[index].SetAction(action);
                //Set actions in appropriate Attacks actions
            }
            else if (i >= 8 && i <= 11)
            {
                int index = i - 8;
                Spells.Actions[index].SetAction(action);
                //Set actions in appropriate Spells actions
            }
        }
    }

    public void CloseAll()
    {
        Attacks.CloseAllActionSlots();
        Attacks.gameObject.SetActive(false);
        Spells.CloseAllActionSlots();
        Spells.gameObject.SetActive(false);
        Items.CloseAllActionSlots();
        Items.gameObject.SetActive(false);
    }

    public void OpenAttacks()
    {
        Attacks.gameObject.SetActive(true);
        Spells.CloseAllActionSlots();
        Spells.gameObject.SetActive(false);
        Items.CloseAllActionSlots();
        Items.gameObject.SetActive(false);
    }

    public void OpenSpells()
    {
        Attacks.CloseAllActionSlots();
        Attacks.gameObject.SetActive(false);
        Spells.gameObject.SetActive(true);
        Items.CloseAllActionSlots();
        Items.gameObject.SetActive(false);
    }

    public void OpenItems()
    {
        Attacks.CloseAllActionSlots();
        Attacks.gameObject.SetActive(false);
        Spells.CloseAllActionSlots();
        Spells.gameObject.SetActive(false);
        Items.gameObject.SetActive(true);
    }
}
