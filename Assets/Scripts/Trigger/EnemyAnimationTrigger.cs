using UnityEngine;

public class EnemyAnimationTrigger : MonoBehaviour
{
    private Enemy enemy => GetComponentInParent <Enemy>();

    private void AnimationTrigger()
    {
        enemy.AnimationTrigger();
    }
    
    private void AttackTrigger()
    {
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(enemy.attackCheck.position, enemy.attackRadius);
        foreach (var hit in collider2Ds)
        {
            if (hit.GetComponent<Player>() != null)
            {
                PlayerStat playerStat = hit.GetComponent<PlayerStat>();
                enemy.stat.DoDamage(playerStat);
            }
        }
    }

    private void OpenCounterAttackWindow() => enemy.OpenCounterAttackWindow();
    private void CloseCounterAttackWindow() => enemy.CloseCounterAttackWindow();

    private void SpecialAttackTrigger()
    {
        enemy.AnimationSpecialTrigger();
    }

}
