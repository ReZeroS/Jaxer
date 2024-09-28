using UnityEngine;

public class PlayerAnimationTrigger : MonoBehaviour
{
    private Player player => GetComponentInParent <Player>();

    private void AnimationTrigger()
    {
        player.AnimationTrigger();
    }

    private void AttackTrigger()
    {
        AudioManager.instance.PlaySFX(2);

        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackRadius);
        foreach (var hit in collider2Ds)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                EnemyStat enemyStat = hit.GetComponent<EnemyStat>();
                player.stat.DoDamage(enemyStat);
                Inventory.instance.GetEquipment(EquipmentType.Weapon)?.Effect(hit.transform);
                
            }
        }
    }

    private void ThrowSword()
    {
        SkillManager.instance.swordSkill.ThrowSword();
    }
    
}
