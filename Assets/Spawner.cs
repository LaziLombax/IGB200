using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject objectToSpawn;
    public float objectSpeed;
    public float spawnTimer;
    public float spawnTime;

    // Update is called once per frame
    void Update()
    {
        if (spawnTime < 0)
        {
            spawnTime = spawnTimer;
            SpawnHazzard();
        }
        else
        {
            spawnTime -= Time.deltaTime;
        }
    }
    private void SpawnHazzard()
    {
        GameObject newObject = Instantiate(objectToSpawn, transform.position, transform.rotation);
        newObject.GetComponent<Rigidbody>().velocity = Vector3.right * objectSpeed;
        Destroy(newObject,10f);
    }
}
