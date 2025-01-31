using System.Collections;
using ReZeros.Jaxer.Base;
using UnityEngine;

namespace ReZeros.Jaxer.PlayerBase
{
    [SelectionBase]
    public class MainPlayer : Entity
    {
        #region Player Data
        [Header("Attack details")]
        public Vector2[] attackMovements;

        public float counterAttackDuration;

        public bool isBusy { get; private set; }

        [Header("Move info")]
        public float maxMoveSpeed = 8f;

        private float platformSpeed = 0f; 
        public float accelerationFrame = 12f; // 加速到最大速度需要的帧数
        public float decelerationFrame = 8f; // 减速到静止需要的帧数
        public float airAccelerationMultiplier = 0.5f; // 空中加速度倍数
        
        
        

        public float jumpForce = 16f;
        public float wallJumpForce = 16f;
        public float swordReturnImpact;
        public float defaultMoveSpeed;
        public float defaultJumpForce;
        public float defaultDashSpeed;

        [Header("Dash info")]
        public float dashSpeed = 12f;

        public float dashDuration = 1.5f;
        public float dashDir { get; private set; }


        [Header("Jump info")]
        [SerializeField] private float dropDelay = 0.5f;

        public double coyoteTime = 0.2f;


        public bool canDropDown { get; private set; } = true;
        #endregion


        #region Components
        public SkillManager skillManager { get; private set; }
        public GameObject sword { get; private set; }
        public PlayerFx fx { get; private set; }

        public Transform defaultTransform;
        #endregion

        #region Material

        [SerializeField] private PhysicsMaterial2D defaultMaterial;
        #endregion
    
    
        #region States

        public PlayerStateMachine stateMachine { get; private set; }
        public PlayerSwimState playerSwimState { get; private set; }
        public PlayerIdleState playerIdleState { get; private set; }
        public PlayerMoveState playerMoveState { get; private set; }
    
        public PlayerCoyoteTimeState playerCoyoteTimeState { get; private set; }
    
        public PlayerJumpState playerJumpState { get; private set; }
    
        public PlayerFallingState playerFallingState { get; private set; }

        public PlayerWallSlideState playerWallSlideState { get; private set; }
        public PlayerWallJumpState playerWallJumpState { get; private set; }

        public PlayerDashState playerDashState { get; private set; }

        public PlayerPrimaryAttackState primaryAttackState { get; private set; }
        public PlayerCounterAttackState couterAttackState { get; private set; }

        public PlayerAnimSwordState animSwordState { get; private set; }
        public PlayerCatchSwordState catchSwordState { get; private set; }

        public PlayerBlackholeState blackholeState { get; private set; }
        public PlayerDeathState deathState { get; private set; }

        #endregion

        protected override void Awake()
        {
            base.Awake();
            stateMachine = new PlayerStateMachine();
            playerIdleState = new PlayerIdleState(stateMachine, this, "Idle");
            playerMoveState = new PlayerMoveState(stateMachine, this, "Move");
            playerCoyoteTimeState = new PlayerCoyoteTimeState(stateMachine, this, "Move");
            playerJumpState = new PlayerJumpState(stateMachine, this, "Jump");
            playerSwimState = new PlayerSwimState(stateMachine, this, "Swim");
            playerFallingState = new PlayerFallingState(stateMachine, this, "Jump");
            playerWallJumpState = new PlayerWallJumpState(stateMachine, this, "Jump");
            playerWallSlideState = new PlayerWallSlideState(stateMachine, this, "WallSlide");
            playerDashState = new PlayerDashState(stateMachine, this, "Dash");
            primaryAttackState = new PlayerPrimaryAttackState(stateMachine, this, "Attack");
            couterAttackState = new PlayerCounterAttackState(stateMachine, this, "CounterAttack");
            animSwordState = new PlayerAnimSwordState(stateMachine, this, "AnimSword");
            catchSwordState = new PlayerCatchSwordState(stateMachine, this, "CatchSword");
            blackholeState = new PlayerBlackholeState(stateMachine, this, "Jump");
            deathState = new PlayerDeathState(stateMachine, this, "Die");
        }


        protected override void Start()
        {
            base.Start();
            fx = GetComponent<PlayerFx>();
            skillManager = SkillManager.instance;

            defaultMaterial = rb.sharedMaterial;
            defaultTransform = transform.parent;
            stateMachine.Initialize(playerIdleState);


            defaultMoveSpeed = maxMoveSpeed;
            defaultJumpForce = jumpForce;
            defaultDashSpeed = dashSpeed;
        }

