using System;
using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController;

public class BOXDEBUG : MonoBehaviour
{
    public KinematicCharacterMotor Motor;

    private void Update(){
        Debug.Log(Motor.GroundingStatus.IsStableOnGround);
    }
}
