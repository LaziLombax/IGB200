using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerController : Entity
{
    #region Player Variables

    [Space(10)]
    [Header("Player Movement Variables")]
    public float moveSpeed = 20f;
    public Rigidbody rb;
    public bool gotHit;
    public bool isRed;
    public float flashCount = 5f;
    public Renderer model;
    public MovementState state;
    public enum MovementState
    {
        moving,
        damaged
    };
    [HideInInspector] public InputHandler inputHandler;

    //etc.

    private static PlayerController _instance;
    public static PlayerController Instance
    {
        get
        {
            _instance = FindObjectOfType<PlayerController>();
            return _instance;
        }
    }

    #endregion
    private void Awake()
    {
        gameHandler = GameHandler.Instance;
        inputHandler = InputHandler.Instance;
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        PlayerInput();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy" && !gotHit)
        {
            gameHandler.uiHandler.factToDisplay = gameHandler.currentLevelData.HazardFact(gameHandler.stageName,other.GetComponent<Info>().hazardName);
            TakeDamage(1);
            gotHit = true;
            StartCoroutine(Flash());
        }
    }
    private IEnumerator Flash()
    {
        if (model != null && gotHit)
        {
            Color originalColour = model.material.GetColor("_Colour");
            float elapsedTime = 0f;

            while (elapsedTime < 2f)
            {
                if (!isRed)
                {
                    isRed = true;
                    model.material.SetColor("_Colour", Color.red);
                }
                else
                {
                    isRed = false;
                    model.material.SetColor("_Colour", originalColour);
                }
                // Wait for the next flash
                yield return new WaitForSeconds(0.2f);
                elapsedTime += 0.2f;
            }

            // Ensure the renderer is enabled at the end
            model.material.SetColor("_Colour", originalColour);
            gotHit = false;
        }
    }
    public abstract void PlayerInput();
    public abstract void MovePlayer();

}
