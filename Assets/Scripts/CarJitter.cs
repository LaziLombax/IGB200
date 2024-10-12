using UnityEngine;

public class CarJitter : MonoBehaviour
{
    // Amplitude of the jitter (how far the object moves up and down)
    public float amplitude = 0.05f; // You can adjust this value to fit your car's suspension feel

    // Frequency of the jitter (how fast the object moves up and down)
    public float frequency = 2.0f;

    // Starting Y position of the object
    private float startY;

    void Start()
    {
        // Save the initial Y position of the object
        startY = transform.localPosition.y;
    }

    void Update()
    {
        // Calculate the new Y position using a sine wave for smooth up and down motion
        float newY = startY + Mathf.Sin(Time.time * frequency) * amplitude;

        // Apply the new position to the object
        Vector3 newPosition = new Vector3(transform.localPosition.x, newY, transform.localPosition.z);
        transform.localPosition = newPosition;
    }
}