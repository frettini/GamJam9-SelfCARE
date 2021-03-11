using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuUi : MonoBehaviour
{
    public GameObject textHowTo;

    private bool isHowTo = false;

    public void QuitGame()
    {

        Debug.Log("Quit");
        Application.Quit();
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Restart()
    {
        SceneManager.LoadScene(1);
    }

    public void HowTo()
    {
        isHowTo = !isHowTo;
        textHowTo.SetActive(isHowTo);
    }

}
