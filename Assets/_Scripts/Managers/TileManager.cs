using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    // A list of all active tiles in the game
    private List<GameObject> activeTiles = new List<GameObject>();

    // The prefabs for the current level
    [SerializeField] private GameObject[] currentTilePrefabs;

    // An array of prefabs for each level
    [SerializeField] private GameObject[] tilePrefabs;  // level 1
    [SerializeField] private GameObject[] tilePrefabs2; // level 2
    [SerializeField] private GameObject[] tilePrefabs3; // level 3

    // The spawn location for new tiles and the length of each tile
    [SerializeField] private float zSpawn = 0;
    [SerializeField] private float tileLength = 30;

    // The number of tiles to spawn at the start of the game
    [SerializeField] private int numberOfTiles = 5;

    // The current level of the game
    [SerializeField] private int currentLevel;


    // The transform of the player object
    public Transform playerTransform;

    // Called when the object is first created
    void Start()
    {
        // Set the current tile prefabs based on the current level
        if (currentLevel == 1)
        {
            currentTilePrefabs = tilePrefabs;
        }
        else if (currentLevel == 2)
        {
            currentTilePrefabs = tilePrefabs2;
        }
        else if (currentLevel == 3)
        {
            currentTilePrefabs = tilePrefabs3;
        }

        // Spawn the initial set of tiles
        for (int i = 0; i < numberOfTiles; i++)
        {
            if (i == 0)
                SpawnTile(0);
            else
                SpawnTile(Random.Range(0, currentTilePrefabs.Length));
        }
    }

    // Called every frame
    void Update()
    {
        // Update the current level
        currentLevel = GameManager.level;

        // Check if a new tile needs to be spawned
        if (playerTransform.position.z - 20 > zSpawn - (numberOfTiles * tileLength))
        {
            // Update the current tile prefabs if necessary
            if (currentLevel == 1 && currentTilePrefabs != tilePrefabs)
            {
                currentTilePrefabs = tilePrefabs;
            }
            else if (currentLevel == 2 && currentTilePrefabs != tilePrefabs2)
            {
                currentTilePrefabs = tilePrefabs2;
            }
            else if (currentLevel == 3 && currentTilePrefabs != tilePrefabs3)
            {
                currentTilePrefabs = tilePrefabs3;
            }

            // Spawn a new tile and delete the oldest one
            SpawnTile(Random.Range(0, currentTilePrefabs.Length));
            DeleteTile();
        }
    }

    // Spawn a new tile at the specified index
    public void SpawnTile(int tileIndex)
    {
        GameObject go = Instantiate(currentTilePrefabs[tileIndex], transform.forward * zSpawn, transform.rotation);
        activeTiles.Add(go);
        zSpawn += tileLength;
    }

    // Delete the oldest tile
    private void DeleteTile()
    {
        Destroy(activeTiles[0]);
        activeTiles.RemoveAt(0);
    }
}
