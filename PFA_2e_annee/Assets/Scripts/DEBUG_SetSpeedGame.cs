using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DEBUG_SetSpeedGame : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            Time.timeScale = Time.timeScale * 2;
            if (Time.timeScale >= 6)
            {
                Time.timeScale = 6;
            }
        }
        else if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            Time.timeScale = Time.timeScale / 2;
            if (Time.timeScale <= 1)
            {
                Time.timeScale = 1;
            }
        }
    }
}
