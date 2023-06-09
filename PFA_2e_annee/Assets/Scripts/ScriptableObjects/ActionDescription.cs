using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum VFXAttachLocation
{
    Root,
    HitPoint,
    OverHead,
}

[CreateAssetMenu(menuName = "JRPG/Game/Combat/Action")]
public class ActionDescription : ScriptableObject
{
    public enum ActionType
    {
        Attack,
        Spell,
        Item,
    }

    public enum DamageType
    {
        Physical,
        Magical,
    }

    public enum TargetRow
    {
        Both,
        Front,
        Back,
        Self,
    }

    [Header("Parameters")]
    public string Name;
    public Sprite Icon;
    [TextArea(5, 10)]
    public string DescriptionText = "";
    public Color IconColor;
    public ActionType Type;
    public float Damage;
    public bool DamageIsHeal = false;
    public DamageType TypeOfDamage;
    public int NumberOfTimes;
    public float EtherCost;
    [Range(0.0f, 100.0f)]
    public float StunChance;
    public float TemperatureChange = 0f;

    [Header("Stat modifiers")]
    public StatModifier StrengthModifier;
    public StatModifier AgilityModifier;
    public StatModifier IntelligenceModifier;
    public StatModifier ConstitutionModifier;
    public StatModifier VitalityModifier;
    public StatModifier LuckModifier;
    public StatModifier HealthModifier;
    public StatModifier EtherModifier;
    public StatModifier SpeedModifier;
    public StatModifier PhysDMGModifier;
    public StatModifier PhysRESModifier;
    public StatModifier MagDMGModifier;
    public StatModifier MagRESModifier;

    //Will have to change this system below in BattleManager.
    [Header("Need to change to adapt to new targeting rules.")]
    public TargetRow ViableSourceRow;
    public TargetRow ViableTargetRow;
    public bool TargetsAllies = false;
    public bool TargetsAllViableTargets = false;

    [Header("Animation")]
    public string AnimationName;
    public bool doesSlide = false;
    public float EffectDelayInSeconds = 0f;

    [Header("Projectile")]
    public ActionProjectile Projectile;
    public float ProjectileShootDelay;

    [Header("VFX")]
    public GameObject OnCasterCastVFX;
    public VFXAttachLocation CasterVFXAttachPoint;
    public GameObject OnTargetCastVFX;
    public VFXAttachLocation TargetVFXAttachPoint;
}
