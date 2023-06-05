using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Header("Stats")]
    public CharacterStat Health;
    public CharacterStat Magic;
    public CharacterStat Prowess;
    public CharacterStat Arcane;
    public CharacterStat Vitality;
    public CharacterStat Speed;

    [Header("State")]
    public bool isDown = false;

    private int level = 1;


    private void OnEnable()
    {
        Vitality.MaxValueChanged -= RecalculateHealth;
        Vitality.MaxValueChanged += RecalculateHealth;

        Arcane.MaxValueChanged -= RecalculateMagic;
        Arcane.MaxValueChanged += RecalculateMagic;

        Health.CurrentValueReachedZero -= CharacterDown;
        Health.CurrentValueReachedZero += CharacterDown;
        Health.CurrentValueBroughtBackFromZero -= CharacterRevive;
        Health.CurrentValueBroughtBackFromZero += CharacterRevive;
    }

    private void OnDisable()
    {
        Vitality.MaxValueChanged -= RecalculateHealth;

        Arcane.MaxValueChanged -= RecalculateMagic;

        Health.CurrentValueReachedZero -= CharacterDown;
        Health.CurrentValueBroughtBackFromZero -= CharacterRevive;
    }

    private void Start()
    {
        CalculateAllStats();       
    }

    public void CalculateAllStats()
    {
        float health = Health.MaxValue;
        float magic = Magic.MaxValue;
        float prowess = Prowess.MaxValue;
        float arcane = Arcane.MaxValue;
        float vitality = Vitality.MaxValue;
        float speed = Speed.MaxValue;
    }

    public void RecalculateHealth()
    {
        Health.RemoveAllModifiersFromSource(Vitality);

        float vitalityValue = Vitality.MaxValue * GameManager.instance.UniversalVariables.VitalityToHealthFactor;

        StatModifier healthModifier = new StatModifier(vitalityValue, StatModifierType.Flat, Vitality);
        Health.AddModifier(healthModifier);

        float health = Health.MaxValue;
    }

    public void RecalculateMagic()
    {
        Magic.RemoveAllModifiersFromSource(Arcane);

        float arcaneValue = Arcane.MaxValue * GameManager.instance.UniversalVariables.ArcaneToMagicFactor;

        StatModifier magicModifier = new StatModifier(arcaneValue, StatModifierType.Flat, Arcane);
        Magic.AddModifier(magicModifier);

        float magic = Magic.MaxValue;
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    level++;

        //    Debug.Log("Leveled up to level " + level + "!");
        //    Health.BaseValue += 10;
        //    Magic.BaseValue += 10;
        //    Prowess.BaseValue += 3;
        //    Vitality.BaseValue += 2;
        //    Arcane.BaseValue += 1;
        //    Speed.BaseValue += 2;

        //    CalculateAllStats();
        //}

        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    if (!isDown)
        //    {
        //        Debug.Log("Damage health for 10 damage!");
        //        Health.Damage(10f);
        //    }
        //    else
        //    {
        //        Debug.Log("Stop, he's already dead!");
        //    }
            
        //}

        //if (Input.GetKeyDown(KeyCode.Z))
        //{
        //    Debug.Log("Character has been fully healed!");
        //    Health.HealToMaxValue();
        //}
    }

    public void CharacterDown()
    {
        if (!isDown)
        {
            isDown = true;
            Debug.Log("Character has been downed!");
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
}
