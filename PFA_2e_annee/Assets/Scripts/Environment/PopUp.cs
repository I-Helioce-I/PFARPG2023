using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUp : MonoBehaviour
{
    public GameObject PopUpObject;

    [SerializeField] private float _showDuration = 5f;
    private float _timer = 0f;

    private bool _isShowing = false;

    private void Start()
    {
        Show(false);
    }

    private void Update()
    {
        if (_isShowing)
        {
            if (_timer < _showDuration)
            {
                _timer += Time.deltaTime;
            }
            else
            {
                Show(false);
            }
        }
    }

    public void Show(bool value)
    {
        _isShowing = value;
        _timer = 0f;

        PopUpObject.SetActive(value);
    }
}
