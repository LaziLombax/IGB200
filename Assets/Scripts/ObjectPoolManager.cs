using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public List<ObjectPool> pools;  // A list of pools, each for a different prefab

    // Dictionary to quickly access the pool by prefab name or type
    private Dictionary<string, ObjectPool> poolDictionary;

    void Awake()
    {
        poolDictionary = new Dictionary<string, ObjectPool>();
        foreach (ObjectPool pool in pools)
        {
            // Assume each ObjectPool has a unique identifier for its prefab type
            poolDictionary.Add(pool.objectPrefab.name, pool);
        }
    }

    // Method to get the right pool based on the prefab type
    public ObjectPool GetPoolByPrefab(GameObject prefab)
    {
        if (poolDictionary.ContainsKey(prefab.name))
        {
            return poolDictionary[prefab.name];
        }
        else
        {
            Debug.LogWarning("No pool found for prefab: " + prefab.name);
            return null;
        }
    }
}