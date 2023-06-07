using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    public static DialogueUI instance;

    [SerializeField] private GameObject DialogueGO;
    [SerializeField] private Image MainCharacterPortait;
    [SerializeField] private Image SecondaryCharacterPortrait;
    [SerializeField] private TextMeshProUGUI DialogueText;

    [SerializeField] private Color BrightPortrait;
    [SerializeField] private Color DarkPortrait;
    [SerializeField] private float ChangeLuminosityDuration = .3f;

    private CharacterPortraitHandler _mainCharacterPortraitHandler;
    private CharacterPortraitHandler _secondaryCharacterPortraitHandler;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        DialogueGO.SetActive(false);
    }

    public void InitializeDialogueUI(CharacterPortraitHandler mainCharacter, CharacterPortraitHandler secondaryCharacter)
    {
        //Open dialogue UI, disable controls.

        MainCharacterPortait.sprite = mainCharacter.CurrentPortrait;
        SecondaryCharacterPortrait.sprite = secondaryCharacter.CurrentPortrait;

        _mainCharacterPortraitHandler = mainCharacter;
        _secondaryCharacterPortraitHandler = secondaryCharacter;

        DialogueGO.SetActive(true);
    }

    public void GoToNextDialogueLine(DialogueLinesSet dialogue, int toIndex)
    {
        if (dialogue.DialogueLines[toIndex].MainCharacterSpeaking)
        {
            BrightenPortrait(MainCharacterPortait);
            DarkenPortrait(SecondaryCharacterPortrait);

            _mainCharacterPortraitHandler.CurrentEmotion = dialogue.DialogueLines[toIndex].Emotion;
            MainCharacterPortait.sprite = _mainCharacterPortraitHandler.CurrentPortrait;
        }
        else
        {
            BrightenPortrait(SecondaryCharacterPortrait);
            DarkenPortrait(MainCharacterPortait);

            _secondaryCharacterPortraitHandler.CurrentEmotion = dialogue.DialogueLines[toIndex].Emotion;
            SecondaryCharacterPortrait.sprite = _secondaryCharacterPortraitHandler.CurrentPortrait;
        }

        DialogueText.text = dialogue.DialogueLines[toIndex].Text;
    }

    public void EndDialogue()
    {
        _mainCharacterPortraitHandler.CurrentEmotion = CharacterPortraitEmotion.Neutral;
        _secondaryCharacterPortraitHandler.CurrentEmotion = CharacterPortraitEmotion.Neutral;

        MainCharacterPortait.sprite = null;
        SecondaryCharacterPortrait.sprite = null;
        _mainCharacterPortraitHandler = null;
        _secondaryCharacterPortraitHandler = null;

        DialogueGO.SetActive(false);

        //Close dialogue UI, re-enable controls.
    }

    private void BrightenPortrait(Image portrait)
    {
        StartCoroutine(ChangePortraitLuminosity(portrait, true));
    }

    private void DarkenPortrait(Image portrait)
    {
        StartCoroutine(ChangePortraitLuminosity(portrait, false));
    }

    private IEnumerator ChangePortraitLuminosity(Image portrait, bool brighten)
    {
        float duration = ChangeLuminosityDuration;
        float elapsedTime = 0f;
        Color currentPortraitColor = portrait.color;

        while(elapsedTime < duration)
        {
            if (brighten)
            {              
                portrait.color = Color.Lerp(currentPortraitColor, BrightPortrait, elapsedTime / duration);
            }
            else
            {
                portrait.color = Color.Lerp(currentPortraitColor, DarkPortrait, elapsedTime / duration);
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (brighten)
        {
            portrait.color = BrightPortrait;
        }
        else
        {
            portrait.color = DarkPortrait;
        }
    }
}
