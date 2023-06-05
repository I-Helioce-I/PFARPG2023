using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GraphSearch
{
    public static BFSResult BFSGetRange(HexGrid grid, Vector3Int startPoint, int movementPoints)
    {
        Dictionary<Vector3Int, Vector3Int?> _visitedHexes = new Dictionary<Vector3Int, Vector3Int?>();
        Dictionary<Vector3Int, int> _costSoFar = new Dictionary<Vector3Int, int>();
        Queue<Vector3Int> _hexesToVisitQueue = new Queue<Vector3Int>();

        _hexesToVisitQueue.Enqueue(startPoint);
        _costSoFar.Add(startPoint, 0);
        _visitedHexes.Add(startPoint, null);

        while (_hexesToVisitQueue.Count > 0)
        {
            Vector3Int currentHex = _hexesToVisitQueue.Dequeue();
            foreach (Vector3Int neighborCoordinates in grid.GetNeighborsFor(currentHex))
            {
                if (grid.GetTileAt(neighborCoordinates).IsObstacle()) continue;
                int hexCost = grid.GetTileAt(neighborCoordinates).GetTerrainCost();
                int currentCost = _costSoFar[currentHex];
                int newCost = currentCost + hexCost;

                if (newCost <= movementPoints)
                {
                    if (!_visitedHexes.ContainsKey(neighborCoordinates))
                    {
                        _visitedHexes[neighborCoordinates] = currentHex;
                        _costSoFar[neighborCoordinates] = newCost;
                        _hexesToVisitQueue.Enqueue(neighborCoordinates);
                    }
                    else if (_costSoFar[neighborCoordinates] > newCost)
                    {
                        _costSoFar[neighborCoordinates] = newCost;
                        _visitedHexes[neighborCoordinates] = currentHex;
                    }
                }
            }
        }

        return new BFSResult { VisitedHexesDict = _visitedHexes };
    }

    public static List<Vector3Int> GeneratePathBFS(Vector3Int current, Dictionary<Vector3Int, Vector3Int?> visitedHexesDict)
    {
        List<Vector3Int> path = new List<Vector3Int>();
        path.Add(current);
        while (visitedHexesDict[current]!= null)
        {
            path.Add(visitedHexesDict[current].Value);
            current = visitedHexesDict[current].Value;
        }
        path.Reverse();
        return path.Skip(1).ToList();
    }
}

public struct BFSResult
{
    public Dictionary<Vector3Int, Vector3Int?> VisitedHexesDict;

    public List<Vector3Int> GetPathTo(Vector3Int destination)
    {
        if (VisitedHexesDict.ContainsKey(destination) == false)
        {
            return new List<Vector3Int>();
        }
        return GraphSearch.GeneratePathBFS(destination, VisitedHexesDict);
    }

    public bool IsHexPositionInRange(Vector3Int position)
    {
        return VisitedHexesDict.ContainsKey(position);
    }

    public IEnumerable<Vector3Int> GetRangePositions()
    {
        return VisitedHexesDict.Keys;
    }
}