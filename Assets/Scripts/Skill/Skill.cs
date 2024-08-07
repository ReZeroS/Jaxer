using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    [SerializeField] private float coolDown;
    private float coolDownTimer;

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
    
    
}
