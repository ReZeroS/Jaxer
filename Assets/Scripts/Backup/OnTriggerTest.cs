using System;
using UnityEngine;

public class OnTriggerTest : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("OnCollisionEnter2D" + other.gameObject.name);
    }
}
