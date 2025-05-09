using System.Collections.Generic;
using UnityEngine;

public class GroundSpawner : MonoBehaviour
{
    public GameObject groundTilePrefab;
    public int initialTiles = 5;
    public float tileLength = 50f;

    private Vector3 spawnPosition = Vector3.zero;
    private List<GameObject> activeTiles = new List<GameObject>();
    public Transform player;

    void Start()
    {
        for (int i = 0; i < initialTiles; i++)
        {
            SpawnTile();
        }
    }

    void Update()
    {
        // Spawnea nuevo tile cuando el jugador se acerca al final
        if (player.position.z - 60 > spawnPosition.z - (initialTiles * tileLength))
        {
            SpawnTile();
            RemoveOldTile();
        }
    }

    void SpawnTile()
    {
        GameObject tile = Instantiate(groundTilePrefab, spawnPosition, Quaternion.identity);

        // Llama al método para colocar obstáculos aleatorios
        GroundTile tileScript = tile.GetComponent<GroundTile>();
        if (tileScript != null)
        {
            tileScript.SpawnObstacle();
        }

        activeTiles.Add(tile);
        spawnPosition.z += tileLength;
    }

    void RemoveOldTile()
    {
        if (activeTiles.Count == 0) return;

        GameObject firstTile = activeTiles[0];

        // Solo elimina si el jugador está bien lejos
        if (player.position.z - firstTile.transform.position.z > tileLength + 10f)
        {
            Destroy(firstTile);
            activeTiles.RemoveAt(0);
        }
    }
}
