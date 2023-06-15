using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetSelectOnSpawn : MonoBehaviour
{
    private void Start()
    {
        this.gameObject.GetComponent<Button>().Select();
    }
}
