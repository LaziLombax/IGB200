using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // For UI components

public class SharkSpawner : MonoBehaviour
{
    public ObjectPoolManager poolManager; // Reference to the pool manager
    public ObjectPool objectPool; // Reference to the specific object pool
    public GameObject objectToSpawn; // The object prefab to spawn
    public float spawnInterval = 2f; // Time between each spawn
    public float spawnHeight = 5f; // Height at which to spawn objects
    public float spawnOffset = 2f; // Horizontal offset for pattern
    public int numberOfObjects = 5; // Number of objects to spawn
    public bool isUnderWater; // Flag for underwater logic

    // New variables for flashing indicator
    public GameObject spawnIndicatorPrefab; // Reference to the UI Indicator prefab
    public float flashDuration = 0.5f; // How long the indicator will flash

    private float nextSpawnTime;
    private RectTransform canvasRectTransform;

    private void Start()
    {
        poolManager = GameHandler.Instance.GetComponent<ObjectPoolManager>();
        objectPool = poolManager.GetPoolByPrefab(objectToSpawn);
        nextSpawnTime = Time.time + spawnInterval;

        // Get the RectTransform of the main canvas
        canvasRectTransform = GameObject.Find("Canvas").GetComponent<RectTransform>();
    }

    private void Update()
    {
        // Check if it's time to spawn objects
        if (Time.time >= nextSpawnTime)
        {
            SpawnObjectsInPattern();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    private void SpawnObjectsInPattern()
    {
        numberOfObjects = Random.Range(3, 5);
        for (int i = 0; i < numberOfObjects; i++)
        {
            float offset = 0f;
            if(numberOfObjects==3) offset = spawnOffset;
            Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y + (i * spawnOffset) + offset, transform.position.z);
            // Use object pooling to get a pooled object
            GameObject spawnedObject = objectPool.GetFromPool(spawnPosition, transform.rotation);
            if (spawnedObject != null)
            {
                // Optionally add force or modify the spawned object
                Rigidbody rb = spawnedObject.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddForce(transform.forward * 7f, ForceMode.VelocityChange); // Adjust the force as needed
                }

                // Spawn a new indicator for each shark
                SpawnIndicator(spawnPosition);

                // Return the object to the pool after some time
                StartCoroutine(ReturnToPoolAfterDelay(spawnedObject, 5f));
            }
        }
    }

    private void SpawnIndicator(Vector3 sharkPosition)
    {
        // Instantiate a new indicator on the canvas
        GameObject indicator = Instantiate(spawnIndicatorPrefab, canvasRectTransform);

        // Convert the shark's world position to screen position
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(sharkPosition);

        // Check if the shark is within the screen bounds
        if (screenPosition.z > 0) // Ensures the shark is in front of the camera
        {
            // Convert screen position to Canvas (UI) space
            Vector2 viewportPosition = new Vector2(screenPosition.x / Screen.width, screenPosition.y / Screen.height);
            Vector2 canvasPosition = new Vector2(
                (viewportPosition.x * canvasRectTransform.sizeDelta.x) - (canvasRectTransform.sizeDelta.x * 0.5f),
                (viewportPosition.y * canvasRectTransform.sizeDelta.y) - (canvasRectTransform.sizeDelta.y * 0.5f)
            );

            // Place the indicator on the right side of the canvas
            float indicatorX = canvasRectTransform.sizeDelta.x / 2 - 50; // X coordinate near the right edge
            canvasPosition.x = indicatorX; // Fix the X position

            // Set the position of the indicator in canvas space
            indicator.GetComponent<RectTransform>().anchoredPosition = canvasPosition;

            // Ensure the indicator is visible
            indicator.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f); // Fully opaque

            // Start the flashing coroutine for this indicator
            StartCoroutine(FlashIndicator(indicator));
        }
    }

    private IEnumerator FlashIndicator(GameObject indicator)
    {
        // Flash the indicator for the specified duration
        float duration = flashDuration;
        float startTime = Time.time;

        while (Time.time < startTime + duration)
        {
            // Calculate the alpha value based on time
            float alpha = Mathf.PingPong(Time.time * 4f, 1f); // Adjust the speed of the flashing
            indicator.GetComponent<Image>().color = new Color(1f, 1f, 1f, alpha); // Update alpha
            yield return null; // Wait for the next frame
        }

        // Destroy the indicator after flashing
        Destroy(indicator);
    }

    private IEnumerator ReturnToPoolAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        objectPool.ReturnToPool(obj); // Return the object to the pool
    }
}