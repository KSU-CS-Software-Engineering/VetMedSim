using UnityEngine;
using System.Collections;

public class Node : IHeapItem<Node>
{

    public bool walkable;
    public Vector3 worldPosition;
    public int gridX;
    public int gridY;

    public int gCost;
    public int hCost;
    public Node parent;
    int heapIndex;

    /// <summary>
    /// Constructor for the Nodes
    /// </summary>
    /// <param name="_walkable">If the node is able to be walked on or through</param>
    /// <param name="_worldPos">The position of the node relative to the world (There is an offset)</param>
    /// <param name="_gridX">x grid position</param>
    /// <param name="_gridY">y grid position</param>
    public Node(bool _walkable, Vector3 _worldPos, int _gridX, int _gridY)
    {
        walkable = _walkable;
        worldPosition = _worldPos;
        gridX = _gridX;
        gridY = _gridY;
    }

    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
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

    /// <summary>
    /// Compares current node's f and g costs to another node
    /// </summary>
    /// <param name="nodeToCompare">the given node that is being compared to</param>
    /// <returns></returns>
    public int CompareTo(Node nodeToCompare)
    {
        int compare = fCost.CompareTo(nodeToCompare.fCost);
        if (compare == 0)
        {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }
        return -compare;
    }
}