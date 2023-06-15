using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public delegate void CharacterStatsEvent();
    public event CharacterStatsEvent CharacterDowned = null;

    public CharacterStateHandler CharacterStateHandler;

    [Header("Stats")]
    public CharacterStat Strength;
    public CharacterStat Agility;
    public CharacterStat Intelligence;
    public CharacterStat Constitution;
    public CharacterStat Vitality;
    public CharacterStat Luck;

    [Header("Substats")]
    public CharacterStat Health;
    public CharacterStat Ether;
    public CharacterStat Temperature;
    public CharacterStat Speed;
    public CharacterStat PhysicalDamage;
    public CharacterStat MagicalDamage;
    public CharacterStat PhysicalResistance;
    public CharacterStat MagicalResistance;

    [Header("State")]
    public bool isDown = false;

    [Header("Leveling")]
    public int Level = 1;
    public int Experience = 0;


    private void OnEnable()
    {
        if (CharacterStateHandler)
        {
            CharacterStateHandler.TransitionedFromTo -= OnStateTransition;
            CharacterStateHandler.TransitionedFromTo += OnStateTransition;
        }

        Health.CurrentValueReachedZero -= CharacterDown;
        Health.CurrentValueReachedZero += CharacterDown;
        Health.CurrentValueBroughtBackFromZero -= CharacterRevive;
        Health.CurrentValueBroughtBackFromZero += CharacterRevive;

        Strength.ValueChanged -= RecalculateSTRPhysDMG;
        Agility.ValueChanged -= RecalculateAGIPhysDMG;
        Intelligence.ValueChanged -= RecalculateEther;
        Constitution.ValueChanged -= RecalculateHealth;
        Vitality.ValueChanged -= RecalculatePhysRES;
    }

    private void OnDisable()
    {
        if (CharacterStateHandler)
        {
            CharacterStateHandler.TransitionedFromTo -= OnStateTransition;
        }

        Health.CurrentValueReachedZero -= CharacterDown;
        Health.CurrentValueBroughtBackFromZero -= CharacterRevive;
    }

    private void Awake()
    {
        CalculateAllStats();       
    }

    public void CalculateAllStats()
    {
        float strength = Strength.MaxValue;
        float agility = Agility.MaxValue;
        float intelligence = Intelligence.MaxValue;
        float constitution = Constitution.MaxValue;
        float vitality = Vitality.MaxValue;
        float luck = Luck.MaxValue;

        CalculateAllSubStats();
    }

    private void CalculateAllSubStats()
    {
        RecalculateHealth();
        RecalculateEther();
        RecalculateSpeed();
        RecalculateTemperature();
        RecalculateSTRPhysDMG();
        RecalculateAGIPhysDMG();
        RecalculatePhysRES();
        RecalculateMagDMG();
        RecalculateMagRES();
    }

    private void RecalculateHealth()
    {
        Health.RemoveAllModifiersFromSource(Constitution);

        float factor = 1f;

        float constitutionValue = Constitution.CurrentValue * factor;
        StatModifier healthModifier = new StatModifier(constitutionValue, StatModifierType.Flat, Constitution);
        Health.AddModifier(healthModifier);

        float health = Health.MaxValue;
    }

    private void RecalculateEther()
    {
        Ether.RemoveAllModifiersFromSource(Intelligence);

        float factor = 1f;

        float intelligenceValue = Constitution.CurrentValue * factor;
        StatModifier etherModifier = new StatModifier(intelligenceValue, StatModifierType.Flat, Intelligence);
        Ether.AddModifier(etherModifier);

        float aether = Ether.MaxValue;
    }

    private void RecalculateSpeed()
    {
        float speed = Speed.MaxValue;
    }

    private void RecalculateTemperature()
    {
        float temp = Temperature.MaxValue;
    }

    private void RecalculateSTRPhysDMG()
    {
        PhysicalDamage.RemoveAllModifiersFromSource(Strength);

        float factor = 1f;

        float strengthValue = Strength.CurrentValue * factor;
        StatModifier physDMGModifier = new StatModifier(strengthValue, StatModifierType.Flat, Strength);
        PhysicalDamage.AddModifier(physDMGModifier);

        float physDMG = PhysicalDamage.MaxValue;
    }

    private void RecalculateAGIPhysDMG()
    {
        PhysicalDamage.RemoveAllModifiersFromSource(Agility);

        float factor = 1f;

        float agilityValue = Agility.CurrentValue * factor;
        StatModifier physDMGModifier = new StatModifier(agilityValue, StatModifierType.Flat, Agility);
        PhysicalDamage.AddModifier(physDMGModifier);

        float physDMG = PhysicalDamage.MaxValue;
    }

    private void RecalculatePhysRES()
    {
        PhysicalResistance.RemoveAllModifiersFromSource(Vitality);

        float factor = 1f;

        float vitalityValue = Vitality.CurrentValue * factor;
        StatModifier physRESModifier = new StatModifier(vitalityValue, StatModifierType.Flat, Vitality);
        PhysicalResistance.AddModifier(physRESModifier);

        float physRes = PhysicalResistance.MaxValue;
    }

    private void RecalculateMagDMG()
    {
        float magDMG = MagicalDamage.MaxValue;
    }

    private void RecalculateMagRES()
    {
        float magRES = MagicalResistance.MaxValue;
    }

    public void CharacterDown()
    {
        if (!isDown)
        {
            isDown = true;
            Debug.Log("Character has been downed!");

            CharacterDowned?.Invoke();
        }
    }

    public void CharacterRevive()
    {
        if (isDown)
        {
            isDown = false;
            Debug.Log("Character has been revived!");
        }
    }

    public void OnStateTransition(CharacterTypeState fromState, CharacterTypeState toState)
    {
        //Remove all modifiers from affected stats of fromState source.
        // STAT.RemoveAllModifiersFromSource(fromState);
        switch (fromState)
        {
            case CharacterTypeState.None:
                break;
            case CharacterTypeState.Solid:
                break;
            case CharacterTypeState.Liquid:
                break;
            case CharacterTypeState.Gas:
                break;
            case CharacterTypeState.TriplePoint:
                break;
            default:
                break;
        }

        //Add all modifiers from affected stats. Remember to make the modifier have, as a source, toState.
        //StatModifier stateModifier = new StatModifier(value, StatModifierType, toState);
        switch (toState)
        {
            case CharacterTypeState.None:
                break;
            case CharacterTypeState.Solid:
                break;
            case CharacterTypeState.Liquid:
                break;
            case CharacterTypeState.Gas:
                break;
            case CharacterTypeState.TriplePoint:
                break;
            default:
                break;
        }
    }
}
