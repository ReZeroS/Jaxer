using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISkillTreeSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISaveManager
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
        if (unlocked)
        {
            skillImage.color = Color.white;
        }
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
        ui.skillTooltip.ShowTooltip(skillName, skillDescription, eventData.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.skillTooltip.HideTooltip();
    }

    public void LoadData(GameData gameData)
    {
        if (gameData.skillTree.TryGetValue(skillName, out bool savedUnlocked))
        {
            unlocked = savedUnlocked;
        }
    }

    public void SaveData(ref GameData gameData)
    {
        if (gameData.skillTree.TryGetValue(skillName, out _))
        {
            gameData.skillTree.Remove(skillName);
            gameData.skillTree.Add(skillName, unlocked);
        }
        else
        {
            gameData.skillTree.Add(skillName, unlocked);
        }
    }
}