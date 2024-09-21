using System;
using UnityEngine;
using UnityEngine.InputSystem;

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
    bool speedInput;

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
        //
        playerControls.Player.SpeedUp.performed += SpeedingUp;
        playerControls.Player.SpeedUp.canceled += StopSpeedingUp;
    }

    private void StopSpeedingUp(InputAction.CallbackContext context)
    {
        speedInput = false;
    }

    private void SpeedingUp(InputAction.CallbackContext context)
    {
        speedInput = true;
    }

    public bool GetSpeedInput()
    {
        return speedInput;
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
    public bool BeachMoveDown()
    {
        if (playerControls.Player.BeachDown.triggered)
        {
            return true;
        }
        return false;
    }
    public bool LMBDialogue()
    {
        if (playerControls.Player.LMB.triggered)
        {
            return true;
        }
        return false;
    }
}
