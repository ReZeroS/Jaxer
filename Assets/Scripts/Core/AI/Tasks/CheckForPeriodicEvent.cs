using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace ReZeros.Jaxer.Core.AI.Tasks
{
    public class CheckForPeriodicEvent : EnemyConditional
    {

        public float interval = 1f;
        public SharedFloat PeriodicTimer;


        public override TaskStatus OnUpdate()
        {
            PeriodicTimer.Value += Time.deltaTime;
            if (PeriodicTimer.Value > interval)
            {
                return TaskStatus.Success;
            }
            return TaskStatus.Failure;
        }
    }
}