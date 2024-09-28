using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;
   

    private PlayerInput playerInput;
    
    #region left part
    private InputAction leftShoulderAction;
    private InputAction leftTriggerAction;
    private InputAction moveAction;
    private InputAction padLeftAction;
    private InputAction padRightAction;
    private InputAction padUpAction;
    private InputAction padDownAction;
    #endregion
    
    private InputAction viewAction;
    private InputAction menuAction;
    
    #region right part

    private InputAction jumpAction;
    private InputAction dashAction;
    private InputAction lightAttackAction;
    private InputAction heavyAttackAction;
    private InputAction rightShoulderAction;
    private InputAction rightTriggerAction;
    
    private InputAction rightStick;
    
    #endregion

    
    // left stick
    public Vector2 moveInput;
    
    public bool leftShoulderPressed { get; private set; }
    public bool leftShoulderJustBeingHeld { get; private set; }
    public bool leftShoulderReleased { get; private set; }
    
    public bool leftTriggerPressed { get; private set; }
    public bool leftTriggerJustBeingHeld { get; private set; }
    public bool leftTriggerReleased { get; private set; }
    
    public bool padLeftPressed { get; private set; }
    public bool padLeftJustBeingHeld { get; private set; }
    public bool padLeftReleased { get; private set; }
    
    public bool padRightPressed { get; private set; }
    public bool padRightJustBeingHeld { get; private set; }
    public bool padRightReleased { get; private set; }
    
    public bool padUpPressed { get; private set; }
    public bool padUpJustBeingHeld { get; private set; }
    public bool padUpReleased { get; private set; }
    
    public bool padDownPressed { get; private set; }
    public bool padDownJustBeingHeld { get; private set; }
    public bool padDownReleased { get; private set; }
    
    
    public bool viewJustPressed { get; private set; }
    public bool viewJustBeingHeld { get; private set; }
    public bool viewJustReleased { get; private set; }
    
    public bool menuJustPressed { get; private set; }
    public bool menuJustBeingHeld { get; private set; }
    public bool menuJustReleased { get; private set; } 
    
    
    
    // right part
    public Vector2 rightStickInput;
    
    
    public bool jumpJustPressed { get; private set; }
    public bool jumpJustBeingHeld { get; private set; }
    public bool jumpReleased { get; private set; }
    
    public bool dashJustPressed { get; private set; }
    public bool dashJustBeingHeld { get; private set; }
    public bool dashReleased { get; private set; }
    
    public bool lightAttackJustPressed { get; private set; }
    public bool lightAttackJustBeingHeld { get; private set; }
    public bool lightAttackReleased { get; private set; }
    
    public bool heavyAttackJustPressed { get; private set; }
    public bool heavyAttackJustBeingHeld { get; private set; }
    public bool heavyAttackReleased { get; private set; }
    
    public bool rightShoulderPressed { get; private set; }
    public bool rightShoulderJustBeingHeld { get; private set; }
    public bool rightShoulderReleased { get; private set; }
    
    public bool rightTriggerJustPressed { get; private set; }
    public bool rightTriggerBeingHeld { get; private set; }
    public bool rightTriggerJustReleased { get; private set; }
    
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
        leftShoulderAction = playerInput.actions["LeftShoulder"];
        leftTriggerAction = playerInput.actions["LeftTrigger"];

        moveAction = playerInput.actions["Movement"];

        padLeftAction = playerInput.actions["PadLeft"];
        padRightAction = playerInput.actions["PadRight"];
        padUpAction = playerInput.actions["PadUp"];
        padDownAction = playerInput.actions["PadDown"];
        
        
        viewAction = playerInput.actions["View"];
        menuAction = playerInput.actions["Menu"];

        
        rightStick = playerInput.actions["RightStick"];
        
        jumpAction = playerInput.actions["Jump"]; // A
        dashAction = playerInput.actions["Dash"]; // B
        lightAttackAction = playerInput.actions["LightAttack"]; // X
        heavyAttackAction = playerInput.actions["HeavyAttack"]; // Y
        
        rightShoulderAction = playerInput.actions["RightShoulder"]; // R1
        leftShoulderAction = playerInput.actions["LeftShoulder"]; // L1
        leftTriggerAction = playerInput.actions["LeftTrigger"]; // L2
        rightTriggerAction = playerInput.actions["RightTrigger"]; // R2
    }

    private void Update()
    {
        UpdateInputs();
    }

    private void UpdateInputs()
    {
        leftShoulderPressed = leftShoulderAction.WasPerformedThisFrame();
        leftShoulderJustBeingHeld = leftShoulderAction.IsPressed();
        leftShoulderReleased = leftShoulderAction.WasReleasedThisFrame();
        leftTriggerPressed = leftTriggerAction.WasPerformedThisFrame();
        leftTriggerJustBeingHeld = leftTriggerAction.IsPressed();
        leftTriggerReleased = leftTriggerAction.WasReleasedThisFrame();
        
        moveInput = moveAction.ReadValue<Vector2>();
        
        padLeftPressed = padLeftAction.WasPerformedThisFrame();
        padLeftJustBeingHeld = padLeftAction.IsPressed();
        padLeftReleased = padLeftAction.WasReleasedThisFrame();
        padRightPressed = padRightAction.WasPerformedThisFrame();
        padRightJustBeingHeld = padRightAction.IsPressed();
        padRightReleased = padRightAction.WasReleasedThisFrame();
        padUpPressed = padUpAction.WasPerformedThisFrame();
        padUpJustBeingHeld = padUpAction.IsPressed();
        padUpReleased = padUpAction.WasReleasedThisFrame();
        padDownPressed = padDownAction.WasPerformedThisFrame();
        padDownJustBeingHeld = padDownAction.IsPressed();
        padDownReleased = padDownAction.WasReleasedThisFrame();
        
        
        viewJustPressed = viewAction.WasPerformedThisFrame();
        viewJustBeingHeld = viewAction.IsPressed();
        viewJustReleased = viewAction.WasReleasedThisFrame();
        menuJustPressed = menuAction.WasPerformedThisFrame();
        menuJustBeingHeld = menuAction.IsPressed();
        menuJustReleased = menuAction.WasReleasedThisFrame();
        
        
        jumpJustPressed = jumpAction.WasPerformedThisFrame();
        jumpJustBeingHeld = jumpAction.IsPressed();
        jumpReleased = jumpAction.WasReleasedThisFrame();
        dashJustPressed = dashAction.WasPerformedThisFrame();
        dashJustBeingHeld = dashAction.IsPressed();
        dashReleased = dashAction.WasReleasedThisFrame();
        lightAttackJustPressed = lightAttackAction.WasPerformedThisFrame();
        lightAttackJustBeingHeld = lightAttackAction.IsPressed();
        lightAttackReleased = lightAttackAction.WasReleasedThisFrame();
        heavyAttackJustPressed = heavyAttackAction.WasPerformedThisFrame();
        heavyAttackJustBeingHeld = heavyAttackAction.IsPressed();
        heavyAttackReleased = heavyAttackAction.WasReleasedThisFrame();  
        
        rightStickInput = rightStick.ReadValue<Vector2>();
        
        rightShoulderPressed = rightShoulderAction.IsPressed();
        rightShoulderJustBeingHeld = rightShoulderAction.IsPressed();
        rightShoulderReleased = rightShoulderAction.WasReleasedThisFrame();
        rightTriggerJustPressed = rightTriggerAction.WasPressedThisFrame();
        rightTriggerBeingHeld = rightTriggerAction.IsPressed();
        rightTriggerJustReleased = rightTriggerAction.WasReleasedThisFrame();
         
    }
    
    
    

    // private void LateUpdate()
    // {
    //     // 重置一次性状态
    //     isJumpKeyDown = false;
    //     isJumpKeyUp = false;
    // }
    //

  
}

