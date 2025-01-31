using ReZeros.Jaxer.Manager;
using UnityEngine;

public class ThunderStrikeController : MonoBehaviour
{

    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Enemy>() != null)
        {
            PlayerStat playerStat = PlayerManager.instance.Player.GetComponent<PlayerStat>();

            EnemyStat enemyTarget = other.GetComponent<EnemyStat>();
            playerStat.DoMagicalDamage(enemyTarget);
        }
    }
}
