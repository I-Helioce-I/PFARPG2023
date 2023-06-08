using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Displacer : MonoBehaviour
{
    [Header("Object references")]
    public Transform From;
    public Transform To;

    public AnimationCurve FromToCurve;
    public float DisplacementTime;

    public void Displace(InteractibleHandler handler)
    {
        KRB_CharacterController character = handler.GetComponent<KRB_CharacterController>();
        Transform from = From.transform;
        Transform to = To.transform;

        character.CurrentDisplacer = this;
        character.DisplacementTime = DisplacementTime;

        character.DisplacerTargetPosition = from.position;
        character.DisplacerTargetRotation = to.rotation;

        character.DisplacerDestinationPosition = to.position;
        character.DisplacerDestinationRotation = to.rotation;

        character.TransitionToState(CharacterState.Displacing);
    }

    public void DisplaceReverse(InteractibleHandler handler)
    {
        KRB_CharacterController character = handler.GetComponent<KRB_CharacterController>();
        Transform from = To.transform;
        Transform to = From.transform;

        character.CurrentDisplacer = this;
        character.DisplacementTime = DisplacementTime;

        character.DisplacerTargetPosition = from.position;
        character.DisplacerTargetRotation = to.rotation;

        character.DisplacerDestinationPosition = to.position;
        character.DisplacerDestinationRotation = to.rotation;

        character.TransitionToState(CharacterState.Displacing);
    }
}
