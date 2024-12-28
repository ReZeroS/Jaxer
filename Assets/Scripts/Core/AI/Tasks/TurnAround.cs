using BehaviorDesigner.Runtime.Tasks;

namespace ReZeros.Jaxer.Core.AI.Tasks
{
    public class TurnAround : EnemyAction
    {
        public override TaskStatus OnUpdate()
        {
            var scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;

            return TaskStatus.Success;
        }
    }
}