using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class BoatSpawner : MonoBehaviour
{
    public GameObject Boat;

    public float SpawnInterval;
    public float minSpawnTimer = 10;
    public float maxSpawnTimer = 20;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnTimer(SpawnInterval));
    }

    IEnumerator SpawnTimer(float WaitTime){
        yield return new WaitForSeconds(WaitTime);
        SpawnBoat();
        float spawnTime = Random.Range(minSpawnTimer, maxSpawnTimer);
        StartCoroutine(SpawnTimer(spawnTime));
    }

    public void SpawnBoat(){
        if (!GameHandler.Instance.timerOn) return;
        GameObject newBoat = Instantiate(Boat, new Vector3(gameObject.transform.position.x, 8.0f, gameObject.transform.position.z), gameObject.transform.rotation);
        newBoat.GetComponent<Boat>().Player = gameObject.transform.parent.gameObject;
    }
}
