using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    #region UI Variables
    [Header("UI Data")]
    public GameHandler gameHandler;
    [Header("Start Menu")]
    public int exampleInt;

    [Space(10)]
    [Header("etc.")]
    public float exampleFloat;

    private void Awake()
    {
        gameHandler = GameHandler.Instance;
    }
    #endregion
    public void ButtonExample()
    {
        //functionailty
    }
}
