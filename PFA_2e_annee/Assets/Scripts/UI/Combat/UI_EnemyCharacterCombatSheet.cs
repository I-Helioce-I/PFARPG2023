using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_EnemyCharacterCombatSheet : MonoBehaviour
{
    [Header("Portrait")]
    public Image Portrait;

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

        _representedStats.Health.ValueChanged -= TargetHealthValue;
        _representedStats.Temperature.ValueChanged -= TargetTemperatureValue;

        _representedStats.Health.ValueChanged += TargetHealthValue;
        _representedStats.Temperature.ValueChanged += TargetTemperatureValue;

        CurrentHealthValueShown = _representedStats.Health.CurrentValue;
        CalculateHealthBarValue();
        AdaptHealthColor();
        CalculateTemperatureBarValue();
        AdaptTemperatureColor();
    }

    private void OnDisable()
    {
        _representedStats.Health.ValueChanged -= TargetHealthValue;
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
        if (HealthSlider.value <= 1f && HealthSlider.value > .5f)
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

    private void CalculateTemperatureBarValue()
    {
        TemperatureSlider.value = _currentTemperatureValueShown / _representedStats.Temperature.MaxValue;
    }

    private void UpdateHealthText()
    {
        HealthText.text = Mathf.RoundToInt(CurrentHealthValueShown).ToString();
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
