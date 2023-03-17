using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    private List<GameObject> activeTiles = new List<GameObject>();

    [SerializeField] private GameObject[] tilePrefabs;  // level 1
    [SerializeField] private GameObject[] tilePrefabs2; // level 2
    [SerializeField] private GameObject[] tilePrefabs3; // level 3

    [SerializeField] private float zSpawn = 0;
    [SerializeField] private float tileLength = 30;
    [SerializeField] private int numberOfTiles = 5;

    [SerializeField] private int currentLevel;
    [SerializeField] private GameObject[] currentTilePrefabs;

    public Transform playerTransform;

    void Start()
    {
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

        for (int i = 0; i < numberOfTiles; i++)
        {
            if (i == 0)
                SpawnTile(0);
            else
                SpawnTile(Random.Range(0, currentTilePrefabs.Length));
        }
    }


    void Update()
    {
        currentLevel = GameManager.level;

        if (playerTransform.position.z - 20 > zSpawn - (numberOfTiles * tileLength))
        {
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

            SpawnTile(Random.Range(0, currentTilePrefabs.Length));
            DeleteTile();
        }
    }

    public void SpawnTile(int tileIndex)
    {
        GameObject go = Instantiate(currentTilePrefabs[tileIndex], transform.forward * zSpawn, transform.rotation);
        activeTiles.Add(go);
        zSpawn += tileLength;
    }

    private void DeleteTile()
    {
        Destroy(activeTiles[0]);
        activeTiles.RemoveAt(0);
    }
}
