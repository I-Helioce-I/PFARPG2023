using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_CombatTimelapseCharacterIcon : MonoBehaviour
{
    public enum TimelapseIconState
    {
        Idle,
        Moving,
    }

    public Image CharacterIcon;
    public bool HiddenIcon = false;
    public Character RepresentedCharacter;

    private Slider CharacterSlider;

    private TimelapseIconState _state = TimelapseIconState.Idle;
    private float _originalSliderValue;
    private float _destinationSliderValue;
    private float _movingDuration = .3f;
    private float _movingTimer = 99f;

    private void Awake()
    {
        CharacterSlider = GetComponent<Slider>();
    }

    private void Update()
    {
        switch (_state)
        {
            case TimelapseIconState.Idle:
                break;
            case TimelapseIconState.Moving:
                if (_movingTimer < _movingDuration)
                {
                    _movingTimer += Time.deltaTime;
                    float interpolation = Mathf.Lerp(_originalSliderValue, _destinationSliderValue, _movingTimer / _movingDuration);
                    CharacterSlider.value = interpolation;
                }
                else
                {
                    CharacterSlider.value = _destinationSliderValue;
                    _state = TimelapseIconState.Idle;
                }
                break;
            default:
                break;
        }
    }

    public void SetIconVisible(bool show)
    {
        CharacterIcon.enabled = show;
        HiddenIcon = !show;
    }

    public void SetDestinationValue(float value)
    {
        _originalSliderValue = CharacterSlider.value;
        _destinationSliderValue = value;
        _movingTimer = 0f;
        _state = TimelapseIconState.Moving;
    }

    public void ForceSetValue(float value)
    {
        _originalSliderValue = value;
        _destinationSliderValue = value;
        CharacterSlider.value = value;
        _state = TimelapseIconState.Idle;
    }
}
