using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

namespace ReZeros.Jaxer.Manager
{
    public enum CameraType
    {
        MainLevelCamera,
        ArenaCamera,
    }


    public class MainCameraSwitcher : MonoBehaviour
    {
        // 单例实例

        [Header("Cameras")]
        public CinemachineCamera mainLevelCamera;
        public CinemachineCamera arenaCamera;

        private const int ActiveCameraPriority = 10;
        private const int InactiveCameraPriority = 0;

        private void Awake()
        {
            SwitchCamera(CameraType.MainLevelCamera);
        }

        /// <summary>
        /// 切换到指定类型的摄像机。
        /// </summary>
        /// <param name="cameraType">目标摄像机类型。</param>
        public void SwitchCamera(CameraType cameraType)
        {
            // 设置所有摄像机为非激活状态
            if (mainLevelCamera != null)
                mainLevelCamera.Priority = InactiveCameraPriority;

            if (arenaCamera != null)
                arenaCamera.Priority = InactiveCameraPriority;

            // 激活目标摄像机
            switch (cameraType)
            {
                case CameraType.MainLevelCamera:
                    if (mainLevelCamera != null)
                        mainLevelCamera.Priority = ActiveCameraPriority;
                    break;

                case CameraType.ArenaCamera:
                    if (arenaCamera != null)
                        arenaCamera.Priority = ActiveCameraPriority;
                    break;

                default:
                    Debug.LogError($"Invalid camera type: {cameraType}");
                    break;
            }
        }
    }
}