using System;
using UnityEngine;

[Serializable]
public class GridCell
{
    public Vector3 worldPosition;
    public bool isOccupied;

    public GridCell upwardNeighbor;
    public GridCell botomNeighbor;
    public GridCell leftNeighbor;
    public GridCell rightNeighbor;
    public GridCell forwardCell;
    public GridCell backwardCell;


    //? Pathfinding properties
    public GridCell parent;
    public int gCost;
    public int hCost;
    public int fCost => gCost + hCost;

    public GridCell(Vector3 pos, bool occupied)
    {
        worldPosition = pos;
        isOccupied = occupied;
    }

    public GridCell[] GetNeighbors()
    {
        return new GridCell[]
        {
            upwardNeighbor,
            botomNeighbor,
            leftNeighbor,
            rightNeighbor,
            forwardCell,
            backwardCell
        };
    }
}
