using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISkillTreeSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private UI ui;
    [SerializeField] private Image skillImage;

    [SerializeField] private int skillPrice;
    
    [SerializeField] private string skillName;
    [TextArea] [SerializeField] private string skillDescription;
    [SerializeField] private Color lockedSkillColor;

    public bool unlocked; 

    [SerializeField] private UISkillTreeSlot[] shouldBeUnlocked;
    [SerializeField] private UISkillTreeSlot[] shouldBeLocked;


    private void OnValidate()
    {
        gameObject.name = "SkillTreeSlot - " + skillName;
    }

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(UnlockSkillSlot);
    }

    private void Start()
    {
        ui = GetComponentInParent<UI>();
        skillImage = GetComponent<Image>();
        

        skillImage.color = lockedSkillColor;

    }


    public void UnlockSkillSlot()
    {

       
        
        // check if all the skills that should not be unlocked are locked
        for (int i = 0; i < shouldBeUnlocked.Length; i++)
        {
            if (shouldBeUnlocked[i].unlocked == false)
            {
                return;
            }
        }

        // Check if all the skills that should be unlocked are unlocked
        for (int i = 0; i < shouldBeLocked.Length; i++)
        {
            if (shouldBeLocked[i].unlocked == true)
            {
                return;
            }
        }
        
        if (PlayerManager.instance.HaveEnoughCurrency(skillPrice) == false)
        {
            return;
        }
        
        unlocked = true;
        skillImage.color = Color.white ;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.skillTooltip.ShowTooltip(skillName, skillDescription);
        Vector2 pointerPosition = eventData.position;

        var halfScreenWidth = Screen.width / 2;
        float xOffset, yOffset;
        if (pointerPosition.x > halfScreenWidth)
        {
            xOffset = -150;
        }
        else
        {
            xOffset = 150;
        }

        var halfScreenHeight = Screen.height /2;
        if (pointerPosition.y > halfScreenHeight)
        {
            yOffset = -150;
        }
        else
        {
            yOffset = 150;
        }

        ui.skillTooltip.transform.position = new Vector2(pointerPosition.x + xOffset, pointerPosition.y + yOffset);
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.skillTooltip.HideTooltip();
    }
}