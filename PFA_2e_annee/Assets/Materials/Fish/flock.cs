using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flock : MonoBehaviour
{
    public float speed = 0.1f;
    float rotationSpeed = 4.0f;
    Vector3 averageHeading;
    Vector3 averagePosition;
    float neighbourDistance = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        speed = Random.Range(0.5f, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Random.Range(0, 5) < 1)
            ApplyRules();
        transform.Translate(0, 0, Time.deltaTime * speed);
    }
    void ApplyRules()
    {
        GameObject[] gos;
        gos = globalFlock.allFish;

        Vector3 vcentre = Vector3.zero;
        Vector3 vavoid = Vector3.zero;

        float gSpeed = 0.1f;

        Vector3 goalPos = globalFlock.goalPos;

        float dist;

        int groupSize = 0;
        //foreach(go != this.GameObject)

    }
}
