using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class myLine : MonoBehaviour
{
    public Vector3 lastPosition; // last position of last vertex
    public Vector3 startPosition; // starting position (first vertex)
    public int id; // id of the dots
    public int startDot; // gives the index of the dot...

    private LineRenderer lr;
    private int vertexCount = 2;

    public void init()
    {
        this.lr = GetComponent<LineRenderer>();

        lr.SetPosition(0,startPosition);
        lr.SetPosition(1,lastPosition);

    }

    public void addVertex(float height, float width)
    {
        vertexCount++;
        lr.positionCount = vertexCount;
        lr.SetPosition(vertexCount - 1, new Vector3(height, width, 0));

    }

    public void setLastPosition(float height, float width)
    {
        int vertexCount = lr.positionCount;
        lr.SetPosition(vertexCount-1, new Vector3(width, height, 0));
    }


}
