using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexMapCoordinates : MonoBehaviour
{
    public static float xOffset = 2f, yOffset = 1f, zOffset = 1.73f;

    [Header("Offset coordinates")]
    [SerializeField] private Vector3Int _offsetCoordinates;
    public Vector3Int HexCoordinates
    {
        get
        {
            return _offsetCoordinates;
        }
    }

    private void Awake()
    {
        _offsetCoordinates = ConvertPositionToOffset(transform.position);
    }

    private Vector3Int ConvertPositionToOffset(Vector3 position)
    {
        int x = Mathf.CeilToInt(position.x / xOffset);
        int y = Mathf.RoundToInt(position.y / yOffset);
        int z = Mathf.RoundToInt(position.z / zOffset);
        return new Vector3Int(x, y, z);
    }
}
