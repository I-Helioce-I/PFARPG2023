using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Header("Stats")]
    public CharacterStat Health;
    public CharacterStat Aether;
    public CharacterStat Speed;
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
        float health = Health.CurrentValue;
        float aether = Aether.CurrentValue;
        float speed = Speed.CurrentValue;
        float temp = Temperature.CurrentValue;
    }

    public void OnStateTransition(CharacterTypeState fromState, CharacterTypeState toState)
    {

    }
}
