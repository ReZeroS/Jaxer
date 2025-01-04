using ReZeros.Jaxer.Enums;
using Unity.Cinemachine;
using UnityEngine;

namespace ReZeros.Jaxer.Scene
{
    public class LevelBoundTrigger : MonoBehaviour
    {
        [SerializeField] private CinemachineConfiner2D confiner;
        private PolygonCollider2D levelBound;
     
        

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (confiner == null)
            {
                GameObject cameraObj = GameObject.FindWithTag("MainSceneCamera");
                if (cameraObj != null)
                {
                    confiner = cameraObj.GetComponent<CinemachineConfiner2D>();
                    Debug.Log($"[LevelBoundTrigger]: Confiner auto-assigned to {cameraObj.name}", this);
                }
                else
                {
                    Debug.LogWarning("[LevelBoundTrigger]: MainSceneCamera not found in the scene!", this);
                }
            }
        }
#endif
        
        private void Awake()
        {
            levelBound = GetComponent<PolygonCollider2D>();
        }
        
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log($"[LevelBoundTrigger]: Entered {other.name}", this);
            if (!other.CompareTag("Player"))
            {
                return;
            }
            Debug.Log("level bound " + levelBound.gameObject.name + " " + gameObject.name );
            if (!levelBound || !levelBound.gameObject.name.Equals(gameObject.name))
            {
                confiner.BoundingShape2D = levelBound;
            }
        }
        
    }
}
