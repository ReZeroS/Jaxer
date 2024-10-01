using UnityEngine.InputSystem;

public class InputState
{
    private InputAction inputAction;

    public bool justPressed { get; private set; }
    public bool beingHeld { get; private set; }
    public bool justReleased { get; private set; }

    public InputState(InputAction action)
    {
        inputAction = action;
    }

    public void Update()
    {
        justPressed = inputAction.WasPerformedThisFrame();
        beingHeld = inputAction.IsPressed();
        justReleased = inputAction.WasReleasedThisFrame();
    }
}
