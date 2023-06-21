using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ExperienceScreen : MonoBehaviour
{
    [SerializeField] private List<Slider> ExperienceSliders = new List<Slider>();
    [SerializeField] private int MaxEXP = 1000;
    [SerializeField] private int EXPGain = 250;
    private int _currentExpShown = 0;

    [SerializeField] private List<TextMeshProUGUI> ExperienceNumberTextSmall = new List<TextMeshProUGUI>();
    [SerializeField] private List<TextMeshProUGUI> ExperienceNumberTextLarge = new List<TextMeshProUGUI>();

    [SerializeField] private AudioClip ExpGainDing;
    [SerializeField] private float TimeBetweenDings = .1f;

    [SerializeField] private float ExperienceGainOverTime = 1.5f;

    private void Start()
    {
        UpdateTexts();
        UpdateSliders();
    }

    public void GainExperience()
    {
        StartCoroutine(GainExperienceCoroutine());
    }

    private void UpdateTexts()
    {
        foreach(TextMeshProUGUI text in ExperienceNumberTextSmall)
        {
            text.text = "+ " + _currentExpShown.ToString() + "<size=18> Xp.";
        }

        foreach (TextMeshProUGUI text in ExperienceNumberTextLarge)
        {
            text.text = _currentExpShown.ToString() + " <size=20> Xp.";
        }
    }
    private void UpdateSliders()
    {
        foreach(Slider slider in ExperienceSliders)
        {
            Debug.Log(_currentExpShown);
            Debug.Log(MaxEXP);
            Debug.Log(_currentExpShown / MaxEXP);
            slider.value = (float)_currentExpShown / (float)MaxEXP;
        }
    }

    private IEnumerator GainExperienceCoroutine()
    {
        float timer = 0f;
        float dingTimer = 0f;
        while (timer < ExperienceGainOverTime)
        {
            timer += Time.deltaTime;
            int newExpShown = Mathf.RoundToInt(Mathf.Lerp(0, EXPGain, timer / ExperienceGainOverTime));
            _currentExpShown = newExpShown;
            UpdateTexts();
            UpdateSliders();

            if (dingTimer < TimeBetweenDings)
            {
                dingTimer += Time.deltaTime;
            }
            else
            {
                dingTimer = 0f;
                //Ding
            }
            yield return null;
        }

        _currentExpShown = EXPGain;
        UpdateTexts();
        UpdateSliders();
        //Ding
    }
}
