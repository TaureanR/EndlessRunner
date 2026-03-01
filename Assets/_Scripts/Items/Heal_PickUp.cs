using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal_PickUp : MonoBehaviour
{
    public PlayerManager playerManager;

    void Awake()
    {
        playerManager = FindObjectOfType<PlayerManager>();
    }

    void Update()
    {
        transform.Rotate(50 * Time.deltaTime, 0, 0);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (playerManager != null)
            {
                playerManager.AddHealth();
                Debug.Log("I healed!");
                Destroy(gameObject);
            }
            else
            {
                Debug.LogError("PlayerManager not found!");
            }
        }
    }
}
