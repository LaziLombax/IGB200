using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public ObjectPoolManager poolManager;
    public ObjectPool objectPool;
    public GameObject objectToSpawn;
    public float objectSpeed;
    public float spawnTimer;
    public float spawnTime;
    public Vector3 objectDir;
    public bool Obstacles;
    public bool isUnderWater;

    // List to hold available obstacle spots
    public List<int> ObstacleSpots = new List<int>();

    private void Start()
    {
        poolManager = GameHandler.Instance.GetComponent<ObjectPoolManager>();
        objectPool = poolManager.GetPoolByPrefab(objectToSpawn);
        if (Obstacles)
        {
            for (int i = 0; i < Random.Range(1, 4); i++)
            {
                int index = Random.Range(0, ObstacleSpots.Count);
                // Use object pooling instead of Instantiate
                GameObject obstacle = objectPool.GetFromPool(new Vector3(ObstacleSpots[index], transform.position.y, transform.position.z), transform.rotation);
                ObstacleSpots.RemoveAt(index);
            }
        }
        spawnTime = Random.Range(0, 2);
        if (!Obstacles)
        {
            InvokeRepeating(nameof(CheckAndSpawnHazard), spawnTimer, spawnTimer);
        }
    }

    // Method to check game conditions and spawn hazards
    private void CheckAndSpawnHazard()
    {
        // Check if the game has ended or the timer is off
        if (GameHandler.Instance.gameEnded || !GameHandler.Instance.timerOn)
        {
            CancelInvoke(nameof(CheckAndSpawnHazard)); // Stop spawning
            return;
        }

        // Spawn the hazard if the game is still running
        SpawnHazard();
    }

    // Hazard spawning logic
    private void SpawnHazard()
    {
        float yOffset = 0;
        if (isUnderWater) yOffset = Random.Range(-3, 3);

        // Use object pooling instead of Instantiate
        GameObject hazard = objectPool.GetFromPool(new Vector3(transform.position.x, transform.position.y + yOffset, transform.position.z), transform.rotation);
        if (hazard != null)
        {
            hazard.GetComponent<Rigidbody>().AddForce(transform.forward * objectSpeed, ForceMode.VelocityChange);

            // Return the object to the pool after some time (instead of Destroy)
            StartCoroutine(ReturnToPoolAfterDelay(hazard, 5f));
        }
    }

    // Coroutine to return the object to the pool after a delay
    private IEnumerator ReturnToPoolAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        objectPool.ReturnToPool(obj);
    }
}