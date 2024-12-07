using ReZeros.Jaxer.Base;
using UnityEngine;

namespace ReZeros.Jaxer.Core.Character
{
    public class Hazard : MonoBehaviour
    {
        public int damage = 20;

        private void OnTriggerStay2D(Collider2D other)
        {
            CheckCollision(other.gameObject);
        }

        private void OnCollisionStay2D(Collision2D other)
        {
            CheckCollision(other.gameObject);
        }

        private void CheckCollision(GameObject collider)
        {
            if (!collider.CompareTag("Player")) return;
            var target = collider.GetComponent<Entity>();
            if (!target) return;
            target.SetKnockBackDir(transform);
            target.stat.TakeDamage(damage);
            // var recoilDirection = (collider.transform.position - transform.position).normalized;
            // float multiplier = recoilDirection.y < 0 ? 1.0f : 500.0f;
            // Vector2 recoilForce = recoilDirection * multiplier;
            //
            // player.Hurt(damage, recoilForce, killRecoil: false);
        }
    }
}
