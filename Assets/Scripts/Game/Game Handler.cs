using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    private static GameHandler _instance;
    public static GameHandler Instance
    {
        get
        {
            _instance = FindObjectOfType<GameHandler>();
            return _instance;
        }
    }


    #region Game Variables
    [Header("Beach")]
    public int numOfRows;
    public float rowHeight;
    public float rowWidth;
    public List<GameObject> rowList = new List<GameObject>();
    #endregion
}
