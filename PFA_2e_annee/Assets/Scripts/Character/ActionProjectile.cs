using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActionProjectile : MonoBehaviour
{
    public UnityEvent OnBirth;
    public UnityEvent OnDeath;

    [SerializeField][ReadOnlyInspector] private Vector3 _from = Vector3.zero;
    [SerializeField][ReadOnlyInspector] private Vector3 _to = Vector3.zero;

    private float _overTime = 0f;
    private bool _initialized = false;
    private float _timer = 0f;

    public void InitializeProjectile(Vector3 from, Vector3 to, float overTime)
    {
        _from = from;
        _to = to;
        _overTime = overTime;
        if (_overTime < 0) _overTime = 0f;

        _initialized = true;
        OnBirth?.Invoke();
    }

    private void Update()
    {
        if (_initialized)
        {
            if (_timer < _overTime)
            {
                _timer += Time.deltaTime;
                transform.position = Vector3.Lerp(_from, _to, _timer / _overTime);
            }
            else
            {
                transform.position = _to;
                OnDeath?.Invoke();
                Destroy(this.gameObject);
            }
        }
    }
}
