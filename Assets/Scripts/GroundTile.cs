using UnityEngine;

public class GroundTile : MonoBehaviour
{
    public GameObject[] obstaclePrefabs;
    public Transform[] spawnPoints;

    void Start()
    {
        SpawnObstacle();
    }

    public void SpawnObstacle()

    {
        // Elegimos un punto al azar
        int index = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[index];

        // Elegimos un obstáculo al azar.
        GameObject obstacleToSpawn = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];

        // Instanciamos
        Instantiate(obstacleToSpawn, spawnPoint.position, Quaternion.identity);

    }
}
