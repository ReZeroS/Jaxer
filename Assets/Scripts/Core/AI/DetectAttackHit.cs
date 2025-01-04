using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace ReZeros.Jaxer.Core.AI
{
    public class DetectAttackHit : EnemyConditional
    {
        public bool waitForHit;
        private bool isGettingHit;

        public override void OnAwake()
        {
            base.OnAwake();
            destructable.OnHit += OnHit;
        }


        void OnHit(Vector2 position, Vector2 force)
        {
            isGettingHit = true;
        }

        public override void OnStart()
        {
            if (waitForHit)
            {
                isGettingHit = false;
            }
        }


        public override TaskStatus OnUpdate()
        {
            var returnTypeNegative = waitForHit ? TaskStatus.Running : TaskStatus.Failure;
            return isGettingHit ? TaskStatus.Success : returnTypeNegative;
        }

        public override void OnEnd()
        {
            isGettingHit = false;
        }
    }
}