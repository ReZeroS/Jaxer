using UnityEngine;

public class ExplosiveController : MonoBehaviour
{
    private Animator animator;
    private CharacterStat curStat;
    private float growSpeed = 10f;
    private float maxSize = 6;
    private float explosionRadius = 1.5f;

    private bool canGrow = true;
    private static readonly int Explode = Animator.StringToHash("Explode");

    private void Update()
    {
        if (canGrow)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize),
                Time.deltaTime * growSpeed);
        }

        if (maxSize - transform.localScale.x < 0.1f)
        {
            canGrow = false;
            animator.SetTrigger(Explode);
        }
    }

    public void SetupExplosive(CharacterStat stat, float grSP, float max, float exRadius)
    {
        animator = GetComponent<Animator>();
        curStat = stat;
        growSpeed = grSP;
        maxSize = max;
        explosionRadius = exRadius;
    }


    public void ExplodeAnimationEvent()
    {
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (var hit in collider2Ds)
        {
            if (hit.GetComponent<CharacterStat>())
            {
                hit.GetComponent<Entity>()?.SetKnockBackDir(transform);
                curStat.DoDamage(hit.GetComponent<CharacterStat>());
            }
        }
    }

    private void SelfDestroy()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}