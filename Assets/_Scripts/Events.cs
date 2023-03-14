using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Events : MonoBehaviour
{
    void Awake()
    {
        //DontDestroyOnLoad(this.gameObject);
    }

    public void ReplayGame()
    {
        PlayerManager.gameOver = false;
        SceneManager.LoadScene("Main_Scene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
