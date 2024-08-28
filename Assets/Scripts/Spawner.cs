using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject objectToSpawn;
    public float objectSpeed;
    public float spawnTimer;
    public float spawnTime;
    public Vector3 objectDir;

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
        GameObject newObject = Instantiate(objectToSpawn, new Vector3(transform.position.x, objectToSpawn.transform.position.y, transform.position.z), transform.rotation);
        newObject.GetComponent<Rigidbody>().AddForce(transform.forward * objectSpeed, ForceMode.VelocityChange);
        Destroy(newObject,10f);
    }
}
