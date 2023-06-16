using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public delegate void CharacterEvent(Character character);
    public event CharacterEvent CharacterDowned = null;

    [Header("References")]
    public Sprite sprite;
    public Sprite icon;
    new public string name;
    public CharacterStats Stats;
    public CharacterStateHandler State;
    public CharacterBattle Battle;
    public InteractibleHandler InteractibleHandler;
    public CharacterStateHandler CharacterStateHandler;
    public CharacterExplorationStateHandler CharacterExplorationStateHandler;
    public KRB_CharacterController CharacterController;
    public CharacterConditionHandler CharacterConditions;

    [Header("Debug")]
    public Color Color;

    private void OnEnable()
    {
        State.TransitionedFromTo -= Stats.OnStateTransition;
        State.TransitionedFromTo += Stats.OnStateTransition;

        Stats.CharacterDowned -= OnCharacterDowned;
        Stats.CharacterDowned += OnCharacterDowned;
    }

    private void OnDisable()
    {
        State.TransitionedFromTo -= Stats.OnStateTransition;

        Stats.CharacterDowned -= OnCharacterDowned;
    }

    private void OnCharacterDowned()
    {
        CharacterDowned?.Invoke(this);
    }
}
