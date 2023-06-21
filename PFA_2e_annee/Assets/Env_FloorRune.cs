using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Env_FloorRune : MonoBehaviour
{
    public GameObject TargetingTrailGO;

    public void ShootTrail()
    {
        TargetingTrailGO.SetActive(true);
    }
}
