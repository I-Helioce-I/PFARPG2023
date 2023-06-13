using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerCharacterCombatSheet : MonoBehaviour
{
    [Header("Portrait")]
    public Image Portrait;

    [Header("Level")]
    public TextMeshProUGUI LevelText;

    [Header("Health")]
    public float HealthAdaptDuration = .3f;
    public TextMeshProUGUI HealthText;

    public Slider HealthSlider;
    public Image HealthFill;
    public Color HealthFull;
    public Color HealthMid;
    public Color HealthLow;

    private float _targetHealthValue;
    private float _currentHealthValueShown;
    private float CurrentHealthValueShown
    {
        get
        {
            return _currentHealthValueShown;
        }
        set
        {
            _currentHealthValueShown = value;
            UpdateHealthText();
        }
    }

    private IEnumerator _currentHealthCoroutine;

    [Header("Ether")]
    public float EtherAdaptDuration = .5f;
    public TextMeshProUGUI EtherText;

    public Slider EtherSlider;
    public Image EtherFill;
    public Color EtherFull;
    public Color EtherMid;
    public Color EtherLow;

    private float _targetEtherValue;
    private float _currentEtherValueShown;
    private float CurrentEtherValueShown
    {
        get
        {
            return _currentEtherValueShown;
        }
        set
        {
            _currentEtherValueShown = value;
            UpdateEtherText();
        }
    }

    private IEnumerator _currentEtherCoroutine;

    [Header("Temperature")]
    public float TemperatureAdaptDuration = 1f;

    public Slider TemperatureSlider;
    public Image TemperatureFill;
    public Color TemperatureFull;
    public Color TemperatureMid;
    public Color TemperatureLow;

    private float _targetTemperatureValue;
    private float _currentTemperatureValueShown;

    private IEnumerator _currentTemperatureCoroutine;

    private CharacterStats _representedStats;

    public void InitializeSheet(CharacterStats stats, Sprite portrait)
    {
        _representedStats = stats;
        Portrait.sprite = portrait;
        LevelText.text = stats.Level.ToString();

        _representedStats.Health.ValueChanged -= TargetHealthValue;
        _representedStats.Ether.ValueChanged -= TargetEtherValue;
        _representedStats.Temperature.ValueChanged -= TargetTemperatureValue;

        _representedStats.Health.ValueChanged += TargetHealthValue;
        _representedStats.Ether.ValueChanged += TargetEtherValue;
        _representedStats.Temperature.ValueChanged += TargetTemperatureValue;

        CurrentHealthValueShown = _representedStats.Health.CurrentValue;
        CalculateHealthBarValue();
        AdaptHealthColor();
        CurrentEtherValueShown = _representedStats.Ether.CurrentValue;
        CalculateEtherBarValue();
        AdaptEtherColor();
        CalculateTemperatureBarValue();
        AdaptTemperatureColor();
    }

    private void OnDisable()
    {
        _representedStats.Health.ValueChanged -= TargetHealthValue;
        _representedStats.Ether.ValueChanged -= TargetEtherValue;
        _representedStats.Temperature.ValueChanged -= TargetTemperatureValue;
    }

    private void TargetHealthValue()
    {
        _targetHealthValue = Mathf.RoundToInt(_representedStats.Health.CurrentValue);

        if (_currentHealthCoroutine != null)
        {
            StopCoroutine(_currentHealthCoroutine);
        }
        StartCoroutine(ChangeHealthShownValue(_targetHealthValue));
        _currentHealthCoroutine = ChangeHealthShownValue(_targetHealthValue);
    }
    private void TargetEtherValue()
    {
        _targetEtherValue = Mathf.RoundToInt(_representedStats.Ether.CurrentValue);

        if (_currentEtherCoroutine != null)
        {
            StopCoroutine(_currentEtherCoroutine);
        }
        StartCoroutine(ChangeEtherShownValue(_targetEtherValue));
        _currentEtherCoroutine = ChangeEtherShownValue(_targetEtherValue);
    }
    private void TargetTemperatureValue()
    {
        _targetTemperatureValue = _representedStats.Temperature.CurrentValue;

        if (_currentTemperatureCoroutine != null)
        {
            StopCoroutine(_currentTemperatureCoroutine);
        }
        StartCoroutine(ChangeTemperatureShownValue(_targetTemperatureValue));
        _currentTemperatureCoroutine = ChangeTemperatureShownValue(_targetTemperatureValue);
    }

    private void AdaptHealthColor()
    {
        Color newColor = Color.white;
        if(HealthSlider.value <= 1f && HealthSlider.value > .5f)
        {
            float interpolation = 1f - ((1f - HealthSlider.value) * 2f);
            newColor = Color.Lerp(HealthMid, HealthFull, interpolation);
            HealthFill.color = newColor;
        }
        else if (HealthSlider.value <= .5f && HealthSlider.value >= 0f)
        {
            float interpolation = HealthSlider.value * 2f;
            newColor = Color.Lerp(HealthLow, HealthMid, interpolation);
            HealthFill.color = newColor;
        }

    }
    private void AdaptEtherColor()
    {
        Color newColor = Color.white;
        if (EtherSlider.value <= 1f && EtherSlider.value > .5f)
        {
            float interpolation = 1f - ((1f - EtherSlider.value) * 2f);
            newColor = Color.Lerp(EtherMid, EtherFull, interpolation);
            EtherFill.color = newColor;
        }
        else if (EtherSlider.value <= .5f && EtherSlider.value >= 0f)
        {
            float interpolation = EtherSlider.value * 2f;
            newColor = Color.Lerp(EtherLow, EtherMid, interpolation);
            EtherFill.color = newColor;
        }

    }
    private void AdaptTemperatureColor()
    {
        Color newColor = Color.white;
        if (TemperatureSlider.value <= 1f && TemperatureSlider.value > .5f)
        {
            float interpolation = 1f - ((1f - TemperatureSlider.value) * 2f);
            newColor = Color.Lerp(TemperatureMid, TemperatureFull, interpolation);
            TemperatureFill.color = newColor;
        }
        else if (TemperatureSlider.value <= .5f && TemperatureSlider.value >= 0f)
        {
            float interpolation = TemperatureSlider.value * 2f;
            newColor = Color.Lerp(TemperatureLow, TemperatureMid, interpolation);
            TemperatureFill.color = newColor;
        }

    }

    private void CalculateHealthBarValue()
    {
        HealthSlider.value = _currentHealthValueShown / _representedStats.Health.MaxValue;
    }

    private void CalculateEtherBarValue()
    {
        EtherSlider.value = _currentEtherValueShown / _representedStats.Ether.MaxValue;
    }

    private void CalculateTemperatureBarValue()
    {
        TemperatureSlider.value = _currentTemperatureValueShown / _representedStats.Temperature.MaxValue;
    }

    private void UpdateHealthText()
    {
        HealthText.text = Mathf.RoundToInt(CurrentHealthValueShown).ToString();
    }
    private void UpdateEtherText()
    {
        EtherText.text = Mathf.RoundToInt(CurrentEtherValueShown).ToString();
    }

    private IEnumerator ChangeHealthShownValue(float targetValue)
    {
        float shownValue = _currentHealthValueShown;
        float timer = 0f;
        while (timer < HealthAdaptDuration)
        {
            timer += Time.deltaTime;
            float newShownValue = Mathf.Lerp(shownValue, targetValue, timer / HealthAdaptDuration);
            CurrentHealthValueShown = newShownValue;
            CalculateHealthBarValue();
            AdaptHealthColor();
            yield return null;
        }
        CurrentHealthValueShown = targetValue;
        CalculateHealthBarValue();
        AdaptHealthColor();

        _currentHealthCoroutine = null;
    }
    private IEnumerator ChangeEtherShownValue(float targetValue)
    {
        float shownValue = _currentEtherValueShown;
        float timer = 0f;
        while (timer < EtherAdaptDuration)
        {
            timer += Time.deltaTime;
            float newShownValue = Mathf.Lerp(shownValue, targetValue, timer / EtherAdaptDuration);
            CurrentEtherValueShown = newShownValue;
            CalculateEtherBarValue();
            AdaptEtherColor();
            yield return null;
        }

        CurrentEtherValueShown = targetValue;
        CalculateEtherBarValue();
        AdaptEtherColor();
 
        _currentEtherCoroutine = null;

    }

    private IEnumerator ChangeTemperatureShownValue(float targetValue)
    {
        float shownValue = _currentTemperatureValueShown;
        float timer = 0f;
        while (timer < TemperatureAdaptDuration)
        {
            timer += Time.deltaTime;
            float newShownValue = Mathf.Lerp(shownValue, targetValue, timer / TemperatureAdaptDuration);
            _currentTemperatureValueShown = newShownValue;
            CalculateTemperatureBarValue();
            AdaptTemperatureColor();
            yield return null;
        }

        _currentTemperatureValueShown = targetValue;
        CalculateTemperatureBarValue();
        AdaptTemperatureColor();

        _currentTemperatureCoroutine = null;

    }


}
