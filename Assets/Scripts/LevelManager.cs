using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public GameObject blockBackground;
    public Sprite[] flowerSprites;
    public GameObject PrefabFlower;
    public AudioClip succes02;
    public GameObject nextBut;
    
    private LevelGenerator lg;
    private bool hasWon= false;
    private Vector3 characterPos;
    private AudioSource source;

    // Start is called before the first frame update
    void Start()
    {

        lg = GameObject.Find("LevelGenerator").GetComponent<LevelGenerator>();
        characterPos = Vector3.zero;
        source = GetComponent<AudioSource>();

    }

    private void Update()
    {
        if (hasWon)
        {
            
        }
    }

    public void WinCondition()
    {

        float height1 = lg.characterPositions[0].x;
        float width1 = lg.characterPositions[0].y;

        float height2 = lg.characterPositions[0].z;
        float width2 = lg.characterPositions[0].w;

        characterPos = new Vector3(height1 + (height2 - height1) / 2f + lg.offset, width1 + (width2 - width1) / 2f + lg.offset, 0f);
        Instantiate(blockBackground, characterPos, Quaternion.identity, this.transform);
        
        source.clip = succes02;
        source.Play();

        nextBut.SetActive(true);

        StartCoroutine("SpawnFlowers");
        hasWon = true;
    }

    IEnumerator SpawnFlowers()
    {
        int nFlowers=50;
        for(int i = 1; i< nFlowers; i++)
        {

            //Vector3 spawnPos = new Vector3(Random.Range(0f, lg.height), Random.Range(0f, lg.width), 0);
            // try polar coordinates
            //float xPos = i/3 * Mathf.Cos(Random.Range(0f, Mathf.PI*2));
            //float yPos = i/3 * Mathf.Sin(Random.Range(0f, Mathf.PI*2));
            //Vector3 spawnPos = new Vector3(xPos, yPos, 0) + characterPos;

            float mean = 0f;
            float stdDev = 2f;
            float u1 = 1.0f - Random.value; //uniform(0,1] random doubles
            float u2 = 1.0f - Random.value;

            float u3 = 1.0f - Random.value; //uniform(0,1] random doubles
            float u4 = 1.0f - Random.value;

            float randStdNormal1 = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) *
                         Mathf.Sin(2.0f * Mathf.PI * u2); //random normal(0,1)

            float randStdNormal2 = Mathf.Sqrt(-2.0f * Mathf.Log(u3)) *
                         Mathf.Sin(2.0f * Mathf.PI * u4); //random normal(0,1)

            float xPos = mean + stdDev * randStdNormal1; //random normal(mean,stdDev^2)
            float yPos = mean + stdDev * randStdNormal2; //random normal(mean,stdDev^2)

            Vector3 spawnPos = new Vector3(xPos, yPos, 0) + characterPos;

            if (Vector3.Distance(spawnPos,characterPos) > 1)
            {
                SpriteRenderer sr = Instantiate(PrefabFlower, spawnPos, Quaternion.identity, this.transform).GetComponent<SpriteRenderer>();
                sr.sprite = flowerSprites[(int)Random.Range(0, flowerSprites.Length)];
                print(spawnPos);
            }


            yield return new WaitForSeconds(.2f);
        }
        //hasWon = false;
        //yield return null;
    }

    public void QuitGame()
    {

        Debug.Log("Quit");
        Application.Quit();
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
