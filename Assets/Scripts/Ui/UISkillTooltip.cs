using TMPro;
using UnityEngine;

public class UISkillTooltip : UITooltip
{

    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private TextMeshProUGUI skillDescription;
    
    public void ShowTooltip(string na, string description, Vector2 position)
    {
        skillName.text = na;
        skillDescription.text = description;
        AdjustPosition(position);
        gameObject.SetActive(true);
    }


    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }
}
