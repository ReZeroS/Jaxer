using BehaviorDesigner.Runtime.Tasks;
using DG.Tweening;
using ReZeros.Jaxer.Manager;
using UnityEngine;

namespace ReZeros.Jaxer.Core.AI.Tasks
{
    public class Jump : EnemyAction
    {
        public float horizontalForce = 5.0f;
        public float jumpForce = 10.0f;

        public float buildupTime;
        public float jumpTime;

        public string animationTriggerName;
        
        public string mainAnimationTriggerName;
        
        
        public SpriteRenderer jumpEffect;
        public Vector3 effectOffset;
        
        public bool shakeCameraOnLanding;

        private bool hasLanded;

        private Tween buildupTween;
        private Tween jumpTween;
        
        public override void OnStart()
        {
            buildupTween = DOVirtual.DelayedCall(buildupTime, StartJump, false);
            animator.SetTrigger(animationTriggerName);
        }

        private void StartJump()
        {
            if (!string.IsNullOrEmpty(mainAnimationTriggerName))
            {
                animator.SetTrigger(mainAnimationTriggerName);
            }
            
            var direction = player.transform.position.x < transform.position.x ? -1 : 1;
            body.AddForce(new Vector2(horizontalForce * direction, jumpForce), ForceMode2D.Impulse);

            if (jumpEffect != null)
            {
                EffectManager.Instance.PlaySpriteOneShot(jumpEffect, transform.position + effectOffset, direction > 0);
            }
            
            jumpTween = DOVirtual.DelayedCall(jumpTime, () =>
            {
                hasLanded = true;
                body.linearVelocity = Vector2.zero; 
                if (shakeCameraOnLanding)
                    CameraManager.Instance.ShakeCamera(5f);
            }, false);
        }

        public override TaskStatus OnUpdate()
        {
            return hasLanded ? TaskStatus.Success : TaskStatus.Running;
        }

        public override void OnEnd()
        {
            buildupTween?.Kill();
            jumpTween?.Kill();
            hasLanded = false;
        }
    }
}
