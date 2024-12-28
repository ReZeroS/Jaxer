using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace ReZeros.Jaxer.Core.AI.Tasks
{
    public class GotoNextStage : EnemyAction
    {
        public SharedInt CurrentStage;

        public override TaskStatus OnUpdate()
        {
            CurrentStage.Value++;
            return TaskStatus.Success;
        }
    }
}