using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace ReZeros.Jaxer.Core.AI.Tasks
{
    public class FreezeTime : EnemyAction
    {
        public SharedFloat Duration = 0.1f;

        public override TaskStatus OnUpdate()
        {
            GameManager.instance.FreezeTime(Duration.Value);
            return TaskStatus.Success;
        }
    }
}