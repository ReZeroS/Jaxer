using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;
   

    private PlayerInput playerInput;
    // private InputAction jumpAction;
    private InputAction moveAction;
    private InputAction submitAction;

    public bool jumpJustPressed { get; private set; }
    public bool jumpJustBeingHeld { get; private set; }
    public bool jumpReleased { get; private set; }
    
    public bool submitJustPressed { get; private set; }
    public bool submitJustBeingHeld { get; private set; }
    public bool submitReleased { get; private set; }
    
    
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
        submitAction = playerInput.actions["Submit"];
    }

    private void Update()
    {
        UpdateInputs();
    }

    private void UpdateInputs()
    {
        moveInput = moveAction.ReadValue<Vector2>();
        submitJustPressed = submitAction.WasPerformedThisFrame();
        submitJustBeingHeld = submitAction.IsPressed();
        submitReleased = submitAction.WasReleasedThisFrame();
    }

    // private void LateUpdate()
    // {
    //     // 重置一次性状态
    //     isJumpKeyDown = false;
    //     isJumpKeyUp = false;
    // }
    //

  
}

