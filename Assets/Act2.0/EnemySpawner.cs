using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Reference to the enemy prefab
    public Transform[] spawnPoints; // Array of spawn points
    public float spawnDelay = 2f; // Delay between spawns
    private int enemyCount = 0; // Counter to track spawned enemies
    public int maxEnemies = 5; // Maximum number of enemies to spawn

    private void Start()
    {
        // Start the spawning process
        StartCoroutine(SpawnEnemies());
    }

    void Update()
    {
        if (enemyCount ==5)
        {
            SceneManager.LoadScene("PuzzleScene");
        }
    }

    IEnumerator SpawnEnemies()
    {
        while (enemyCount < maxEnemies) // Spawn only up to the maximum number of enemies
        {
            // Randomly pick a spawn point from the spawn points array
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

            // Instantiate the enemy prefab at the chosen spawn point
            Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);

            // Increment the enemy count
            enemyCount++;

            // Wait for the specified spawn delay before spawning the next enemy
            yield return new WaitForSeconds(spawnDelay);
        }

        Debug.Log("Enemy spawning stopped after reaching the limit.");
    }
}
