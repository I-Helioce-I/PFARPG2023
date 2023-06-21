using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    //Place on root object holding the mesh, NOT on the actual gameobject designed to move in worldspace.

    public Vector3 MinRotSpeeds;
    public Vector3 MaxRotSpeeds;

    private float _xRotSpeed;
    private float _yRotSpeed;
    private float _zRotSpeed;

    private void Start()
    {
        RecalculateRotSpeed(MinRotSpeeds, MaxRotSpeeds);
    }

    private void Update()
    {
        transform.Rotate(_xRotSpeed * Time.deltaTime, _yRotSpeed * Time.deltaTime, _zRotSpeed * Time.deltaTime);
    }

    public void RecalculateRotSpeed(Vector3 MinRotSpeeds, Vector3 MaxRotSpeeds)
    {
        _xRotSpeed = Random.Range(MinRotSpeeds.x, MaxRotSpeeds.x);
        _yRotSpeed = Random.Range(MinRotSpeeds.y, MaxRotSpeeds.y);
        _zRotSpeed = Random.Range(MinRotSpeeds.z, MaxRotSpeeds.z);
    }
}
