using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;


public class Node : IHeapItem<Node>
{
    public Vector2 worldPosition;

    public Vector2 gridPosition;

    public bool isSolid = false;

    public Node parent;

    public int  hCost = 0, gCost = 0;

    int heapIndex;

    public int internalID = 0;

    public Node(Vector2 worldPosition, bool isSolid, Vector2 gridPosition)
    {
        this.worldPosition = worldPosition;
        this.isSolid = isSolid;
        this.gridPosition = gridPosition;
    }

    public bool CanAcces()
    {
        return isSolid == false;
    }


    public int HeapIndex
    {
        get
        {
            return heapIndex;
        }
        set
        {
            heapIndex = value;
        }
    }


    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }


    public int CompareTo(Node other)
    {
        int compare = fCost.CompareTo(other.fCost);
        if (compare == 0)
        {
            compare = hCost.CompareTo(other.hCost);
        }

        return -compare;
    }
}

