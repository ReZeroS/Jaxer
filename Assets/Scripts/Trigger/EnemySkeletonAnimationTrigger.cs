using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkeletonAnimationTrigger : MonoBehaviour
{
    private EnemySkeleton enemySkeleton => GetComponentInParent <EnemySkeleton>();

    private void AnimationTrigger()
    {
        enemySkeleton.AnimationTrigger();
    }
    
    private void AttackTrigger()
    {
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(enemySkeleton.attackCheck.position, enemySkeleton.attackRadius);
        foreach (var hit in collider2Ds)
        {
            if (hit.GetComponent<Player>() != null)
            {
                hit.GetComponent<Player>().Damage();
            }
        }
    }

    private void OpenCounterAttackWindow() => enemySkeleton.OpenCounterAttackWindow();
    private void CloseCounterAttackWindow() => enemySkeleton.CloseCounterAttackWindow();


}
