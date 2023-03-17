using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    private int maxHealth = 3;
    [SerializeField]
    private int currentHealth;

    public List<GameObject> heartIcons;
    public CameraController cameraController;

    public static bool gameOver;
    public GameObject gameOverPanel;

    public static int numberOfCoins;
    public static int coins;
    public Text coinsText;

    public static bool isGameStarted;
    public GameObject startingText;

    private bool isPaused = false;

    void Start()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
        HealthUI();

        gameOver = false;
        isGameStarted = false;

        numberOfCoins = 0;
        coins = 0;

        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {

        GameOver();
        StartGame();
        AddHealth(); //heals every 50 coins
        
        coinsText.text = numberOfCoins.ToString();
    }

    public void GameOver()
    {
        if (currentHealth <= 0)
        {
            // Slow down time for the death animation
            Time.timeScale = 0.5f;
            StartCoroutine(ShowGameOverPanel());
            gameOver = true;
        }
    }

    private IEnumerator ShowGameOverPanel()
    {
        // Wait for 1 second before showing the game over panel
        yield return new WaitForSeconds(1f);
        Time.timeScale = 0f;
        gameOverPanel.SetActive(true);
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

    public void StartGame()
    {
        if (SwipeManager.tap)
        {
            isGameStarted = true;
            Destroy(startingText);
        }
    }

    public void HealthUI()
    {
        // Initialize the heartIcons list
        heartIcons = new List<GameObject>();
        foreach (GameObject heartIcon in GameObject.FindGameObjectsWithTag("HeartIcon"))
        {
            heartIcons.Add(heartIcon);
        }
    }

    public void TakeFatalDamage()
    {
        gameOver = true;
        currentHealth = 0;
        cameraController.ScreenShake();

        foreach (GameObject heartIcon in heartIcons)
        {
            heartIcon.SetActive(false);
        }

        HealthUI(); // Update the heartIcons list
    }

    public void TakeDamage()
    {
        currentHealth--;

        if (currentHealth >= 0)
        {
            heartIcons[currentHealth].SetActive(false);
            cameraController.ScreenShake();
        }

        if (currentHealth == 0)
        {
            TakeFatalDamage();
        }
    }

    public void AddHealth()
    {
        if (numberOfCoins >= 50 && currentHealth < maxHealth)
        {
            currentHealth++;
            numberOfCoins -= 50;

            heartIcons[currentHealth - 1].SetActive(true); // Re-add the last heart icon
        }
    }

}