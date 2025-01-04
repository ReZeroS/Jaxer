using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace ReZeros.Jaxer.Core.AI
{
    public class PlayParticles : EnemyAction
    {
        public ParticleSystem effect;

        public override TaskStatus OnUpdate()
        {
            EffectManager.Instance.PlayOneShot(effect, transform.position);
            return TaskStatus.Success;
        }
    }
}