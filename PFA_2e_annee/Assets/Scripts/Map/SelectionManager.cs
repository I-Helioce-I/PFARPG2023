using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    [SerializeField] private Camera _cam;

    [SerializeField] private LayerMask _selectionMask;

    [SerializeField] private HexGrid _hexGrid;

    List<Vector3Int> _hexNeighbors = new List<Vector3Int>();

    private void Awake()
    {
        if (_cam == null)
        {
            _cam = Camera.main;
        }
    }

    public void HandleClick(Vector3 mousePosition)
    {
        GameObject result;
        if (FindTarget(mousePosition, out result))
        {
            Hex selectedHex = result.GetComponentInParent<Hex>();
            selectedHex.DisableHighlight();

            foreach(Vector3Int neighbor in _hexNeighbors)
            {
                _hexGrid.GetTileAt(neighbor).DisableHighlight();
            }

            //_hexNeighbors = _hexGrid.GetNeighborsFor(selectedHex.HexCoordinates);
            BFSResult bfsResult = GraphSearch.BFSGetRange(_hexGrid, selectedHex.HexCoordinates, 20);
            _hexNeighbors = new List<Vector3Int>(bfsResult.GetRangePositions());

            foreach (Vector3Int neighbor in _hexNeighbors)
            {
                _hexGrid.GetTileAt(neighbor).EnableHighlight();
            }

            Debug.Log($"Neighbors for {selectedHex.HexCoordinates} are: ");
            foreach (Vector3Int neighborPos in _hexNeighbors)
            {
                Debug.Log(neighborPos);
            }
        }
    }

    private bool FindTarget(Vector3 mousePosition, out GameObject result)
    {
        RaycastHit hit;
        Ray ray = _cam.ScreenPointToRay(mousePosition);
        if (Physics.Raycast(ray, out hit, 100f, _selectionMask))
        {
            result = hit.collider.gameObject;
            return true;
        }

        result = null;
        return false;
    }
}
