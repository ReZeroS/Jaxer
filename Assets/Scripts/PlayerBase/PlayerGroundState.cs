namespace ReZeros.Jaxer.PlayerBase
{
    public class PlayerGroundState : PlayerState
    {
    
        // 定义状态转换检查的优先级顺序
        private readonly StateTransitionCheck[] stateTransitions;

        protected PlayerGroundState(PlayerStateMachine stateMachine, MainPlayer mainPlayer, string animBoolName) 
            : base(stateMachine, mainPlayer, animBoolName)
        {
            // 按优先级顺序初始化状态转换检查
            stateTransitions = new[]
            {
                new StateTransitionCheck(CheckDropDown, () => mainPlayer.playerFallingState),
                new StateTransitionCheck(CheckBlackholeSkill, () => mainPlayer.blackholeState),
                new StateTransitionCheck(CheckSwordSkill, () => mainPlayer.animSwordState),
                new StateTransitionCheck(CheckCounterAttack, () => mainPlayer.couterAttackState),
                new StateTransitionCheck(CheckPrimaryAttack, () => mainPlayer.primaryAttackState),
                // 优先进入土狼时间？
                new StateTransitionCheck(CheckFalling, () => mainPlayer.playerFallingState),
                new StateTransitionCheck(CheckJump, () => mainPlayer.playerJumpState)
            };
        }

        public override void Enter()
        {
            base.Enter();
            mainPlayer.playerJumpState.ResetAmountOfJumpsLeft();

        }

        public override void Exit() => base.Exit();

        public override void LogicUpdate()
        {
            base.LogicUpdate();
        
            foreach (var transition in stateTransitions)
            {
                if (transition.TryTransition(stateMachine))
                    return;
            }
        }

        #region State Transition Checks
        private bool CheckDropDown()
        {
            if (!ShouldDropDown()) return false;
        
            mainPlayer.StartDropThroughPlatform();
            return true;
        }

        private bool CheckBlackholeSkill()
        {
            if (!CanUseBlackholeSkill()) return false;
        
            return !mainPlayer.skillManager.blackholeSkill.IsInCoolDown();
        }

        private bool CheckSwordSkill()
        {
            if (!CanUseSwordSkill()) return false;
        
            return HasNoSword();
        }

        private bool CheckCounterAttack()
        {
            return CanUseCounterAttack();
        }

        private bool CheckPrimaryAttack()
        {
            return InputManager.instance.west.justPressed;
        }

        private bool CheckFalling()
        {
            return !mainPlayer.IsGroundDetected();
        }

        private bool CheckJump()
        {
            return InputManager.instance.south.justPressed 
                   && mainPlayer.IsGroundDetected() 
                   && mainPlayer.playerJumpState.CanJump();
        }
        #endregion

        #region Condition Checks
        private bool ShouldDropDown()
        {
            return mainPlayer.canDropDown && 
                   InputManager.instance.moveInput.y < 0 && 
                   InputManager.instance.south.justPressed;
        }

        private bool CanUseBlackholeSkill()
        {
            return InputManager.instance.padDown.justPressed && 
                   mainPlayer.skillManager.blackholeSkill.blackholeUnlocked;
        }

        private bool CanUseSwordSkill()
        {
            return InputManager.instance.rightTrigger.justPressed && 
                   mainPlayer.skillManager.swordSkill.swordUnlocked;
        }

        private bool CanUseCounterAttack()
        {
            return InputManager.instance.north.beingHeld && 
                   mainPlayer.skillManager.parrySkill.parryUnlocked;
        }

        private bool HasNoSword()
        {
            if (!mainPlayer.sword) return true;
        
            mainPlayer.sword.GetComponent<SwordSkillController>().ReturnSword();
            return false;
        }
        #endregion
    }
}
