using System.Collections;
using System.Collections.Generic;
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
        CameraManager.instance.ExplorationCameras[0].Follow = Player.instance.Character.transform;
    }

    public void SwitchStateForward()
    {
        if (!CharacterController.Motor.GroundingStatus.IsStableOnGround || CharacterController.CurrentCharacterState != CharacterState.Default) return;

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
            //Play transitionIn animation. At the end of the animation
            TransitionToState(PossibleStates[0]);
        }
        else
        {
            TransitionToState(PossibleStates[currentIndex]);
        }
    }

    public void SwitchStateBackward()
    {
        if (!CharacterController.Motor.GroundingStatus.IsStableOnGround || CharacterController.CurrentCharacterState != CharacterState.Default) return;

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
            TransitionToState(PossibleStates[PossibleStates.Count - 1]);
        }
        else
        {
            TransitionToState(PossibleStates[currentIndex]);
        }
    }
}
