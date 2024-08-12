using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    [SerializeField] private float coolDown;
    [SerializeField] private float coolDownTimer;
    protected Player player;

    protected virtual void Start()
    {
        player = PlayerManager.instance.player;
    }

    protected virtual void Update()
    {
        coolDownTimer -= Time.deltaTime;
    }


    public virtual bool CanUseSkill()
    {
        if (coolDownTimer < 0)
        {
            coolDownTimer = coolDown;
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
