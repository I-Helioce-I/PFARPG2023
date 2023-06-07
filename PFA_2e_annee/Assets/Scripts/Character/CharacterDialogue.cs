using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDialogue : MonoBehaviour
{
    public DialogueLinesSet DialogueLines;
    [SerializeField][ReadOnlyInspector] private int _numberOfTimesDialogueInitiated = 0;

    public CharacterPortraitHandler DefaultSecondaryCharacter;


    private bool _hasStarted = false;

    private CharacterPortraitHandler _currentMainCharacter;
    private CharacterPortraitHandler _currentSecondaryCharacter;
    private int _currentLineIndex = -1;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _hasStarted)
        {
            _currentLineIndex += 1;
            if (_currentLineIndex > DialogueLines.DialogueLines.Count-1)
            {
                //Dialogue is finished.
                DialogueUI.instance.EndDialogue();
                _currentLineIndex = -1;
                _hasStarted = false;
            }
            else
            {
                DialogueUI.instance.GoToNextDialogueLine(DialogueLines, _currentLineIndex);
            }
        }
    }

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

        _currentMainCharacter = mainCharacter;
        _currentSecondaryCharacter = secondaryCharacter;

        _hasStarted = true;

        DialogueUI.instance.InitializeDialogueUI(mainCharacter, secondaryCharacter);
        _currentLineIndex = 0;
        DialogueUI.instance.GoToNextDialogueLine(DialogueLines, _currentLineIndex);
    }
}
