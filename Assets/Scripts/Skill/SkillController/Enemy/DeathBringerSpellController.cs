using ReZeros.Jaxer.Base;
using UnityEngine;

public class DeathBringerSpellController : MonoBehaviour
{
    [SerializeField] private Transform check;
    [SerializeField] private Vector2 boxSize;
    [SerializeField] private LayerMask whatIsPlayer;

    private CharacterStat characterStat;


    public void SetupSpell(CharacterStat characterStat)
    {
        this.characterStat = characterStat;
    }


    private void AnimationTrigger()
    {
        Collider2D[] overlapBox = Physics2D.OverlapBoxAll(check.position, boxSize, whatIsPlayer);

        foreach (Collider2D collider in overlapBox)
        {
            Player target = collider.GetComponent<Player>();
            if (target != null)
            {
                collider.GetComponent<Entity>().SetKnockBackDir(transform);
                characterStat.DoDamage(target.stat);
            }
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(check.position, boxSize);
    }


    private void SelfDestroy()
    {
        Destroy(gameObject);
    }
    
}