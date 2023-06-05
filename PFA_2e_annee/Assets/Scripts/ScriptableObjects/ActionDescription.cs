using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public Color IconColor;
    public ActionType Type;
    public Vector2 MinMaxDamage;
    public DamageType TypeOfDamage;
    public int NumberOfTimes;
    public float MagicCost;
    public TargetRow ViableSourceRow;
    public TargetRow ViableTargetRow;
    public bool TargetsAllies = false;
    public bool TargetsAllViableTargets = false;

    [Header("Animation")]
    public string AnimationName;
    public bool doesSlide = false;
}
