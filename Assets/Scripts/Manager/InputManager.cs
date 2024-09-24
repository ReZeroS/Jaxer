using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;
   

    private PlayerInput playerInput;
    // private InputAction jumpAction;
    private InputAction moveAction;

    public bool jumpJustPressed;
    public bool jumpJustBeingHeld;
    public bool jumpReleased;
    
    public Vector2 moveInput;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        playerInput = GetComponent<PlayerInput>();
        SetupInputActions();
    }

    private void SetupInputActions()
    {
        moveAction = playerInput.actions["Movement"];
    }

    private void Update()
    {
        UpdateInputs();
    }

    private void UpdateInputs()
    {
        moveInput = moveAction.ReadValue<Vector2>();
    }

    // private void LateUpdate()
    // {
    //     // 重置一次性状态
    //     isJumpKeyDown = false;
    //     isJumpKeyUp = false;
    // }
    //

  
}