        public void LeavePlatform()
        {
            rb.sharedMaterial = defaultMaterial;
            transform.SetParent(defaultTransform);
        }
    
        public void EnterPlatform(Transform platform)
        {
            rb.sharedMaterial = null;
            transform.SetParent(platform); 
        }

        protected override void Update()
        {
            if (Time.timeScale == 0)
            {
                return;
            }

            base.Update();
            stateMachine.currentState.LogicUpdate();

            CheckDashInput();

            if (InputManager.instance.padLeft.justPressed && skillManager.crystalSkill.crystalUnlocked)
            {
                skillManager.crystalSkill.CanUseSkill();
            }

            if (InputManager.instance.leftShoulder.justPressed)
            {
                Inventory.instance.UseFlask();
            }
        }

        private void FixedUpdate()
        {
            stateMachine.currentState.PhysicsUpdate();
        }


        public override void SlowEntityBy(float slowPercentage, float slowDuration)
        {
            base.SlowEntityBy(slowPercentage, slowDuration);
            maxMoveSpeed *= (1 - slowPercentage);
            jumpForce *= (1 - slowPercentage);
            dashSpeed *= (1 - slowPercentage);
            animator.speed *= (1 - slowPercentage);
            Invoke(nameof(ReturnDefaultSpeed), slowDuration);
        }

        protected override void ReturnDefaultSpeed()
        {
            base.ReturnDefaultSpeed();
            maxMoveSpeed = defaultMoveSpeed;
            jumpForce = defaultJumpForce;
            dashSpeed = defaultDashSpeed;
        }


        public void AssignSword(GameObject newSword)
        {
            sword = newSword;
        }

        public void CatchSword()
        {
            stateMachine.ChangeState(catchSwordState);
            Destroy(sword);
        }


        public void SetBusyFor(float duration)
        {
            StartCoroutine(BusyFor(duration));
        }

        private IEnumerator BusyFor(float seconds)
        {
            isBusy = true;
            yield return new WaitForSeconds(seconds);
            isBusy = false;
        }

        public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();

        private void CheckDashInput()
        {
            if (IsWallDetected())
            {
                return;
            }

            if (skillManager.dashSkill.dashUnlocked == false)
            {
                return;
            }

            if (InputManager.instance.east.justPressed && skillManager.dashSkill.CanUseSkill())
            {
                dashDir = InputManager.instance.moveInput.x;
                if (dashDir == 0)
                {
                    dashDir = facingDir;
                }

                stateMachine.ChangeState(playerDashState);
            }
        }

        public override void SetupZeroKnockBackPower()
        {
            SetKnockBackPower(Vector2.zero);
        }

        public override void Die()
        {
            base.Die();
            stateMachine.ChangeState(deathState);
        }


        public void StartDropThroughPlatform()
        {
            StartCoroutine(DropThroughPlatform());
        }


        IEnumerator DropThroughPlatform()
        {
            canDropDown = false; // 防止连续下落


            // todo 播放音效和动画

            int platformLayer = LayerMask.NameToLayer("Platform");
            Physics2D.IgnoreLayerCollision(gameObject.layer, platformLayer, true);

            yield return new WaitForSeconds(dropDelay);

            Physics2D.IgnoreLayerCollision(gameObject.layer, platformLayer, false);
            canDropDown = true;
        }


        public void SetUseGravity(bool useGravity)
        {
            if (useGravity)
            {
                rb.gravityScale = 1;
            }
            else
            {
                rb.gravityScale = 0;
            }
        }
    
        public void SetGravityScale(float gravity)
        { 
            rb.gravityScale = gravity;
        }

        public void AddPlatformVelocity(float platformVelocityX)
        {
            platformSpeed = platformVelocityX;
        }

        public void RemovePlatformVelocity()
        {
            platformSpeed = 0;
        }

        public Vector2 GetVelocity()
        {
            return rb.linearVelocity;
        }

        public void SetVelocity(Vector2 velocity)
        {
            SetVelocity(velocity.x, velocity.y);
        }


        public override void SetVelocity(float xVelocity, float yVelocity)
        {
            if (isKnocked)
            {
                return;
            }
            rb.linearVelocity = new Vector2(xVelocity, yVelocity);
     
            FlipController(xVelocity);
        }
    
    
    }
}