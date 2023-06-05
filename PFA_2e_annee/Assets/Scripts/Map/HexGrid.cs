using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGrid : MonoBehaviour
{
    Dictionary<Vector3Int, Hex> _hexTileDict = new Dictionary<Vector3Int, Hex>();
    Dictionary<Vector3Int, List<Vector3Int>> _hexTilesNeighborsDict = new Dictionary<Vector3Int, List<Vector3Int>>();

    private void Start()
    {
        foreach(Hex hex in FindObjectsOfType<Hex>())
        {
            _hexTileDict[hex.HexCoordinates] = hex;
        }


    }

    public Hex GetTileAt(Vector3Int hexCoordinates)
    {
        Hex result = null;
        _hexTileDict.TryGetValue(hexCoordinates, out result);
        return result;
    }

    public List<Vector3Int> GetNeighborsFor(Vector3Int hexCoordinates)
    {
        if (_hexTileDict.ContainsKey(hexCoordinates) == false)
        {
            return new List<Vector3Int>();
        }

        if (_hexTilesNeighborsDict.ContainsKey(hexCoordinates))
        {
            return _hexTilesNeighborsDict[hexCoordinates];
        }

        _hexTilesNeighborsDict.Add(hexCoordinates, new List<Vector3Int>());

        foreach (var direction in Direction.GetDirectionList(hexCoordinates.z))
        {
            if (_hexTileDict.ContainsKey(hexCoordinates + direction))
            {
                _hexTilesNeighborsDict[hexCoordinates].Add(hexCoordinates + direction);
            }
        }

        return _hexTilesNeighborsDict[hexCoordinates];
    }

}

public static class Direction
{
    public static List<Vector3Int> directionOffsetOdd = new List<Vector3Int>
    {
        new Vector3Int(-1, 0, 1), //N1
        new Vector3Int(0, 0, 1), //N2
        new Vector3Int(1, 0, 0), //E
        new Vector3Int(0, 0, -1), //S2
        new Vector3Int(-1, 0, -1), //S1
        new Vector3Int(-1, 0, 0), //W
    };

    public static List<Vector3Int> directionOffsetEven = new List<Vector3Int>
    {
        new Vector3Int(0, 0, 1), //N1
        new Vector3Int(1, 0, 1), //N2
        new Vector3Int(1, 0, 0), //E
        new Vector3Int(1, 0, -1), //S2
        new Vector3Int(0, 0, -1), //S1
        new Vector3Int(-1, 0, 0), //W
    };

    public static List<Vector3Int> GetDirectionList(int z)
        => z % 2 == 0 ? directionOffsetEven : directionOffsetOdd;
}
