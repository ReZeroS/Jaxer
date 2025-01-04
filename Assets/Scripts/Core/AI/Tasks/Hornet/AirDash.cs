using BehaviorDesigner.Runtime.Tasks;
using DG.Tweening;
using ReZeros.Jaxer.Manager;
using UnityEngine;

namespace ReZeros.Jaxer.Core.AI.Tasks.Hornet
{
    public class AirDash : EnemyAction
    {
        public float dashForce = 5.0f;
        public float jumpForce = 10.0f;

        public float buildupTime;
        public float jumpTime;
        public float dashTime;

        public string buildupAnimation;
        
        public string mainAnimation;
        
        
        public SpriteRenderer jumpEffect;
        public Vector3 effectOffset;
        
        public bool shakeCameraOnLanding;

        private bool hasLanded;

        private Tween buildupTween;
        private Tween jumpTween;
        private Tween dashTween;


        float defaultGravity;
        
        
        public override void OnAwake()
        {
            base.OnAwake();
            defaultGravity = body.gravityScale;
        }

        public override void OnStart()
        {
            body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpTween = DOVirtual.DelayedCall(jumpTime, StartBuildUp, false);
            animator.SetTrigger(buildupAnimation);
        }

        void StartBuildUp()
        {
            body.gravityScale = 0;
            body.linearVelocity = Vector2.zero;
            buildupTween = DOVirtual.DelayedCall(buildupTime, StartDash, false);
        }
        

        private void StartDash()
        {
            if (!string.IsNullOrEmpty(mainAnimation))
            {
                animator.SetTrigger(mainAnimation);
            }
            body.gravityScale = defaultGravity;
            var direction = transform.localScale.x;
            var distance = Mathf.Abs(transform.position.x - player.transform.position.x);
            var downwardsDirection = distance < 4f ? -1 : -0.5f;
            body.AddForce(new Vector2(direction, downwardsDirection) * dashForce, ForceMode2D.Impulse);
            
            if (jumpEffect != null)
            {
                EffectManager.Instance.PlaySpriteOneShot(jumpEffect, transform.position + effectOffset, direction > 0);
            }
            
            dashTween = DOVirtual.DelayedCall(dashTime, () =>
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
            dashTween?.Kill();
            hasLanded = false;
            body.gravityScale = defaultGravity;
        }
    }
}
