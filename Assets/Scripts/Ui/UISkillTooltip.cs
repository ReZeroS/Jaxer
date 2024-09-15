using TMPro;
using UnityEngine;

public class UISkillTooltip : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private TextMeshProUGUI skillDescription;
    
    public void ShowTooltip(string name, string description)
    {
        skillName.text = name;
        skillDescription.text = description;
        gameObject.SetActive(true);
    }


    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }
}
