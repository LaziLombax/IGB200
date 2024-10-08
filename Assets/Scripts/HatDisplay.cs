using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatDisplay : MonoBehaviour
{

    [HideInInspector] public InputHandler inputHandler;
    [HideInInspector] public GameHandler gameHandler;
    public List<GameObject> hatList = new List<GameObject>();
    private void Awake()
    {
        gameHandler = GameHandler.Instance;
    }

    private static HatDisplay _instance;
    public static HatDisplay Instance
    {
        get
        {
            _instance = FindObjectOfType<HatDisplay>();
            return _instance;
        }
    }

    private void Start()
    {
        SetHat();
    }
    public void SetHat()
    {
        List<string> currentHatKeyList = gameHandler.gameData.CheckWhichHat();
        foreach (var hat in hatList)
        {
            hat.SetActive(false);
            foreach (var key in currentHatKeyList)
            {
                if (hat.name == key)
                {
                    hat.SetActive(true);
                }
            }
        }
    }
}
