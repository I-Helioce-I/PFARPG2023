using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    public CharacterDialogue DialogueOnStartLevel;
    public CharacterPortraitHandler Isen;
    public CharacterPortraitHandler Leaghan;

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

    public void StartLevelDialogue()
    {
        DialogueOnStartLevel.StartDialogueBetween(Isen, Leaghan);
    }
}
