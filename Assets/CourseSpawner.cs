using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CourseSpawner : MonoBehaviour
{
    private GameHandler gameHandler;
    // Start is called before the first frame update
    void Start()
    {
        gameHandler = GameHandler.Instance;

        for (int i = 0; i < gameHandler.numOfRows; i++)
        {
            SpawnCourse();
        }
    }

    private void SpawnCourse()
    {
        Instantiate(gameHandler.rowList[Random.Range(0, gameHandler.rowList.Count)], transform.position, Quaternion.identity);

        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + gameHandler.rowHeight);
    }
}
