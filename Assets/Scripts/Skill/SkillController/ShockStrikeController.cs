using UnityEngine;

public class ShockStrikeController : MonoBehaviour
{
    [SerializeField] private CharacterStat targetStat;
    [SerializeField] private float speed;

    private int damage;
    

    private Animator animator;
    private static readonly int Hit = Animator.StringToHash("Hit");



    private bool triggered;


    public void Setup(int dam, CharacterStat stat)
    {
        damage = dam;
        targetStat = stat;
    }
    
    
    
    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!targetStat)
        {
            return;
        }
        
        if (triggered)
        {
            return;
        }
        transform.position = Vector2.MoveTowards(transform.position, targetStat.transform.position,
                speed * Time.deltaTime);
        transform.right = transform.position - targetStat.transform.position;
        
        if (Vector2.Distance(transform.position, targetStat.transform.position) < .1f)
        {
            animator.transform.localPosition = new Vector2(0, 0.5f);
            animator.transform.localRotation = Quaternion.identity;
            transform.localRotation = Quaternion.identity;
            transform.localScale = new Vector3(3, 3);

            triggered = true;
            animator.SetTrigger(Hit);
            
            Invoke(nameof(DamageAndDestroySelf), .2f);
        }
    }
    
    
    private void DamageAndDestroySelf()
    {
        targetStat.ApplyShock(true);
        targetStat.TakeDamage(damage);
        Destroy(gameObject);
    }
}
