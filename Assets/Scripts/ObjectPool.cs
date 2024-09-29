using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject objectPrefab;
    public int poolSize = 10;  // Pool size can be adjusted based on your needs
    private Queue<GameObject> poolQueue;

    void Awake()
    {
        poolQueue = new Queue<GameObject>();

        // Preload the pool with objects
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(objectPrefab);
            obj.SetActive(false);
            poolQueue.Enqueue(obj);
        }
    }

    // Get an object from the pool
    public GameObject GetFromPool(Vector3 position, Quaternion rotation)
    {
        if (poolQueue.Count > 0)
        {
            GameObject obj = poolQueue.Dequeue();
            obj.SetActive(true);
            obj.transform.position = position;
            obj.transform.rotation = rotation;
            return obj;
        }
        else
        {
            // Optionally instantiate more if needed
            Debug.LogWarning("No more objects in the pool! Consider increasing the pool size.");
            return null;
        }
    }

    // Return an object to the pool
    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
        poolQueue.Enqueue(obj);
    }
}