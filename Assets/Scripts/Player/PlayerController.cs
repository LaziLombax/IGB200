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
        if (other.gameObject.tag == "Enemy")
        {
            TakeDamage(1);
            Destroy(other.gameObject);
        }
    }

    public abstract void PlayerInput();
    public abstract void MovePlayer();

}
