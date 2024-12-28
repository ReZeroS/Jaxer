using BehaviorDesigner.Runtime.Tasks;
using DG.Tweening;
using ReZeros.Jaxer.Manager;
using UnityEngine;

namespace ReZeros.Jaxer.Core.AI.Tasks
{
    public class DestroyBoss : EnemyAction
    {
        public float bleedTime = 2.0f;
        public ParticleSystem bleedEffect;
        public ParticleSystem explodeEffect;

        private bool isDestroyed;
        
        public override void OnStart()
        {
            EffectManager.Instance.PlayOneShot(bleedEffect, transform.position);
            DOVirtual.DelayedCall(bleedTime, () =>
            {
                EffectManager.Instance.PlayOneShot(explodeEffect, transform.position);
                CameraManager.Instance.ShakeCamera(0.7f);
                isDestroyed = true;
                Object.Destroy(gameObject);
            }, false);
        }

        public override TaskStatus OnUpdate()
        {
            return isDestroyed ? TaskStatus.Success : TaskStatus.Running;
        }
    }
}