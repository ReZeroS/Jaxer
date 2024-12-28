using System;
using ReZeros.Jaxer.Base;
using Sound.SoundManager;
using UnityEngine;

namespace ReZeros.Jaxer.Core.Combat.Projectile
{
    public abstract class AbstractProjectile : MonoBehaviour
    {
        public int damage;
        public ParticleSystem explosionEffect;
        public AudioClip splatterSound;

        public GameObject Shooter { get; set; }

        protected Vector2 force;

        public event Action<AbstractProjectile> OnProjectileDestroyed;

        public abstract void SetForce(Vector2 forc);

        protected void DestroyProjectile()
        {
            OnProjectileDestroyed?.Invoke(this);

            if (splatterSound != null)
                SoundManager.instance.PlaySoundAtLocation(splatterSound, transform.position, 0.75f);

            if (explosionEffect != null)
            {
                Debug.Log("Destroy Projectile "+ explosionEffect.name);
                EffectManager.Instance.PlayOneShot(explosionEffect, transform.position);
            }
          
            // Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            // Can't shoot yourself
            if (collision.gameObject == Shooter)
                return;

            // Projectile hit player
            var player = collision.GetComponent<Player>();
            if (player != null)
            {
                var target = collision.GetComponent<Entity>();
                if (target)
                {
                    target.SetKnockBackDir(transform);
                    target.stat.TakeDamage(damage);
                }
            }

            DestroyProjectile();
        }
    }
}