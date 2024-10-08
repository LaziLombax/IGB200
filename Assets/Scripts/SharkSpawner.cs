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
    public float spawnOffset = 2f; // Vertical offset for pattern
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
        if (Time.time >= nextSpawnTime && GameHandler.Instance.timerOn)
        {
            SpawnObjectsInPattern();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    private void SpawnObjectsInPattern()
    {
        int numberOfSets = 1;
        int objectsPerSet = 3;
        float setInterval = 1.0f;
        float verticalOffset = spawnOffset * 3;
        float spaceing = 2f; 
        int randomPattern = Random.Range(0, 3);
        switch (randomPattern)
        {
            case 0:
                // Set the number of sets and objects per set
                numberOfSets = 1;
                objectsPerSet = 4; // Adjust this range as necessary
                setInterval = 2.0f; // Time interval between each set
                verticalOffset = spawnOffset * 3; // Adjust the spacing between sets
                spaceing = 3f;
                break;
            case 1:
                // Set the number of sets and objects per set
                numberOfSets = 3;
                objectsPerSet = 3; // Adjust this range as necessary
                setInterval = 1.0f; // Time interval between each set
                verticalOffset = spawnOffset * 1f; // Adjust the spacing between sets
                spaceing = 1f;
                break;
            case 2:
                // Set the number of sets and objects per set
                numberOfSets = 6;
                objectsPerSet = 1; // Adjust this range as necessary
                setInterval = 0.2f; // Time interval between each set
                verticalOffset = spawnOffset * 1f; // Adjust the spacing between sets
                spaceing = 1f;
                break;
        }
        StartCoroutine(SpawnSharkSets(numberOfSets, objectsPerSet, setInterval, verticalOffset, spaceing));
    }
    private IEnumerator SpawnSharkSets(int numberOfSets, int objectsPerSet, float setInterval, float verticalOffset, float spaceOffset)
    {
        for (int set = 0; set < numberOfSets; set++)
        {
            for (int i = 0; i < objectsPerSet; i++)
            {
                if (!GameHandler.Instance.timerOn) yield return null;
                Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y + (i * spaceOffset) + (set * verticalOffset), transform.position.z);

                GameObject spawnedObject = objectPool.GetFromPool(spawnPosition, transform.rotation);
                if (spawnedObject != null)
                {
                    Rigidbody rb = spawnedObject.GetComponent<Rigidbody>();
                    rb.velocity = Vector3.zero;
                    if (rb != null)
                    {
                        rb.AddForce(transform.forward * 7f, ForceMode.VelocityChange);
                    }

                    SpawnIndicator(spawnPosition);
                    StartCoroutine(ReturnToPoolAfterDelay(spawnedObject, 5f));
                }
            }

            // Wait for the interval before spawning the next set
            yield return new WaitForSeconds(setInterval);
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
            if (!GameHandler.Instance.timerOn) yield return null;
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