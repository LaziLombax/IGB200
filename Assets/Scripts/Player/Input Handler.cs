using UnityEngine;

public class InputHandler : MonoBehaviour
{
    private static InputHandler _instance;
    public static InputHandler Instance
    {
        get
        {
            _instance = FindObjectOfType<InputHandler>();
            return _instance;
        }
    }


    public PlayerControls playerControls;

    private void Awake()
    {
        if(_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else
        {
            _instance = this;
        }
        playerControls = new PlayerControls();
    }
    private void OnEnable()
    {
        playerControls.Enable();
    }
    private void OnDisable()
    {
        playerControls.Disable(); 
    }
    public Vector2 GetPlayerMovement()
    {
        return playerControls.Player.Movement.ReadValue<Vector2>();
    }

    public bool BeachMoveForward()
    {
        if (playerControls.Player.BeachForward.triggered)
        {
            return true;
        }
        return false;
    }
    public bool BeachMoveLeft()
    {
        if (playerControls.Player.BeachLeft.triggered)
        {
            return true;
        }
        return false;
    }
    public bool BeachMoveRight()
    {
        if (playerControls.Player.BeachRight.triggered)
        {
            return true;
        }
        return false;
    }
}
