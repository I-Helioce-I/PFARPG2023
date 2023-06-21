using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TranslatePosition : MonoBehaviour
{
    //Place on root gameobject designed to move in world space.

    [Header("Start and destination")]
    public Transform StartTransform;
    public Transform EndTransform;

    public float TravelOverTime = 5f;

    public bool DestroyOnReachEnd = false;
    public bool ActivateOnStart = false;

    public UnityEvent OnStart;
    public UnityEvent OnEnd;

    private bool _isMoving = false;
    private float timer = 0f;

    private void Start()
    {
        if (ActivateOnStart) Activate();
    }
    public void Activate()
    {
        if (StartTransform && EndTransform)
        {
            OnStart?.Invoke();
            _isMoving = true;
            transform.position = StartTransform.position;
        }
    }

    private void Update()
    {
        if (_isMoving)
        {
            if (timer < TravelOverTime)
            {
                timer += Time.deltaTime;
                Vector3 lerpdPosition = Vector3.Lerp(StartTransform.position, EndTransform.position, timer / TravelOverTime);
                transform.position = lerpdPosition;
            }
            else
            {
                OnEnd?.Invoke();
                if (DestroyOnReachEnd)
                {
                    Destroy(this.gameObject);
                }
                else
                {
                    transform.position = EndTransform.position;
                    _isMoving = false;
                    this.enabled = false;
                }

            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(StartTransform.position, .5f);
        Gizmos.DrawWireSphere(EndTransform.position, .5f);
        Gizmos.DrawLine(StartTransform.position, EndTransform.position);
    }
}
