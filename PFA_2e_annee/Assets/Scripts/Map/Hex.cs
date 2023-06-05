using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Hex : MonoBehaviour
{
    public enum HexTerrainType
    {
        None,
        Default,
        Difficult,
        Impassable,
        Road,
        Water,
    }

    [SerializeField] private HexTerrainType _hexTerrainType;

    private HexMapCoordinates _hexMapCoordinates;
    private GlowHighlight _glowHighlight;
    public Vector3Int HexCoordinates => _hexMapCoordinates.HexCoordinates;

    private void Awake()
    {
        _hexMapCoordinates = GetComponent<HexMapCoordinates>();
        _glowHighlight = GetComponent<GlowHighlight>();
    }

    public int GetTerrainCost()
    {
        switch (_hexTerrainType)
        {
            case HexTerrainType.Default:
                return 10;
            case HexTerrainType.Difficult:
                return 20;
            case HexTerrainType.Road:
                return 5;
            default:
                throw new Exception($"Hex of type {_hexTerrainType} not supported");
        }
    }

    public bool IsObstacle()
    {
        return _hexTerrainType == HexTerrainType.Impassable;
    }

    public void EnableHighlight()
    {
        _glowHighlight.ToggleGlow(true);
    }

    public void DisableHighlight()
    {
        _glowHighlight.ToggleGlow(false);
    }
}
