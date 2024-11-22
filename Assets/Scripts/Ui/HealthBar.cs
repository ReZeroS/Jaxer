using ReZeros.Jaxer.Base;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Entity entity => GetComponentInParent<Entity>();
    private Slider slider;
    private CharacterStat characterStat => GetComponentInParent<CharacterStat>();
    private RectTransform rectTransform;


    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        slider = GetComponentInChildren<Slider>();

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


    private void OnEnable()
    {
        entity.onFlipped += FlipUIBar;
        characterStat.onHealthChanged += UpdateHealthUI;
    }

    private void OnDisable()
    {
        if (entity)
        {
            entity.onFlipped -= FlipUIBar;
        }

        if (characterStat)
        {
            characterStat.onHealthChanged -= UpdateHealthUI;
        }
    }
}