using Unity.Cinemachine;
using UnityEngine;

namespace ReZeros.Jaxer
{
    public class RoomCamberManager : MonoBehaviour
    {
        [SerializeField] private GameObject assignedPolygon;
        [SerializeField] private PolygonCollider2D roomCollider;

        private void Awake()
        {
            roomCollider = GetComponent<PolygonCollider2D>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                assignedPolygon.transform.position = transform.position;
                PolygonCollider2D aPolygon = roomCollider.GetComponent<PolygonCollider2D>();

                aPolygon.pathCount = roomCollider.pathCount;
                for (int i = 0; i < roomCollider.pathCount; i++)
                {
                    aPolygon.SetPath(i, roomCollider.GetPath(i));
                }
            }
        }
    }
}