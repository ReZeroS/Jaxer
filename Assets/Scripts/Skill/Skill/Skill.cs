using ReZeros.Jaxer.Manager;
using ReZeros.Jaxer.PlayerBase;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public float cooldown;
    protected float coolDownTimer;
    protected MainPlayer mainPlayer;

    protected virtual void Start()
    {
        mainPlayer = PlayerManager.instance.Player;
        CheckUnlock();
    }

    protected virtual void Update()
    {
        coolDownTimer -= Time.deltaTime;
    }

    public virtual void CheckUnlock()
    {
    }
    


    public virtual bool CanUseSkill()
    {
        if (coolDownTimer < 0)
        {
            coolDownTimer = cooldown;
            UseSkill();
            return true;
        }
        return false;
    }

    public virtual void UseSkill()
    {
        
    }
    
    
    public virtual Transform FindClosestEnemy(Transform checkTransform)
    {
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(checkTransform.position, 25);
        float closeDistance = Mathf.Infinity;
        Transform closestEnemy = null;
        foreach (var hit in collider2Ds)
        {
            if (hit.GetComponent<Enemy>())
            {
                float curDistance = Vector2.Distance(hit.transform.position, checkTransform.position);
                if (curDistance < closeDistance)
                {
                    closeDistance = curDistance;
                    closestEnemy = hit.transform;
                }
            }
        }
        return closestEnemy;
    }
    
    
}
