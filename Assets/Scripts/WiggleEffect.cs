using UnityEngine;

public class WiggleEffect : MonoBehaviour
{
    // Amplitude of the wiggle (how far the object rotates back and forth)
    public float amplitude = 15.0f; // You can adjust this value to increase or decrease the wiggle

    // Frequency of the wiggle (how fast the object rotates)
    public float frequency = 2.0f;

    // Axis around which the object will wiggle (default is Y-axis)
    public Vector3 wiggleAxis = new Vector3(0, 1, 0); // Y-axis by default

    // Starting rotation of the object
    private Quaternion startRotation;

    void Start()
    {
        // Save the initial rotation of the object
        startRotation = transform.localRotation;
    }

    void Update()
    {
        // Calculate the wiggle amount using a sine wave for smooth back and forth rotation
        float wiggleAmount = Mathf.Sin(Time.time * frequency) * amplitude;

        // Apply the wiggle to the object's rotation
        Quaternion wiggleRotation = Quaternion.AngleAxis(wiggleAmount, wiggleAxis);
        transform.localRotation = startRotation * wiggleRotation;
    }
}
