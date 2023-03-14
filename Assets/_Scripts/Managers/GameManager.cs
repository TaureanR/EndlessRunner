using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public Text timerTxt;
    private float timer = 0.0f;

    //private PlayerController playerController;

    void Awake()
    {
        //DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        StartCoroutine(Timer());
        //playerController = playerController.GetComponent<PlayerController>();  
    }

    private void Update()
    {
        StopTimer();
    }

    IEnumerator Timer()
    {
        while (true)
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
        if(!PlayerManager.isGameStarted)
        {
            timer = 0.0f;
            StopCoroutine(Timer());
        }
        
    }
}
