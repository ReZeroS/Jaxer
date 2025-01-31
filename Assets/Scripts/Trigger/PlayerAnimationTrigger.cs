using ReZeros.Jaxer.Core.Combat;
using Sound.SoundManager;
using UnityEngine;

namespace ReZeros.Jaxer.Trigger
{
    public class PlayerAnimationTrigger : MonoBehaviour
    {
        private PlayerBase.MainPlayer mainPlayer => GetComponentInParent<PlayerBase.MainPlayer>();

        private void AnimationTrigger()
        {
            mainPlayer.AnimationTrigger();
        }

        private void AttackTrigger()
        {
            SoundManager.PlaySound(SoundType.LIGHT_ATTACK);

            Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(mainPlayer.attackCheck.position, mainPlayer.attackRadius);
            foreach (var hit in collider2Ds)
            {
                if (hit.GetComponent<Enemy>() != null)
                {
                    EnemyStat enemyStat = hit.GetComponent<EnemyStat>();
                    mainPlayer.stat.DoDamage(enemyStat);
                    Inventory.instance.GetEquipment(EquipmentType.Weapon)?.Effect(hit.transform);
                }

                var hittable = hit.GetComponent<Hittable>();
                if (hittable != null)
                {
                    hittable.OnAttackHit(hit.transform.position, new Vector2(3, 5), 5);
                }
            }
        }

        private void ThrowSword()
        {
            SkillManager.instance.swordSkill.ThrowSword();
        }
    }
}