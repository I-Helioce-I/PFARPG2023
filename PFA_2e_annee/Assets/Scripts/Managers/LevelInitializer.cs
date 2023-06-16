using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInitializer : MonoBehaviour
{
    private void Start()
    {
        UIManager.instance.Transitioner.TransitionFade(1f, 0f, 5f, () =>
        {
            DialogueManager.instance.StartLevelDialogue();
        });
    }
}
