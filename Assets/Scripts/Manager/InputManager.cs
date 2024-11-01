using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    public enum InputMapType
    {
        GamePlay,
        UI
    }
    
    private PlayerInput playerInput;
    private InputMapType currentInputMap = InputMapType.GamePlay;

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
    // 给ui用来退出回到游戏的
    private InputAction exitMenuAction;

    #region right part

    private InputAction southAction;
    private InputAction eastAction;
    private InputAction westAction;
    private InputAction northAction;
    private InputAction rightShoulderAction;
    private InputAction rightTriggerAction;

    private InputAction rightStick;

    #endregion

    
    public Vector2 moveInput => moveAction.ReadValue<Vector2>();
    public bool isMove => moveInput.x != 0;

    public Vector2 rightStickInput => rightStick.ReadValue<Vector2>();
    
    #region Input States
    public InputState leftShoulder { get; private set; }
    public InputState rightShoulder { get; private set; }
    public InputState leftTrigger { get; private set; }
    public InputState rightTrigger { get; private set; }
    public InputState padLeft { get; private set; }
    public InputState padRight { get; private set; }
    public InputState padUp { get; private set; }
    public InputState padDown { get; private set; }
    public InputState view { get; private set; }
    public InputState menu { get; private set; }
    public InputState exitMenu { get; private set; }
    public InputState south { get; private set; }
    public InputState east { get; private set; }
    public InputState west { get; private set; }
    public InputState north { get; private set; }
    #endregion

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        playerInput = GetComponent<PlayerInput>();
        SetupInputActions();
        InitializeInputStates();
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
        exitMenuAction = playerInput.actions["ExitMenu"];


        rightStick = playerInput.actions["RightStick"];

        southAction = playerInput.actions["South"]; // A
        eastAction = playerInput.actions["East"]; // B
        westAction = playerInput.actions["West"]; // X
        northAction = playerInput.actions["North"]; // Y

        rightShoulderAction = playerInput.actions["RightShoulder"]; // R1
        leftShoulderAction = playerInput.actions["LeftShoulder"]; // L1
        leftTriggerAction = playerInput.actions["LeftTrigger"]; // L2
        rightTriggerAction = playerInput.actions["RightTrigger"]; // R2
        
        
    }
    
    private void InitializeInputStates()
    {
        leftShoulder = new InputState(leftShoulderAction);
        rightShoulder = new InputState(rightShoulderAction);
        leftTrigger = new InputState(leftTriggerAction);
        rightTrigger = new InputState(rightTriggerAction);
        padLeft = new InputState(padLeftAction);
        padRight = new InputState(padRightAction);
        padUp = new InputState(padUpAction);
        padDown = new InputState(padDownAction);
        view = new InputState(viewAction);
        menu = new InputState(menuAction);
        exitMenu = new InputState(exitMenuAction);
        south = new InputState(southAction);
        east = new InputState(eastAction);
        west = new InputState(westAction);
        north = new InputState(northAction);
    }
    
    
    public void SwitchActionMap(InputMapType mapType)
    {
        if (currentInputMap == mapType) return;

        currentInputMap = mapType;
        playerInput.SwitchCurrentActionMap(mapType.ToString());
        Debug.Log($"Switched to {mapType} ActionMap");
    }
    

    public bool MatchHotKey(string myHotKey)
    {
        return myHotKey switch
        {
            "X" => west.justPressed,
            "Y" => north.justPressed,
            "A" => south.justPressed,
            "B" => east.justPressed,
            _ => false
        };
    }
}