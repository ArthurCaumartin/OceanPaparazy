
using System.Collections.Generic;
using UnityEngine;

public static class AStarPathfinding
{
    public static Vector3[] GetPath(GridCell startCell, GridCell endCell, int maxIterations = 100)
    {
        List<GridCell> openSet = new List<GridCell>();
        HashSet<GridCell> closedSet = new HashSet<GridCell>();

        openSet.Add(startCell);
        int iterations = 0;

        while (openSet.Count > 0)
        {

            GridCell currentCell = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentCell.fCost ||
                    (openSet[i].fCost == currentCell.fCost && openSet[i].hCost < currentCell.hCost))
                {
                    currentCell = openSet[i];
                }
            }

            openSet.Remove(currentCell);
            closedSet.Add(currentCell);
            iterations++;

            if (currentCell == endCell || iterations >= maxIterations)
            {
                // Reconstruct path
                List<Vector3> path = new List<Vector3>();
                GridCell temp = currentCell;
                while (temp != startCell)
                {
                    path.Add(temp.worldPosition);
                    temp = temp.parent;
                }
                path.Add(startCell.worldPosition);
                path.Reverse();
                return path.ToArray();
            }

            foreach (GridCell neighbor in currentCell.GetNeighbors())
            {
                if (neighbor == null || neighbor.isOccupied || closedSet.Contains(neighbor))
                    continue;

                float tentativeGCost = currentCell.gCost + GetDistance(currentCell, neighbor);
                if (tentativeGCost < neighbor.gCost || !openSet.Contains(neighbor))
                {
                    neighbor.gCost = 10;
                    neighbor.hCost = GetDistance(neighbor, endCell);
                    neighbor.parent = currentCell;

                    if (!openSet.Contains(neighbor))
                        openSet.Add(neighbor);
                }
            }
        }

        return new Vector3[0];
    }

    private static int GetDistance(GridCell a, GridCell b)
    {
        if (a == null || b == null)
        {
            Debug.LogError("GetDistance called with null GridCell");
            return int.MaxValue;
        }
        return (int)(Mathf.Abs(a.worldPosition.x - b.worldPosition.x)
                        + Mathf.Abs(a.worldPosition.y - b.worldPosition.y)
                        + Mathf.Abs(a.worldPosition.z - b.worldPosition.z));
    }
}