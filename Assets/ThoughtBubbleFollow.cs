using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThoughtBubbleFollow : MonoBehaviour
{
    public Transform player;   // Assign the player in the Inspector
    public RectTransform thoughtBubble; // Assign the thought bubble UI element (e.g., an Image or Text) in the Inspector
    public Camera mainCamera;  // The camera rendering the canvas
    public Vector3 offset = new Vector3(0, 2f, 0); // Offset for bubble height
    private RectTransform canvasRectTransform;
    private void Start()
    {
        canvasRectTransform = GameObject.Find("Canvas").GetComponent<RectTransform>();
    }

    void FixedUpdate()
    {
        // Convert the player's position (with optional offset) to screen space
        Vector3 worldPosition = player.position + offset;
        Vector3 screenPosition = mainCamera.WorldToScreenPoint(worldPosition);

        // Convert screen position to Canvas (UI) space
        Vector2 viewportPosition = new Vector2(screenPosition.x / Screen.width, screenPosition.y / Screen.height);
        Vector2 canvasPosition = new Vector2(
            (viewportPosition.x * canvasRectTransform.sizeDelta.x) - (canvasRectTransform.sizeDelta.x * 0.5f),
            (viewportPosition.y * canvasRectTransform.sizeDelta.y) - (canvasRectTransform.sizeDelta.y * 0.5f)
        );

        // Place the indicator on the right side of the canvas
        //float indicatorX = canvasRectTransform.sizeDelta.x / 2 - 50; // X coordinate near the right edge
        //canvasPosition.x = indicatorX; // Fix the X position

        // Set the position of the indicator in canvas space
        transform.GetComponent<RectTransform>().anchoredPosition = canvasPosition;
        // Check if the player is in front of the camera (z > 0)
    }
}
