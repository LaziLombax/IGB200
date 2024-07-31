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
}
