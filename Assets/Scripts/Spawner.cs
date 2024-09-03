using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

public class Spawner : MonoBehaviour
{
    public GameObject objectToSpawn;
    public float objectSpeed;
    public float spawnTimer;
    public float spawnTime;
    public Vector3 objectDir;
    public bool Obstacles;
    public bool isUnderWater;

    // Update is called once per frame
    public List<int> ObstacleSpots = new List<int>();
    void Awake(){
        if (Obstacles){
            for (int i = 0; i < Random.Range(1,4); i++){
                int index = Random.Range(0,ObstacleSpots.Count);
                Instantiate(objectToSpawn, new Vector3(ObstacleSpots[index], objectToSpawn.transform.position.y, transform.position.z), transform.rotation);
                ObstacleSpots.RemoveAt(index);
            }
        }
    }
    void Update()
    {
        if(GameHandler.Instance.gameEnded) return;
        if (!Obstacles){
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
    }
    private void SpawnHazzard()
    {
        float yOffset = 0;
        if (isUnderWater) yOffset = Random.Range(-3, 3);
        GameObject newObject = Instantiate(objectToSpawn, new Vector3(transform.position.x, transform.position.y + yOffset, transform.position.z), transform.rotation);
        newObject.GetComponent<Rigidbody>().AddForce(transform.forward * objectSpeed, ForceMode.VelocityChange);
        Destroy(newObject,10f);
    }
}
