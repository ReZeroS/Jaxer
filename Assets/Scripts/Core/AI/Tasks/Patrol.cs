using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace ReZeros.Jaxer.Core.AI.Tasks
{
    public class Patrol : EnemyAction
    {
        public float moveSpeed = 6f;
        public float minbistance = 4f;
        public string runAnimation = "IsRunning";
        private float direction;

        public override void OnStart()
        {
            animator.SetBool(runAnimation, true);
            var diff = transform.position.x - player.transform.position.x;
            var playerDirection = Mathf.Sign(diff);
            direction = Mathf.Abs(diff) < minbistance ? playerDirection : -playerDirection;
        }


        public override TaskStatus OnUpdate()
        {
            body.linearVelocity = Vector2.right * moveSpeed * direction;
            var scale = transform.localScale;
            scale.x = direction;
            transform.localScale = scale;
            return TaskStatus.Running;
        }

        public override void OnEnd()
        {
            animator.SetBool(runAnimation, false);
        }
    }
}