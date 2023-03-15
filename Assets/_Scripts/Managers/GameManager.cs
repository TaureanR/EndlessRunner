using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public Text timerTxt;
    private float timer = 0.0f;
    private bool isTimerRunning = true;

    void Start()
    {
        StartCoroutine(Timer());
    }

    void Update()
    {
        if (PlayerManager.gameOver)
        {
            isTimerRunning = false;
        }
    }

    IEnumerator Timer()
    {
        // Wait until the game is started
        while (!PlayerManager.isGameStarted)
        {
            yield return null;
        }

        while (isTimerRunning)
        {
            // Increment the timer by the time since the last frame
            timer += Time.deltaTime;

            // Update the timer text
            timerTxt.text = timer.ToString("F2");

            // Wait for the next frame
            yield return null;
        }
    }

    void StopTimer()
    {
        if (!PlayerManager.isGameStarted)
        {
            timer = 0.0f;
            StopCoroutine(Timer());
        }

        if (PlayerManager.gameOver == true)
        {
            StopCoroutine(Timer());
        }
    }
}
