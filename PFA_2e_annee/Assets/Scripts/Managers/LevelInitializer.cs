using Cinemachine;
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
        Player.instance.Character.Battle.CharacterAnimatorHandler.PlayAnim("Birth");

        UIManager.instance.Transitioner.WaitAndThen(2f, () =>
        {
            Player.instance.ChangeActionMap("UI");
            CameraManager.instance.SmoothCurrentCameraFov(10f, 60f, 8f, () =>
            {
                CinemachineVirtualCamera vcam = CameraManager.instance.CurrentCamera.GetComponent<CinemachineVirtualCamera>();
                vcam.m_Lens.FieldOfView = 60f;
            });
            UIManager.instance.Transitioner.TransitionFade(1f, 0f, 11f, () =>
            {
                DialogueManager.instance.StartLevelDialogue();
            });
        });
    }
}
