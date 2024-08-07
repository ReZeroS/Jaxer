using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityFx : MonoBehaviour
{
    private SpriteRenderer sr;

    [Header("Flash Fx")] 
    [SerializeField] private float hitDuration;
    [SerializeField] private Material hitMaterial;
    private Material originMaterial;

    private void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        originMaterial = sr.material;
    }

    public IEnumerator FlashFx()
    {
        sr.material = hitMaterial;
        yield return new WaitForSeconds(hitDuration);
        sr.material = originMaterial;
    }


    private void RedColorBlink()
    {
        if (sr.color != Color.white)
        {
            sr.color = Color.white;
        }
        else
        {
            sr.color = Color.red;
        }
    }

    private void CancelRedBlink()
    {
        CancelInvoke();
        sr.color = Color.white;
    }
    
    
    
}
