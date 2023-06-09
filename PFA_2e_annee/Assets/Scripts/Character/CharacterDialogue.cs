using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterDialogue : MonoBehaviour
{
    public DialogueLinesSet DialogueLines;
    [SerializeField][ReadOnlyInspector] private int _numberOfTimesDialogueInitiated = 0;

    public CharacterPortraitHandler DefaultSecondaryCharacter;

    public UnityEvent OnDialogueStart;
    public UnityEvent OnDialogueEnd;

    public void StartDialogueOnInteract(InteractibleHandler handler)
    {
        CharacterPortraitHandler handlerPortraits = handler.GetComponent<CharacterPortraitHandler>();
        StartDialogueBetween(handlerPortraits, DefaultSecondaryCharacter);
    }

    public void StartDialogueBetween(CharacterPortraitHandler mainCharacter, CharacterPortraitHandler secondaryCharacter)
    {
        if (secondaryCharacter == null)
        {
            secondaryCharacter = DefaultSecondaryCharacter;
        }

        DialogueUI.instance.InitializeDialogueUI(mainCharacter, secondaryCharacter, DialogueLines, this);
        _numberOfTimesDialogueInitiated++;
        //_currentLineIndex = 0;
        //DialogueUI.instance.GoToNextDialogueLine(DialogueLines, _currentLineIndex);
    }
}
