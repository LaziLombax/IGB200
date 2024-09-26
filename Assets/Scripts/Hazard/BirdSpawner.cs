using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BirdSpawner : MonoBehaviour
{
    public GameObject objectToSpawn;
    public float minSpawnTimer;
    public float maxSpawnTimer;
    private float spawnTime;
    public float AmountToSpawn;

    void Start(){
        StartCoroutine(SpawnTimer(2));
    }
    void SpawnBird()
    {
        if (!GameHandler.Instance.timerOn) return;
        float xspawn = 0;
        //float roty = 0;
        float random = Random.Range(0,1);
        if (random == 0)
        {
            xspawn = 18;
            //roty = -90;
        }
        else if (random == 1)
        {
            xspawn = -18;
            //roty = 90;
        }
        Vector3 offsetpos = new Vector3(xspawn,0,(4*Random.Range(-2,10)));
        GameObject newObject = Instantiate(objectToSpawn, transform.position + offsetpos, objectToSpawn.transform.rotation);
    }
    IEnumerator SpawnTimer(float WaitTime){
        yield return new WaitForSeconds(WaitTime);
        for (float i = 0; i <= AmountToSpawn; i++){
            SpawnBird();
            yield return new WaitForSeconds(Random.Range(2,4));
        }
        spawnTime = Random.Range(minSpawnTimer, maxSpawnTimer);
        StartCoroutine(SpawnTimer(spawnTime));
    }
}
