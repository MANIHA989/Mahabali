using UnityEngine;
using System.Collections;

public class EnemyManager : MonoBehaviour
{
    public GameObject enemyPrefab; // Enemy prefab to spawn
    public float respawnDelay = 2f; // Delay before spawning a new enemy

    private GameObject currentEnemy; // Reference to the current active enemy

    private void Start()
    {
        SpawnEnemy(); // Spawn the first enemy
    }

    public void SpawnEnemy()
    {
        // Spawn the enemy at the current position of the manager
        Vector3 spawnPosition = transform.position;
        currentEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }

    public void EnemyKilled()
    {
        // Destroy the current enemy and spawn a new one
        if (currentEnemy != null)
        {
            Destroy(currentEnemy);
        }
        StartCoroutine(RespawnEnemyWithDelay());
    }

    private IEnumerator RespawnEnemyWithDelay()
    {
        yield return new WaitForSeconds(respawnDelay); // Wait for the specified delay
        SpawnEnemy(); // Spawn a new enemy
    }
}
