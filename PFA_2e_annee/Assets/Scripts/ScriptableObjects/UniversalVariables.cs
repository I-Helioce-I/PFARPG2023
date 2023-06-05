using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "JRPG/Game/UniversalVariables")]
public class UniversalVariables : ScriptableObject
{
    [Header("Stat variables")]
    [SerializeField] private float _vitalityToHealthFactor;
    public float VitalityToHealthFactor
    {
        get
        {
            return _vitalityToHealthFactor;
        }
    }
    [SerializeField] private float _arcaneToMagicFactor;
    public float ArcaneToMagicFactor
    {
        get
        {
            return _arcaneToMagicFactor;
        }
    }

    [Header("Combat variables")]
    [SerializeField] private int _initiativeRollSize;
    public int InitiativeRollSize
    {
        get
        {
            return _initiativeRollSize;
        }
    }
}
