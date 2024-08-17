using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    private Entity entity;
    private Slider slider;
    private CharacterStat characterStat;
    private RectTransform rectTransform;


    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        entity = GetComponentInParent<Entity>();
        slider = GetComponentInChildren<Slider>();
        characterStat = GetComponentInParent<CharacterStat>();
        entity.onFlipped += FlipUIBar;
        characterStat.onHealthChanged += UpdateHealthUI;
        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        slider.maxValue = characterStat.GetMaxHealthVal();
        slider.value = characterStat.currentHealth;
    }
    
    

    private void FlipUIBar()
    {
        rectTransform.Rotate(0, 180, 0);
    }

    private void OnDisable()
    {
        entity.onFlipped -= FlipUIBar;
        characterStat.onHealthChanged -= UpdateHealthUI;
    }
}