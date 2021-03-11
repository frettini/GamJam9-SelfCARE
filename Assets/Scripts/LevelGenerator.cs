using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public int height = 5;
    public int width = 5;
    public float offset = 0.5f;

    public GameObject gridPrefab;
    public GameObject charPrefab;
    public GameObject[] dotPrefab;

    // characterPosition -> vector 4, first two give lower corner and last two upper corner
    // character placed in the middle
    public Vector4[] characterPositions;
    // dotPositions -> vector 4:x1,y1,x2,y2
    public Vector4[] dotPositions;
    // sprite prefab for each dots
    //public Sprite[] dotSprites;


    [HideInInspector]
    public Dots[] myDots;
    private GameObject[] allChars;
    private MouseControl mc;
    private myGrid grid;
    private GameObject mainCam;
    private LevelManager lm;

    // Start is called before the first frame update
    void Awake()
    {
        this.allChars = new GameObject[characterPositions.Length];

        this.grid = Instantiate(gridPrefab,Vector3.zero, Quaternion.identity).GetComponent<myGrid>();
        this.grid.initGrid(this.height, this.width, this.offset);
        this.placeCharacter();
        this.placeDots();
        this.genGrid();

        this.mc = GameObject.Find("MouseController").GetComponent<MouseControl>();
        this.mc.grid = this.grid;

        this.mainCam = GameObject.Find("Main Camera");
        this.mainCam.transform.position = new Vector3(this.height/2f, this.width/2f, this.mainCam.transform.position.z);
        this.mainCam.GetComponent<Camera>().orthographicSize = width / 2f;

        this.lm = GameObject.Find("LevelManager").GetComponent<LevelManager>();
    }

    private void placeCharacter()
    {
        if(characterPositions != null)
        {
            int ind = 0;
            foreach(Vector4 charPos in characterPositions)
            {
                // get lower corner 
                int height1 = (int)charPos.x;
                int width1 = (int)charPos.y;
                // get upper corner
                int height2 = (int)charPos.z;
                int width2 = (int)charPos.w;

                for(int i=height1; i <= height2; i++)
                {
                    for (int j = width1; j <= width2; j++)
                    {
                        this.grid.setGridValue(i,j,-2);
                    }
                }

                // instantiate the character in the middle of the area
                this.allChars[ind] = Instantiate(charPrefab, 
                            new Vector3(height1 + (height2-height1)/2f + offset, width1 + (width2-width1)/2f + offset, 0f), 
                            Quaternion.identity, this.transform);
                ind++;
            }
        }
        
    }

    private void placeDots()
    {
        this.myDots = new Dots[this.dotPositions.Length];

        if (this.dotPositions != null)
        {
            int ind = 0;
            foreach (Vector4 dotPos in this.dotPositions)
            {
                Dots dt = Instantiate(dotPrefab[ind], Vector3.zero, Quaternion.identity, this.transform).GetComponent<Dots>();
                
                /*SpriteRenderer[] srs = dt.GetComponentsInChildren<SpriteRenderer>();
                foreach(SpriteRenderer sr in srs)
                {
                    sr.sprite = dotSprites[ind];
                }*/

                myDots[ind] = dt;
                myDots[ind].initDots();

                // get lower corner 
                int height1 = (int)dotPos.x;
                int width1 = (int)dotPos.y;
                // get upper corner
                int height2 = (int)dotPos.z;
                int width2 = (int)dotPos.w;

                this.grid.setGridValue(height1, width1, 2);
                this.grid.setGridValue(height2, width2, 2);
                this.grid.setGridIndex(height1, width1, ind);
                this.grid.setGridIndex(height2, width2, ind);

                myDots[ind].setDotPosition(height1 + offset, width1 + offset, 0);
                myDots[ind].setDotPosition(height2 + offset, width2 + offset, 1);

                myDots[ind].id = ind;
                ind++;
            }
        }
    }

    private void genGrid()
    {
        this.grid.genGrid();
    }

    public void checkConnections()
    {
        int count = 0;
        foreach(Dots dot in this.myDots)
        {
            if(dot.isConnected)
            {
                count++;
            }
        }

        if(count == this.myDots.Length)
        {
            lm.WinCondition();
            print("You WOOOOOON");

            foreach(GameObject character in allChars)
            {
                character.GetComponent<Animator>().SetTrigger("openEyes");
            }
        }
    }

}
