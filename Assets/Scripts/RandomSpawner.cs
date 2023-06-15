using System.Collections;
using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    public Transform[] spawnPoints;
    public GameObject[] enemies;

    public float initialSpawnInterval = 10f;
    public float spawnIntervalDecrease = 0.01f;
    public float minSpawnInterval = 0.5f;

    public float currentSpawnInterval;
    public int maxEnemiesPerSpawn = 1;  // Maximum enemies per spawn point
    private int[] currentEnemiesSpawned;  // Track the number of enemies spawned for each spawn point

    public float minSpawnDelay = 2f;
    public float maxSpawnDelay = 25f;

    void Start()
    {
        currentSpawnInterval = initialSpawnInterval;
        currentEnemiesSpawned = new int[spawnPoints.Length]; // Initialize the array with correct length

        

            
        

        StartCoroutine(SpawnEnemy());
    }

    IEnumerator SpawnEnemy()
    {
        while (true)
        {
            bool allSpawnPointsOccupied = true; // Flag to check if all spawn points are occupied

            // Shuffle the spawn points array
            for (int i = 0; i < spawnPoints.Length; i++)
            {
                int randomIndex = Random.Range(i, spawnPoints.Length);
                Transform temp = spawnPoints[i];
                spawnPoints[i] = spawnPoints[randomIndex];
                spawnPoints[randomIndex] = temp;
            }

            for (int i = 0; i < spawnPoints.Length; i++)
            {
                if (currentEnemiesSpawned[i] >= maxEnemiesPerSpawn)
                    continue;

                bool spawnPointEmpty = true;

                foreach (Transform child in spawnPoints[i])
                {
                    if (child.CompareTag("Enemy"))
                    {
                        spawnPointEmpty = false;
                        break;
                    }
                }

                if (spawnPointEmpty)
                {
                    allSpawnPointsOccupied = false; // At least one spawn point is available

                    int randomEnemy = Random.Range(0, enemies.Length);

                    GameObject newEnemy = Instantiate(enemies[randomEnemy], spawnPoints[i].position, Quaternion.identity, spawnPoints[i]);

                    if (spawnPoints[i].position.y < 0)
                    {
                        spawnPoints[i].Rotate(180, 0, 0);
                        spawnPoints[i].position = new Vector3(spawnPoints[i].position.x, spawnPoints[i].position.y, -spawnPoints[i].position.z);
                        Debug.Log("Rotated");
                    }

                    EnemyHealth enemyHealth = newEnemy.GetComponent<EnemyHealth>();

                    if (enemyHealth != null)
                    {
                        enemyHealth.SetSpawnPoint(spawnPoints[i]);
                    }

                    currentEnemiesSpawned[i]++;

                    currentSpawnInterval = Mathf.Max(currentSpawnInterval - spawnIntervalDecrease, minSpawnInterval);

                    float randomWait = Random.Range(minSpawnDelay, maxSpawnDelay);
                    yield return new WaitForSeconds(randomWait);
                }
            }

            // If all spawn points are occupied, wait for a brief duration before checking again
            if (allSpawnPointsOccupied)
            {
                float waitDuration = Random.Range(2f, 25f);
                yield return new WaitForSeconds(waitDuration);
            }
        }
    }


    public void EnemyKilled(Transform spawnPoint)
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            if (spawnPoints[i] == spawnPoint)
            {
                currentEnemiesSpawned[i]--;
                break;
            }
        }
    }
}
