using ReZeros.Jaxer.Manager;
using UnityEngine;
using UnityEngine.Serialization;
using CameraType = ReZeros.Jaxer.Manager.CameraType;

namespace ReZeros.Jaxer.Trigger.BossTrigger
{
    public class HornetArenaTrigger : MonoBehaviour
    {
        public GameObject hornetPrefab;
        // public Transform hornetPosition;
        public MainCameraSwitcher mainCameraSwitcher;


        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                // 切换为竞技场镜头
                mainCameraSwitcher.SwitchCamera(CameraType.ArenaCamera);
                Debug.Log($"Enter hornet {hornetPrefab.name}");
                // 关门

                // 生成大黄蜂
                hornetPrefab.SetActive(true);
            }
        }
    }
}