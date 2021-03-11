using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseControl : MonoBehaviour
{
    public GameObject dotLine;
    
    [HideInInspector]
    public myGrid grid;
    public AudioClip succes01;
    public AudioClip dotClick;

    private bool isTracking = false;
    private float offset = 0.5f;
    private Vector2Int lastPos;
    private Dots trackedDot;

    private LevelGenerator lg;
    private myLine[] allLines;
    private myLine line;
    private int trackingInd = 0;
    private AudioSource source;

    private void Start()
    {
        lastPos = new Vector2Int(-1,-1);
        lg = GameObject.Find("LevelGenerator").GetComponent<LevelGenerator>();
        source = GetComponent<AudioSource>();
        allLines = new myLine[lg.myDots.Length];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // get grid value at grid point
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            int height = 0;
            int width = 0;
            grid.getXY(worldPos, out height, out width);
            int value = grid.getGridValue(height, width);

            if (!isTracking)
            {
                if(value == 2)
                {
                    // if just validated this cell and last pos is there, then dont delete 
                    if(lastPos == new Vector2Int(height, width))
                    {
                        lastPos= new Vector2Int(-1, -1);
                    }
                    else
                    {
                        // start tracking                 
                        startTracking(height, width);
                    }

                }
                else if (value == 1)
                {
                    // delete line at current location
                    deleteLine(height,width);
                }
                else if (value == 0)
                {
                    // do nothing
                }
                else
                {
                    //do nothing
                }
            }
            else
            {
                if (value == 1)
                {
                    //stop tracking
                    this.stopTracking();
                }
                else if(value == 0)
                {
                    //stop tracking
                    this.stopTracking();
                }
                else if (value == -1)
                {
                    //stop tracking
                    this.stopTracking();
                }

            }


        }

        if (isTracking)
        {
            // get mouse world and grid position
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            int height = 0;
            int width = 0;
            grid.getXY(worldPos, out height, out width);

            // check if mouse is in an adjacent block
            //if (Mathf.Abs(height-lastPos.x) == 1 ^ Mathf.Abs(width - lastPos.y) == 1)
            if (Mathf.Abs(height - lastPos.x) + Mathf.Abs(width - lastPos.y) == 1)

                {
                // get value at next grid value
                int value = grid.getGridValue(height, width);
                //print(value);

                if(value == 0)
                {
                    // free block: create vertex and set last post to current pos
                    //create a new vertex
                    this.allLines[trackingInd].addVertex(height+offset,width+offset);
                    this.lastPos = new Vector2Int(height, width);
                    grid.setGridValue(height, width, 1); // replace by occupied block
                    grid.setGridIndex(height, width, this.allLines[trackingInd].id);


                }
                else if(value == 2)
                {
                    //check for id and diff child index -> if all true connect
                    Dots testDot;
                    int childIndex;
                    GetDot(out testDot, out childIndex);

                    if(testDot.id == allLines[trackingInd].id && childIndex != allLines[trackingInd].startDot)
                    {
                       
                        this.allLines[trackingInd].addVertex(height + offset, width + offset);
                        this.lastPos = new Vector2Int(height, width);
                        this.stopTracking();
                        testDot.setConnected(true);

                        source.clip = succes01;
                        source.Play();
                        // add a flag that nullifies the click on the same cell later on...
                        Debug.Log("Successfully connected");
                    }
                }
                else if(value == 1)
                {
                    //do nothing
                }
                
            }
            
            
        }
    }

    private void GetDot(out Dots dot, out int index)
    {
        dot = null;
        index = 0;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
        if (hit.collider)
        {
            if(hit.collider.tag == "Dots")
            {
                Transform childDot = hit.collider.transform;
                dot = childDot.parent.GetComponent<Dots>();
                index = childDot.GetSiblingIndex();
            }
        }

    }

    private void startTracking(int height, int width)
    {
        // get the dot for index and childIndex
        
        int childIndex = 0;
        this.GetDot(out trackedDot, out childIndex);
        this.trackingInd = trackedDot.id;

        if (trackedDot != null)
        {
            if(trackedDot.hasLine == true)
            {
                // delete current line to start a new one
                deleteLine(height, width);
            }

            // instantiate a line and position it to the clicked dot 
            // make sure that we track this line in particular -> use tracking index 

            // instantiate line
            myLine line = Instantiate(dotLine, new Vector3(height, width, 0f), Quaternion.identity, this.transform).GetComponent<myLine>();

            line.startPosition = new Vector2(height + offset, width + offset);
            line.lastPosition = new Vector2(height + offset, width + offset);
            line.init();
            line.id = trackedDot.id; // whatever the dot id is
            line.startDot = childIndex;

            this.allLines[this.trackingInd] = line;

            // turn has line true so that next click it removes it
            trackedDot.hasLine = true;

            // activate mouse tracking
            isTracking = true;
            lastPos = new Vector2Int(height, width);
        }

        source.clip = dotClick;
        source.Play();
    }

    private void stopTracking()
    {
        this.isTracking = false;
    }

    private void deleteLine(int height, int width)
    {
        int deleteIndex = grid.getGridIndex(height, width);

        lg.myDots[deleteIndex].hasLine = false;
        lg.myDots[deleteIndex].isConnected = false;

        Destroy(allLines[deleteIndex].gameObject);

        for (int i = 0; i < this.lg.height; i++)
        {
            for (int j = 0; j < this.lg.width; j++)
            {
                if(grid.getGridIndex(i,j) == deleteIndex && grid.getGridValue(i, j) != 2)
                {
                    grid.setGridIndex(i, j, -1);
                    grid.setGridValue(i, j, 0);
                }
            }
        }

    }
}
