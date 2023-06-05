using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickSystem : MonoBehaviour
{
    [SerializeField] private Camera _cam;

    [SerializeField] private LayerMask _clickableMask;
    [SerializeField] private LayerMask _obstacleMask;

    private void Awake()
    {
        if (_cam == null)
        {
            _cam = Camera.main;
        }
    }

    public void HandleClick(Vector3 mousePosition)
    {
        Clickable result;

        if (FindTarget(mousePosition, out result))
        {
            Clickable selectedClickable = result;
            selectedClickable.OnClick();
        }
    }

    private bool FindTarget(Vector3 mousePosition, out Clickable result)
    {
        RaycastHit hit;
        Ray ray = _cam.ScreenPointToRay(mousePosition);

        if (!Physics.Raycast(ray, out hit, 100f, _obstacleMask))
        {
            if (Physics.Raycast(ray, out hit, 100f, _clickableMask))
            {
                result = hit.collider.gameObject.GetComponent<Clickable>();
                return true;
            }
        }

        result = null;
        return false;
    }
}
