using Cinemachine;
using KinematicCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInitializer : MonoBehaviour
{
    [SerializeField] private bool InitializeLevelOnStart = false;

    [SerializeField] private AudioClip LevelMusic;
    [SerializeField] private AudioClip BattleMusic;
    [SerializeField] private AudioClip BattleStart;

    private void Start()
    {
        if (!InitializeLevelOnStart) return;

        UIManager.instance.Transitioner.TransitionIMG.fillAmount = 1f;

        //KinematicCharacterMotorState motorState = new KinematicCharacterMotorState();
        //CharacterTransientGroundingReport groundingReport = new CharacterTransientGroundingReport();
        //groundingReport.FoundAnyGround = true;
        //groundingReport.IsStableOnGround = true;
        //motorState.GroundingStatus = groundingReport;

        //Player.instance.GasCharacter.gameObject.SetActive(true);
        //Player.instance.GasCharacter.CharacterController.Motor.ApplyState(motorState);
        //foreach (Character character in Player.instance.AllControlledCharacters)
        //{
        //    character.gameObject.SetActive(true);
        //    character.CharacterController.Motor.ApplyState(motorState);
        //}
        Player.instance.Character.Battle.CharacterAnimatorHandler.PlayAnim("Birth");

        SoundManager.instance.StopMusic();
        SoundManager.instance.PlayMusic(LevelMusic);
        SoundManager.instance.Fade(SoundManager.instance.MusicSource, 1f, true);
        SoundManager.instance.Fade(SoundManager.instance.SFXSource, 1f, true);

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

    public void StartBattleMusic()
    {
        SoundManager.instance.PlaySFX(BattleStart);
        SoundManager.instance.Fade(SoundManager.instance.MusicSource, 1f, false, () =>
        {
            SoundManager.instance.StopMusic();
            SoundManager.instance.PlayMusic(BattleMusic);
        });
        SoundManager.instance.Fade(SoundManager.instance.MusicSource, 1f, true);
    }

    public void EndBattleMusic()
    {
        SoundManager.instance.Fade(SoundManager.instance.MusicSource, 1f, false, () =>
        {
            SoundManager.instance.StopMusic();
            SoundManager.instance.PlayMusic(LevelMusic);
        });
        SoundManager.instance.Fade(SoundManager.instance.MusicSource, 1f, true);
    }
}
