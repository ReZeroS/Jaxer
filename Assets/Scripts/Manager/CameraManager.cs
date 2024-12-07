using UnityEngine;
using DG.Tweening;
using Unity.Cinemachine;

namespace ReZeros.Jaxer.Manager
{
    public class CameraManager : MonoBehaviour
    {
        public static CameraManager Instance;
        public CinemachineCamera cinemachineCamera;
        private Transform cameraTransform;
        private Vector3 originalPosition;
        private CinemachineBasicMultiChannelPerlin noise; // 用于震动的噪声组件

        private void Awake()
        {
            Instance = this;
            if (cinemachineCamera != null)
            {
                // 获取噪声组件（CinemachineBasicMultiChannelPerlin）
                noise = cinemachineCamera.GetComponentInChildren<CinemachineBasicMultiChannelPerlin>();
            }
        }

        // 调用此方法来启动摄像机抖动
        public void ShakeCamera(float intensity, float duration = 1f)
        {
            if (noise != null)
            {
                noise.enabled = true;
                // 设置初始震动强度
                noise.AmplitudeGain = intensity;

                // 使用 DOTween 缓动将震动强度减弱至 0
                DOTween.To(() => noise.AmplitudeGain, 
                        x => noise.AmplitudeGain = x, 0, duration)
                    .SetEase(Ease.OutQuad)
                    .OnComplete(() => noise.enabled = false); // 完成后禁用噪声
            }
            else
            {
                Debug.LogWarning("CinemachineBasicMultiChannelPerlin component not found!");
            }
        }
    }
}