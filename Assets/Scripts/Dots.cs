using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dots : MonoBehaviour
{

    [HideInInspector]
    public bool hasLine = false;
    [HideInInspector]
    public bool isConnected = false;
    public int id = 0;

    private LevelGenerator lg;
    private Vector2Int[] dotPositions;

    public void initDots()
    {
        this.lg = GameObject.Find("LevelGenerator").GetComponent<LevelGenerator>();
        dotPositions = new Vector2Int[2];
    }

    public void setDotPosition(float height, float width, int index)
    {
        Transform myDot = this.transform.GetChild(index);
        myDot.position = new Vector3(height, width, index);
        dotPositions[index] = new Vector2Int(Mathf.FloorToInt(height), Mathf.FloorToInt(width));
    }

    public void setConnected(bool connect)
    {
        isConnected = connect;
        this.lg.checkConnections();
    }

    public Vector2Int getOtherDotPos(int height, int width)
    {
        Vector2Int pos = new Vector2Int(height, width);
        if(Vector2Int.Distance(pos,dotPositions[0]) < Vector2Int.Distance(pos, dotPositions[1]))
        {
            return dotPositions[1];
        }
        else
        {
            return dotPositions[0];
        }
    }
}
