using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class CharacterExplorationStateHandler : MonoBehaviour
{
    public delegate void CharacterExplorationTypeStateEvent(CharacterTypeState fromState, CharacterTypeState toState);
    public event CharacterExplorationTypeStateEvent TransitionedFromTo = null;

    public KRB_CharacterController CharacterController;
    [SerializeField] private CharacterTypeState _representedState;

    public List<CharacterTypeState> PossibleStates = new List<CharacterTypeState>();

    public Character SolidCharacter;
    public Character LiquidCharacter;
    public Character GasCharacter;

    public ParticleSystem VFX_ElementalSwirl;
    public ParticleSystem VFX_TransitionFlash;
    private IEnumerator _currentFlashCoroutine;
    private Renderer[] _renderers;

    private void Awake()
    {
        _renderers = GetComponentsInChildren<Renderer>(true);
    }

    public CharacterTypeState RepresentedState
    {
        get
        {
            return _representedState;
        }
        set
        {
            if (_representedState == value) return;
            TransitionToState(value);
        }
    }

    private void TransitionToState(CharacterTypeState toState)
    {
        CharacterTypeState fromState = _representedState;

        OnCharacterTransition(fromState, toState);

        TransitionedFromTo?.Invoke(fromState, toState);
    }

    private void OnCharacterTransition(CharacterTypeState fromState, CharacterTypeState toState)
    {
        Quaternion rotation = Player.instance.CharacterController.Motor.TransientRotation;
        Vector3 position = Player.instance.CharacterController.Motor.TransientPosition;
        Player.instance.CharacterController.Motor.SetPosition(PrisonManager.instance.PrisonSpot.position);
        //Player.instance.CharacterController.Motor.SetMovementCollisionsSolvingActivation(false);
        //Player.instance.CharacterController.Motor.SetGroundSolvingActivation(false);

        PlayerCharacterInputs characterInputs = new PlayerCharacterInputs();

        // Build the CharacterInputs struct
        characterInputs.MoveAxisForward = 0f;
        characterInputs.MoveAxisRight = 0f;
        characterInputs.CameraRotation = Camera.main.transform.rotation;
        characterInputs.JumpDown = false;

        // Apply inputs to character
        Player.instance.CharacterController.SetInputs(ref characterInputs);

        Player.instance.Character = null;
        Player.instance.CharacterController = null;

        switch (fromState)
        {
            case CharacterTypeState.None:
                break;
            case CharacterTypeState.Solid:
                break;
            case CharacterTypeState.Liquid:
                break;
            case CharacterTypeState.Gas:
                break;
            case CharacterTypeState.TriplePoint:
                break;
            default:
                break;
        }

        switch (toState)
        {
            case CharacterTypeState.None:
                break;
            case CharacterTypeState.Solid:
                Player.instance.Character = SolidCharacter;
                Player.instance.CharacterController = SolidCharacter.CharacterController;
                break;
            case CharacterTypeState.Liquid:
                Player.instance.Character = LiquidCharacter;
                Player.instance.CharacterController = LiquidCharacter.CharacterController;
                break;
            case CharacterTypeState.Gas:
                Player.instance.Character = GasCharacter;
                Player.instance.CharacterController = GasCharacter.CharacterController;
                break;
            case CharacterTypeState.TriplePoint:
                break;
            default:
                break;
        }

        //Player.instance.CharacterController.Motor.SetMovementCollisionsSolvingActivation(true);
        //Player.instance.CharacterController.Motor.SetGroundSolvingActivation(true);
        Player.instance.CharacterController.Motor.SetPositionAndRotation(position, rotation);
        Player.instance.Character.ExplorationAnimatorHandler.PlayAnimThenAction("TransitionOut", () =>
        {
            Player.instance.CanMove = true;
        });
        CameraManager.instance.ExplorationCameras[0].Follow = Player.instance.Character.transform;
        ParticleSystem particles = Instantiate<ParticleSystem>(Player.instance.Character.CharacterExplorationStateHandler.VFX_ElementalSwirl, Player.instance.Character.transform.position, Player.instance.Character.transform.rotation);
        var main = particles.main;
        particles.Play();
        main.stopAction = ParticleSystemStopAction.Destroy;
        Player.instance.Character.CharacterExplorationStateHandler.VFX_TransitionFlash.Play();
        Player.instance.Character.CharacterExplorationStateHandler.Flash(100f, 1f, false);
    }

    public void SwitchStateForward()
    {
        if (CharacterController.CurrentCharacterState != CharacterState.Default) return;

        Player.instance.CanMove = false;

        PlayerCharacterInputs characterInputs = new PlayerCharacterInputs();

        characterInputs.MoveAxisForward = 0f;
        characterInputs.MoveAxisRight = 0f;

        CharacterController.SetInputs(ref characterInputs);

        Player.instance.CharacterController.SetInputs(ref characterInputs);

        int currentIndex = -1;
        for (int i = 0; i < PossibleStates.Count; i++)
        {
            if (_representedState == PossibleStates[i])
            {
                currentIndex = i;
            }
        }

        currentIndex++;

        if (currentIndex > PossibleStates.Count - 1)
        {
            //Play transitionIn animation. At the end of the animation, play transitionOut animation.
            Flash(1f, 100f);
            Player.instance.Character.ExplorationAnimatorHandler.PlayAnimThenAction("TransitionIn", () =>
            {
                TransitionToState(PossibleStates[0]);
            });
            
        }
        else
        {
            Flash(1f, 100f);
            Player.instance.Character.ExplorationAnimatorHandler.PlayAnimThenAction("TransitionIn", () =>
            {
                TransitionToState(PossibleStates[currentIndex]);
            });       
        }
    }

    public void SwitchStateBackward()
    {
        if (CharacterController.CurrentCharacterState != CharacterState.Default) return;

        Player.instance.CanMove = false;

        PlayerCharacterInputs characterInputs = new PlayerCharacterInputs();

        characterInputs.MoveAxisForward = 0f;
        characterInputs.MoveAxisRight = 0f;

        CharacterController.SetInputs(ref characterInputs);

        Player.instance.CharacterController.SetInputs(ref characterInputs);

        int currentIndex = -1;
        for (int i = 0; i < PossibleStates.Count; i++)
        {
            if (_representedState == PossibleStates[i])
            {
                currentIndex = i;
            }
        }

        currentIndex--;

        if (currentIndex < 0)
        {
            Flash(1f, 100f);
            Player.instance.Character.ExplorationAnimatorHandler.PlayAnimThenAction("TransitionIn", () =>
            {
                TransitionToState(PossibleStates[PossibleStates.Count - 1]);
            });
        }
        else
        {
            Flash(1f, 100f);
            Player.instance.Character.ExplorationAnimatorHandler.PlayAnimThenAction("TransitionIn", () =>
            {
                TransitionToState(PossibleStates[currentIndex]);
            });
        }
    }

    private void Flash(float fromIntensity, float toIntensity, bool revertToFrom = true)
    {
        if (_currentFlashCoroutine != null) StopCoroutine(_currentFlashCoroutine);
        _currentFlashCoroutine = MeshFlash(fromIntensity, toIntensity, .4f, revertToFrom);
        StartCoroutine(MeshFlash(fromIntensity, toIntensity, .4f, revertToFrom));
    }

    private IEnumerator MeshFlash(float fromIntensity, float toIntensity, float overTime, bool revertToFrom = true)
    {
        float timer = 0f;

        foreach (Renderer renderer in _renderers)
        {
            renderer.material.SetFloat("_Albedo_Intensity", fromIntensity);
        }

        while (timer < overTime)
        {
            timer += Time.deltaTime;
            float lerpdIntensity = Mathf.Lerp(fromIntensity, toIntensity, timer / overTime);
            foreach (Renderer renderer in _renderers)
            {
                renderer.material.SetFloat("_Albedo_Intensity", lerpdIntensity);
            }
            yield return null;
        }

        //Revert to original colors
        if (revertToFrom)
        {
            foreach (Renderer renderer in _renderers)
            {
                renderer.material.SetFloat("_Albedo_Intensity", fromIntensity);
            }
        }
        else
        {
            foreach (Renderer renderer in _renderers)
            {
                renderer.material.SetFloat("_Albedo_Intensity", toIntensity);
            }
        }

    }
}
