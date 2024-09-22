public class EnemyDeathBringerTrigger : EnemyAnimationTrigger
{
    private EnemyDeathBringer enemyDeathBringer => GetComponentInParent<EnemyDeathBringer>();


    private void Relocate() => enemyDeathBringer.FindPosition();


    private void MakeVisible() => enemyDeathBringer.fx.MakeTransparent(false);
    
    
    private void MakeInvisible() => enemyDeathBringer.fx.MakeTransparent(true);
}