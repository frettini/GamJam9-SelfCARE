using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class myGrid : MonoBehaviour
{

    private int height;
    private int width;
    public float offset = 0.5f;
    public GameObject marker;
    public GameObject charTile;

    private int[,] gridArray;
    private int[,] gridIndex;
    // this exists only for debug
    private GameObject[,] markers;

    // Start is called before the first frame update
    public void initGrid(int height, int width, float offset)
    {
        this.height = height;
        this.width = width;
        this.offset = offset;
        
        // in C sharp it initializes to 0 automatically! scary
        this.gridArray = new int[this.height, this.width];
        this.gridIndex = new int[this.height, this.width];
        // initialize grid index
        for (int i = 0; i < this.height; i++)
        {
            for (int j = 0; j < this.width; j++)
            {
                this.gridIndex[i,j] = -1;

            }
        }

        this.markers = new GameObject[this.height, this.width];
    }

    public void genGrid()
    {
        for (int i = 0; i < this.height; i++)
        {
            for (int j = 0; j < this.width; j++)
            {
                if (this.gridArray[i,j] >= 0)
                {

                    GameObject go = Instantiate(marker, new Vector3(i + offset, j + offset, 0), Quaternion.identity, this.transform);
                    this.markers[i, j] = go;
                }
                if(this.gridArray[i,j] == -2)
                {
                    GameObject go = Instantiate(charTile, new Vector3(i + offset, j + offset, 0), Quaternion.identity, this.transform);
                }
            }
        }
    }

    public void setGridValue(int height, int width, int value)
    {
        this.gridArray[height, width] = value;
    }

    public void setGridIndex(int height, int width, int index)
    {
        this.gridIndex[height, width] = index;
    }

    public int getGridValue(int height, int width)
    {
        if(height>=this.height || height < 0 || width >= this.width || width < 0)
        {
            // return an error value if outside of range
            return -1;
        }

        return this.gridArray[height, width];
    }

    public int getGridIndex(int height, int width)
    {
        if (height >= this.height || height < 0 || width >= this.width || width < 0)
        {
            // return an error value if outside of range
            return -1;
        }

        return this.gridIndex[height, width];
    }

    public void getXY(Vector3 worldPos, out int height, out int width )
    {
        height = Mathf.FloorToInt(worldPos.x);
        width = Mathf.FloorToInt(worldPos.y);
    }
    
}
