using ReZeros.Jaxer.Base;
using ReZeros.Jaxer.PlayerBase;
using UnityEngine;
using Random = UnityEngine.Random;

public class CrystalSkillController : MonoBehaviour
{

    private MainPlayer mainPlayer;
    private Animator animator => GetComponent<Animator>();
    private CircleCollider2D cd => GetComponent<CircleCollider2D>();

    private bool canExplode;

    private bool canMove;
    private float moveSpeed;


    private bool canGrow;
    private float growSpeed = 5;
    private Transform closestTarget;
    [SerializeField] private LayerMask whatIsEnemy;
    

    private float crystalExistTimer;
    private static readonly int Explode = Animator.StringToHash("Explode");

    public void SetupCrystal(float crystalDuration, bool canEx, bool canMv, float moveSp,
        Transform findClosestEnemy, MainPlayer pl)
    {
        crystalExistTimer = crystalDuration;
        canExplode = canEx;
        canMove = canMv;
        moveSpeed = moveSp;
        closestTarget = findClosestEnemy;
        mainPlayer = pl;
    }

    public void ChooseRandomEnemy()
    {
        float blackholeRadius = SkillManager.instance.blackholeSkill.GetBlackholeRadius();
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(transform.position, blackholeRadius, whatIsEnemy);
        if (collider2Ds.Length > 0)
        {
            closestTarget = collider2Ds[Random.Range(0, collider2Ds.Length)].transform;
        }
    }


    private void Update()
    {
        crystalExistTimer -= Time.deltaTime;
        if (crystalExistTimer < 0)
        {
            FinishedCrystal();
        }

        if (canMove)
        {
            transform.position =
                Vector2.MoveTowards(transform.position, closestTarget.position, moveSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, closestTarget.position) < 2)
            {
                FinishedCrystal();
                // can not move after explode 
                canMove = false;
            }
        }
        
        if (canGrow)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector3(3, 3), growSpeed * Time.deltaTime);
        }
        
    }

    public void FinishedCrystal()
    {
        if (canExplode)
        {
            canGrow = true;
            animator.SetTrigger(Explode);
        }
        else
        {
            SelfDestroy();
        }
    }

    private void SelfDestroy()
    {
        Destroy(gameObject);
    }


    public void CrystalExplodeAnimationEvent()
    {
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(transform.position, cd.radius);
        
        foreach (var hit in collider2Ds)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            hit.GetComponent<Entity>()?.SetKnockBackDir(transform);
            if (enemy)
            {
                
                mainPlayer.stat.DoMagicalDamage(enemy.GetComponent<CharacterStat>());
                ItemDataEquipment amulet = Inventory.instance.GetEquipment(EquipmentType.Amulet);
                amulet?.Effect(enemy.transform);
            }
        }
    }


}
