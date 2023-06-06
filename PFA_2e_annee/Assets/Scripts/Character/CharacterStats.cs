using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Header("Stats")]
    public CharacterStat Health;
    public CharacterStat Aether;
    public CharacterStat Temperature;

    [Header("State")]
    public bool isDown = false;

    private int level = 1;


    private void OnEnable()
    {

    }

    private void OnDisable()
    {

    }

    private void Start()
    {
        CalculateAllStats();       
    }

    public void CalculateAllStats()
    {

    }

    public void OnStateTransition(CharacterTypeState fromState, CharacterTypeState toState)
    {

    }
}
