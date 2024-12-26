using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class EnemyManager : MonoBehaviour
{
    public GameObject enemyPrefab; // Enemy prefab to spawn
    public float respawnDelay = 2f; // Delay before spawning a new enemy
    public float sceneLoadDelay = 2f; // Delay before loading the next scene

    private GameObject currentEnemy; // Reference to the current active enemy
    private int enemiesKilled = 0; // Counter for the number of enemies killed

    public GameObject Act2Screen;

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
        enemiesKilled++;

        if (enemiesKilled >= 3)
        {
            // Wait for a specified delay before loading the next scene
            StartCoroutine(LoadNextSceneWithDelay());
        }
        else
        {
            StartCoroutine(RespawnEnemyWithDelay());
        }
    }

    private IEnumerator LoadNextSceneWithDelay()
    {
        yield return new WaitForSeconds(2f);
        Act2Screen.SetActive(true);
        yield return new WaitForSeconds(3f);
        // Wait for the specified scene load delay
        SceneManager.LoadScene("Act2"); // Load the next scene
    }

    private IEnumerator RespawnEnemyWithDelay()
    {
        yield return new WaitForSeconds(respawnDelay); // Wait for the specified respawn delay
        SpawnEnemy(); // Spawn a new enemy
    }
}
