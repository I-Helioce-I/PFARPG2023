using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatEntity : MonoBehaviour
{
    public List<Character> CharactersRepresentedInCombat = new List<Character>();

    public void StartCombatOnInteract(InteractibleHandler handler)
    {
        CombatEntity handlerEntities = handler.GetComponent<CombatEntity>();

        Player.instance.ChangeActionMap("UI");
        UIManager.instance.Transitioner.TransitionIntoCombat(1f, () =>
        {
            BattleManager.instance.StartBattle(handlerEntities.CharactersRepresentedInCombat, CharactersRepresentedInCombat);
        });
    }

    public void StartCombatWith(List<Character> otherCharacters)
    {
        Player.instance.ChangeActionMap("UI");
        UIManager.instance.Transitioner.TransitionIntoCombat(1f, () =>
        {
            BattleManager.instance.StartBattle(otherCharacters, CharactersRepresentedInCombat);
        });
    }
}
