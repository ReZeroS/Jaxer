using BehaviorDesigner.Runtime.Tasks;
using DG.Tweening;
using ReZeros.Jaxer.Manager;
using UnityEngine;

namespace ReZeros.Jaxer.Core.AI.Tasks.Hornet
{
    public class GossamerStorm : EnemyAction
    {
        public float airborneChance = 0.5f;
        public float jumpForce = 10.0f;
        public float jumpTime;

        public float buildupTime;
        public float stormTime;

        public string buildupAnimation;
        public string buildupAirAnimation;
        public string mainAnimation;
        
        public GameObject stormPrefab;
        public Vector3 stormOffset;

        private bool hasLanded;
        float defaultGravity;
        bool isAirborne;
        GameObject stormObject;

        private Tween buildupTween;
        private Tween jumpTween;
        private Tween stormTween;
        

        public override void OnAwake()
        {
            base.OnAwake();
            defaultGravity = body.gravityScale;
        }

        public override void OnStart()
        {
            isAirborne = Random.value < airborneChance;
            animator.SetTrigger(isAirborne? buildupAirAnimation: buildupAnimation);
            if (isAirborne)
            {
                body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                jumpTween = DOVirtual.DelayedCall(jumpTime, StartBuildup, false);
            }
            else
            {
                StartBuildup();
            }
                
        }

        void StartBuildup()
        {
            body.gravityScale = 0;
            body.linearVelocity = Vector2.zero;
            buildupTween = DOVirtual.DelayedCall(buildupTime, StartStorm, false);
        }


        private void StartStorm()
        {
            animator.SetBool(mainAnimation, true);

            var direction = transform.localScale.x;
            CameraManager.Instance.ShakeCamera(0.2f);

            stormObject = Object.Instantiate(stormPrefab, transform.position + stormOffset, Quaternion.identity);
            stormObject.transform.localScale = new Vector3(direction, 1, 1);
            
            stormTween = DOVirtual.DelayedCall(stormTime, () =>
            {
                hasLanded = true;
                animator.SetBool(mainAnimation, false);
                body.gravityScale = defaultGravity;
                Object.Destroy(stormObject);
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
            stormTween?.Kill();
            hasLanded = false;
            body.gravityScale = defaultGravity;
            Object.Destroy(stormObject);
        }
    }
}