using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrisonManager : MonoBehaviour
{
    public static PrisonManager instance;

    public Transform PrisonSpot;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
