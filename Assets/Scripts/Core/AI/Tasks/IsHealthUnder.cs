using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace ReZeros.Jaxer.Core.AI.Tasks
{
    public class IsHealthUnder: EnemyConditional
    {

        public SharedInt HealthThrehold;
        
        public override TaskStatus OnUpdate()
        {
            return destructable.CurrentHealth < HealthThrehold.Value ? TaskStatus.Success : TaskStatus.Failure;
        }
    }
}
