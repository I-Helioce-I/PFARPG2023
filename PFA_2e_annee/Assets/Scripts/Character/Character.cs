using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("References")]
    public CharacterStats Stats;
    public CharacterStateHandler State;
    public CharacterBattle Battle;
    public InteractibleHandler InteractibleHandler;
    public CharacterStateHandler CharacterStateHandler;
    public KRB_CharacterController CharacterController;

    [Header("Debug")]
    public Color Color;

    private void OnEnable()
    {
        State.TransitionedFromTo -= Stats.OnStateTransition;
        State.TransitionedFromTo += Stats.OnStateTransition;
    }

    private void OnDisable()
    {
        State.TransitionedFromTo -= Stats.OnStateTransition;
    }
}
