using System;
using UnityEngine;

public class EventManager 
{

    #region Entity

    public static event Action OnFlipped;
    
    public static void CallOnFlippedEvent()
    {
        OnFlipped?.Invoke();
    }

    #endregion

    #region Scene

    public static event Action<string, Vector3> TransitionEvent;


    #endregion

}
