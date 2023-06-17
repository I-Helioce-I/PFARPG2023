using System.Collections;
using System.Collections.Generic;
using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class DialogueUI : MonoBehaviour
{
    public static DialogueUI instance;

    [SerializeField] private GameObject DialogueGO;
    [SerializeField] private Image MainCharacterPortait;
    [SerializeField] private Image SecondaryCharacterPortrait;
    [SerializeField] private TextMeshProUGUI DialogueText;

    [Header("Portraits")]
    [SerializeField] private Color BrightPortrait;
    [SerializeField] private Color DarkPortrait;
    [SerializeField] private float ChangeLuminosityDuration = .3f;

    [Header("Dialogue Text")]
    public AudioClip VoiceBlip;
    public int CharactersPerBlip = 5;
    [SerializeField] private float _delayBetweenTwoCharacters = .1f;
    private Coroutine _currentWritingCoroutine;
    public Coroutine CurrentWritingCoroutine
    {
        get
        {
            return _currentWritingCoroutine;
        }
    }

    [SerializeField] private UI_ButtonPrompt NextPrompt;

    private CharacterPortraitHandler _mainCharacterPortraitHandler;
    private CharacterPortraitHandler _secondaryCharacterPortraitHandler;
    private DialogueLinesSet _currentDialogue;
    private int _currentLineIndex = -1;
    private CharacterDialogue _initiator;

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

    public void InitializeDialogueUI(CharacterPortraitHandler mainCharacter, CharacterPortraitHandler secondaryCharacter, DialogueLinesSet dialogue, CharacterDialogue initiator)
    {
        //SetSlider dialogue UI, disable controls.

        UIManager.instance.CurrentState = UIManager.UIState.Dialogue;
        _initiator = initiator;

        MainCharacterPortait.sprite = mainCharacter.CurrentPortrait;
        SecondaryCharacterPortrait.sprite = secondaryCharacter.CurrentPortrait;

        _mainCharacterPortraitHandler = mainCharacter;
        _secondaryCharacterPortraitHandler = secondaryCharacter;
        _currentDialogue = dialogue;

        DialogueGO.SetActive(true);
        CheckNextDialogueLine();

        NextPrompt.ForceDisappear();

        Player.instance.ChangeActionMap("UI");

        _initiator.OnDialogueStart?.Invoke();
    }

    public void CheckNextDialogueLine()
    {
        _currentLineIndex += 1;
        if (_currentLineIndex > _currentDialogue.DialogueLines.Count - 1)
        {
            //Dialogue is finished.
            DialogueUI.instance.EndDialogue();
            _currentLineIndex = -1;
        }
        else
        {
            DialogueUI.instance.GoToNextDialogueLine(_currentLineIndex);
        }
    }

    private void GoToNextDialogueLine(int toIndex)
    {
        if (_currentDialogue.DialogueLines[toIndex].MainCharacterSpeaking)
        {
            BrightenPortrait(MainCharacterPortait);
            DarkenPortrait(SecondaryCharacterPortrait);

            _mainCharacterPortraitHandler.CurrentEmotion = _currentDialogue.DialogueLines[toIndex].Emotion;
            MainCharacterPortait.sprite = _mainCharacterPortraitHandler.CurrentPortrait;
        }
        else
        {
            BrightenPortrait(SecondaryCharacterPortrait);
            DarkenPortrait(MainCharacterPortait);

            _secondaryCharacterPortraitHandler.CurrentEmotion = _currentDialogue.DialogueLines[toIndex].Emotion;
            SecondaryCharacterPortrait.sprite = _secondaryCharacterPortraitHandler.CurrentPortrait;
        }

        DialogueText.text = _currentDialogue.DialogueLines[toIndex].Text;
        _currentWritingCoroutine = StartCoroutine(WriteNewText());
    }

    private void EndDialogue()
    {
        _mainCharacterPortraitHandler.CurrentEmotion = CharacterPortraitEmotion.Neutral;
        _secondaryCharacterPortraitHandler.CurrentEmotion = CharacterPortraitEmotion.Neutral;

        _currentDialogue = null;
        MainCharacterPortait.sprite = null;
        SecondaryCharacterPortrait.sprite = null;
        _mainCharacterPortraitHandler = null;
        _secondaryCharacterPortraitHandler = null;

        _currentLineIndex = -1;

        DialogueGO.SetActive(false);

        Player.instance.ChangeActionMap(Player.instance.PreviousActionMap);
        UIManager.instance.CurrentState = UIManager.instance.PreviousState;

        _initiator.OnDialogueEnd?.Invoke();
        _initiator = null;

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

    private IEnumerator WriteNewText()
    {
        DialogueText.ForceMeshUpdate();
        NextPrompt.Appear(false);

        //DialogueText.maxVisibleCharacters = 0;
        int totalVisibleCharacters = DialogueText.textInfo.characterCount;
        int counter = 0;
        int blipCounter = 0;

        while (counter <= totalVisibleCharacters)
        {
            int visibleCount = counter % (totalVisibleCharacters + 1);
            DialogueText.maxVisibleCharacters = visibleCount;

            //if (visibleCount >= totalVisibleCharacters)
            //{
            //    Debug.Log("???");
            //    break;
            //}

            counter += 1;
            blipCounter += 1;
            if(blipCounter >= CharactersPerBlip)
            {
                SoundManager.instance.PlaySFX(VoiceBlip);
                blipCounter = 0;
            }
            
            yield return new WaitForSeconds(_delayBetweenTwoCharacters);
        }

        NextPrompt.Appear(true);
        _currentWritingCoroutine = null;
    }

    public void ForceWriteAllText()
    {
        StopCoroutine(_currentWritingCoroutine);
        _currentWritingCoroutine = null;

        DialogueText.ForceMeshUpdate();
        DialogueText.maxVisibleCharacters = DialogueText.textInfo.characterCount;

        NextPrompt.Appear(true);
    }
}
