using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInitializer : MonoBehaviour
{
    [SerializeField] private bool InitializeLevelOnStart = false;

    private void Start()
    {
        if (!InitializeLevelOnStart) return;

        UIManager.instance.Transitioner.TransitionIMG.fillAmount = 1f;

        UIManager.instance.Transitioner.WaitAndThen(2f, () =>
        {
            Player.instance.ChangeActionMap("UI");
            UIManager.instance.Transitioner.TransitionFade(1f, 0f, 5f, () =>
            {
                DialogueManager.instance.StartLevelDialogue();
            });
        });
    }
}
