using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerManager : MonoBehaviour
{
    public static bool gameOver;
    public GameObject gameOverPanel;

    public static int numberOfCoins;
    public Text coinsText;

    public static bool isGameStarted;
    public GameObject startingText;

    private bool isPaused = false;

    void Start()
    {
        isGameStarted = false;
        gameOver = false;
        numberOfCoins = 0;
    }

    // Update is called once per frame
    void Update()
    {
        GameOver();
        coinsText.text = numberOfCoins.ToString();

        if(SwipeManager.tap)
        {
            isGameStarted = true;
            Destroy(startingText);
        }
    }
    public void GameOver()
    {
        if (!gameOver)
        {
            Time.timeScale = 1;
            gameOver = false;           
        }
        else
        {
            Time.timeScale = 0;
            gameOverPanel.SetActive(true);
        }
    }

    public void Pause()
    {
        if (!isPaused)
        {
            Time.timeScale = 1;
            isPaused = false;
            Debug.Log("I paused the game!");
        }
        else
        {
            Time.timeScale = 0;
            isPaused = true;
            Debug.Log("I unpaused the game!");
        }
    }
}
