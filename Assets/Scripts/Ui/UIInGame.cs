using UnityEngine;
using UnityEngine.UI;

public class UIInGame : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private PlayerStat playerStat;

    [SerializeField] private Image dashImage;
    [SerializeField] private Image parryImage;
    
    private SkillManager skillManager;
    private void Start()
    {
        if (playerStat != null)
        {
            playerStat.onHealthChanged += UpdateHealthSlider;
        }

        skillManager = SkillManager.instance;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L) && skillManager.dashSkill.dashUnlocked)
        {
            Debug.Log("cooldownImage.fillAmount = " + dashImage.fillAmount);
            SetCooldownOf(dashImage);
        }

        if (Input.GetKeyDown(KeyCode.I) && skillManager.parrySkill.parryUnlocked)
        {
            SetCooldownOf(parryImage);
        }
        

        CheckCooldownOf(dashImage, skillManager.dashSkill.cooldown);
        CheckCooldownOf(parryImage, skillManager.parrySkill.cooldown);
    }

    private void UpdateHealthSlider()
    {
        slider.maxValue = playerStat.GetMaxHealthVal();
        slider.value = playerStat.currentHealth;
    }

    public void SetCooldownOf(Image image)
    {
        if (image.fillAmount <= 0)
        {
            image.fillAmount = 1;
        }
    }


    private void CheckCooldownOf(Image image, float cooldown)
    {
        if (image.fillAmount > 0)
        {
            image.fillAmount -= 1 / cooldown * Time.deltaTime;
        }
    }
}