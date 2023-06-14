using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    private void Start()
    {
        Destroy(this.gameObject, 1f);
    }
}
