using BehaviorDesigner.Runtime.Tasks;

namespace ReZeros.Jaxer.Core.AI.Tasks
{
    public class InitBoss : EnemyAction
    {
        public override TaskStatus OnUpdate()
        {
            // GuiManager.Instance.ShowBossName(GetComponent<BossConfig>().bossName);
            return TaskStatus.Success;
        }
    }
}