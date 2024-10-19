using UnityEngine.InputSystem;

public class InputState
{
    private InputAction inputAction;

    public bool justPressed => inputAction.WasPerformedThisFrame();
    public bool beingHeld => inputAction.IsPressed();
    public bool justReleased => inputAction.WasReleasedThisFrame();

    public InputState(InputAction action)
    {
        inputAction = action;
    }
    
}
