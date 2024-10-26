using System;
using UnityEngine;

public class EventHandleManager 
{
    
    public static event Action<string, Vector3> TransitionEvent;

    public static void CallTransitionEvent(String sceneName, Vector3 pos)
    {
        TransitionEvent?.Invoke(sceneName, pos);
    }
        
    public static event Action BeforeSceneUnloadEvent;

    public static void CallBeforeSceneUnLoadEvent()
    {
        BeforeSceneUnloadEvent?.Invoke();
    }
        

    public static event Action AfterSceneLoadedEvent;

    public static void CallAfterSceneLoadedEvent()
    {
        AfterSceneLoadedEvent?.Invoke();
    }


    public static event Action<Vector3> movePositionEvent;
    public static void CallMovePosition(Vector3 targetPosition)
    {
        movePositionEvent?.Invoke(targetPosition);
    }

  

}
