using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using ReZeros.Jaxer.Core.AI;
using UnityEngine;

namespace Core.AI
{
    public class SetHealth : EnemyAction
    {
        public SharedInt Health;

        public override TaskStatus OnUpdate()
        {
            destructable.CurrentHealth = Health.Value;
            return TaskStatus.Success;
        }
    }
}