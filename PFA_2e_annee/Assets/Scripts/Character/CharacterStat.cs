using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections.ObjectModel;

[Serializable]
public class CharacterStat
{
    public delegate void ValueEventWithArgs(float to);
    public delegate void ValueEvent();

    public event ValueEvent ValueChanged = null;
    public event ValueEvent MaxValueChanged = null;
    public event ValueEvent CurrentValueReachedZero = null;
    public event ValueEvent CurrentValueReachedFull = null;
    public event ValueEvent CurrentValueBroughtBackFromZero = null;

    public event ValueEventWithArgs ValueChangedTo = null;
    public event ValueEventWithArgs MaxValueChangedTo = null;

    public float BaseValue = 0f;
    [SerializeField][ReadOnlyInspector] private float _maxValue = float.MinValue;
    [SerializeField][ReadOnlyInspector] private float _currentValue = float.MinValue;
    public float MaxValue
    {
        get
        {
           if (_isDirty || BaseValue != _lastBaseValue)
            {
                _lastBaseValue = BaseValue;
                _value =  CalculateValues();

                MaxValueChanged?.Invoke();
                MaxValueChangedTo?.Invoke(_value);
            }
            return _value;
        }
    }
    public float CurrentValue
    {
        get
        {
            if (_isDirty)
            {
                _currentValue = MaxValue - _damage;
                ValueChanged?.Invoke();
                ValueChangedTo?.Invoke(_currentValue);
            }
            return _currentValue;
        }
    }

    protected bool _isDirty = true;
    protected float _value;
    protected float _lastBaseValue = float.MinValue;
    protected float _damage = 0f;

    protected readonly List<StatModifier> _statModifiers;
    public readonly ReadOnlyCollection<StatModifier> StatModifiers;

    public CharacterStat ()
    {
        _statModifiers = new List<StatModifier>();
        StatModifiers = _statModifiers.AsReadOnly();
    }

    public CharacterStat (float baseValue) : this()
    {
        BaseValue = baseValue;
    }

    public void AddModifier(StatModifier mod)
    {
        _isDirty = true;
        _statModifiers.Add(mod);
        _statModifiers.Sort(CompareModifierOrder);
    }

    protected int CompareModifierOrder(StatModifier a, StatModifier b)
    {
        if (a.Order > b.Order) return -1;
        else if (a.Order < b.Order) return 1;
        else return 0;
    }

    public bool RemoveModifier(StatModifier mod)
    {
        if (_statModifiers.Remove(mod))
        {
            _isDirty = true;
            return true;
        }
        return false;
    }

    public bool RemoveAllModifiersFromSource(object source)
    {
        bool didRemove = false;

        for (int i = _statModifiers.Count-1; i >= 0; i--)
        {
            if (_statModifiers[i].Source == source)
            {
                _isDirty = true;
                didRemove = true;
                _statModifiers.RemoveAt(i);
            }
        }

        return didRemove;
    }

    protected float CalculateValues()
    {
        float finalValue = BaseValue;
        float sumPercentAdd = 0;

        for (int i = 0; i < _statModifiers.Count; i++)
        {
            StatModifier mod = _statModifiers[i];

            if(mod.Type == StatModifierType.Flat)
            {
                finalValue += _statModifiers[i].Value;
            }
            else if(mod.Type == StatModifierType.PercentageAdditive)
            {
                sumPercentAdd += _statModifiers[i].Value;

                if (i+1 >= _statModifiers.Count || _statModifiers[i + 1].Type != StatModifierType.PercentageAdditive)
                {
                    finalValue *= 1 + sumPercentAdd;
                    sumPercentAdd = 0;
                }
            }
            else if (mod.Type == StatModifierType.PercentageMultiply)
            {
                finalValue *= 1 + mod.Value;
            }

        }
        finalValue = (float)Math.Round(finalValue, 4);
        _maxValue = finalValue;
        _currentValue = _maxValue - _damage;
        _isDirty = false;

        MaxValueChanged?.Invoke();
        ValueChanged?.Invoke();

        MaxValueChangedTo?.Invoke(_maxValue);
        ValueChangedTo?.Invoke(_currentValue);

        return finalValue;
    }

    public void ForceValue(float value)
    {
        _currentValue = value;
    }

    public void Damage(float damage)
    {
        _isDirty = true;
        float oldDamage = _damage;
        _damage += damage;
        if (oldDamage < _maxValue && _damage >= _maxValue)
        {
            CurrentValueReachedZero?.Invoke();
        }
        if (_damage >= _maxValue)
        {
            _damage = _maxValue;
        }

        CalculateValues();
    }

    public void Heal(float heal)
    {
        _isDirty = true;
        float oldDamage = _damage;
        _damage -= heal;
        if (oldDamage >= _currentValue)
        {
            CurrentValueBroughtBackFromZero?.Invoke();
        }
        if (oldDamage > 0 && heal >= _damage)
        {
            CurrentValueReachedFull?.Invoke();
        }
        if (_damage < 0)
        {
            _damage = 0;
        }

        CalculateValues();
    }

    public void HealToMaxValue()
    {
        Heal(_damage);
    }
}
