using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFOVTransitionBox : MonoBehaviour
{
    public float ToFov = 60f;
    public float OverTime = 3f;

    private void OnTriggerEnter(Collider other)
    {
        Character character = other.GetComponent<Character>();
        if (character)
        {
            CinemachineVirtualCamera vcam = CameraManager.instance.CurrentCamera.GetComponent<CinemachineVirtualCamera>();

            if (vcam.m_Lens.FieldOfView != ToFov)
            {
                CameraManager.instance.SmoothCurrentCameraFov(vcam.m_Lens.FieldOfView, ToFov, OverTime, () =>
                {
                    vcam.m_Lens.FieldOfView = ToFov;
                });
            }
        }
    }
}
