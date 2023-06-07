using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class DialogueLine
{
    public CharacterPortraitEmotion Emotion;
    public bool MainCharacterSpeaking = true;
    [TextArea(5, 10)]
    public string Text = "";
}
