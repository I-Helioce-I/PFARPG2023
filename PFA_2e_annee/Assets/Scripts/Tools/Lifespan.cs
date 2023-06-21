using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lifespan : MonoBehaviour
{
    public float LifespanTime = 3f;
    private float timer = 0f;

    void Update()
    {
        if (timer < LifespanTime)
        {
            timer += Time.deltaTime;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
