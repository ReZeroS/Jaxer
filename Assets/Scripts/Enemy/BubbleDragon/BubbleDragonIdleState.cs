using UnityEngine;

public class BubbleDragonIdleState : BubbleDragonGroundState
{
    public BubbleDragonIdleState(EnemyStateMachine stateMachine, Enemy baseEnemy, string animationName) : base(
        stateMachine, baseEnemy, animationName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = enemy.idleTime;
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer < 0)
        {
            int range = Random.Range(0, 100);
            if (range < 10)
            {
                stateMachine.ChangeState(enemy.sleepState);
            }
            else
            {
                stateMachine.ChangeState(enemy.moveState);
            }
        }
    }


    public override void Exit()
    {
        base.Exit();
    }
}