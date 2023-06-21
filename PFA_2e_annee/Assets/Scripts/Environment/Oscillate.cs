using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillate : MonoBehaviour
{
    //Place onto a root gameobject holding the mesh of the object, -NOT- the actual object if it is designed to move in worldspace.

    public AnimationCurve OscillationCurve;
    public float Intensity = 1f;
    public float Frequency = 1f;

    private Vector3 _initialLocalPosition;
    private float _timer = 0f;

    private void Start()
    {
        _initialLocalPosition = transform.localPosition;
    }

    private void Update()
    {
        if (_timer > 1)
        {
            _timer = 0;
        }
        transform.localPosition = _initialLocalPosition + new Vector3(0, OscillationCurve.Evaluate(_timer) * Intensity, 0);

        _timer += Time.deltaTime * Frequency;
    }
}
