using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDecor : MonoBehaviour
{
    public GameObject[] decorations; 
    public float startPositionZ; // The X position where the prefab will reappear (the front of the line)
    public float endPositionZ;   // The X position where the prefab will be considered "at the back"
    public int numberOfShifts;

    private void Start()
    {
            numberOfShifts = GameHandler.Instance.levelSize -= decorations.Length;
    }
    // This will be called when the player hits a trigger collider
    void Update()
    {
        if (Camera.main.transform.position.z > decorations[0].transform.position.z + endPositionZ) // Make sure the collider belongs to the player
        {
            if(numberOfShifts == 0) return;
                // Move the first decoration to the front
                Vector3 newPos = decorations[2].transform.position;
                newPos.z += startPositionZ;
                decorations[0].transform.position = newPos;

                // Shift the decorations in the array (move first element to the end)
                ShiftDecorationsArray();
            numberOfShifts--;
        }
    }

    // Shift the first decoration in the array to the back and move others forward
    void ShiftDecorationsArray()
    {
        GameObject firstDecoration = decorations[0];

        // Shift all the elements one index forward
        for (int i = 0; i < decorations.Length - 1; i++)
        {
            decorations[i] = decorations[i + 1];
        }

        // Put the first decoration at the last position
        decorations[decorations.Length - 1] = firstDecoration;
    }
}
