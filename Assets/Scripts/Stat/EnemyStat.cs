public class EnemyStat : CharacterStat
{
    private Enemy enemy;
    
    protected override void Start()
    {
        base.Start();
        enemy = GetComponent<Enemy>();
    }

    public override void TakeDamage(int dam)
    {
        base.TakeDamage(dam);
    }

    public override void Die()
    {
        base.Die();
        enemy.Die();
    }
}
