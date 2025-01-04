using System;
using ReZeros.Jaxer.Manager;
using Unity.Cinemachine;
using UnityEngine;

namespace ReZeros.Jaxer
{
    public class TrackedPlayer : MonoBehaviour
    {
        private CinemachineCamera mainCamera;


        private void OnValidate()
        {
            mainCamera = GetComponent<CinemachineCamera>();
        }


        private void Start()
        {
            mainCamera.Follow = PlayerManager.instance.player.transform;
        }

       
    }
}
