using System.Collections;
using UnityEngine;

public class BirdSpawner : MonoBehaviour
{
    public GameObject objectToSpawn;
    public float minSpawnTimer;
    public float maxSpawnTimer;
    private float spawnTime;
    public float AmountToSpawn;
    public GameObject player;  // Reference to the player's transform
    public float activationDistance = 20f;  // Distance at which the spawner becomes active

    void Start()
    {
        StartCoroutine(SpawnTimer(2));
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void SpawnBird()
    {
        if (!GameHandler.Instance.timerOn) return;

        float xspawn = 0;
        float random = Random.Range(0, 1);
        if (random == 0)
        {
            xspawn = 18;
        }
        else if (random == 1)
        {
            xspawn = -18;
        }
        Vector3 offsetpos = new Vector3(xspawn, 10, player.transform.position.z + (4 * Random.Range(-2, 5)));
        GameObject newObject = Instantiate(objectToSpawn, offsetpos, objectToSpawn.transform.rotation);
    }

    IEnumerator SpawnTimer(float WaitTime)
    {
        yield return new WaitForSeconds(WaitTime);

        // Check the distance between player and spawner
        float distanceToPlayer = Vector3.Distance(PlayerController.Instance.transform.position, transform.position);

        if (distanceToPlayer <= activationDistance)  // Only spawn if the player is within the set distance
        {
            for (float i = 0; i <= AmountToSpawn; i++)
            {
                SpawnBird();
                yield return new WaitForSeconds(Random.Range(2, 4));
            }
        }

        spawnTime = Random.Range(minSpawnTimer, maxSpawnTimer);
        StartCoroutine(SpawnTimer(spawnTime));
    }
}

