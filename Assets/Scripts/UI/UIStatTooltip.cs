using TMPro;
using UnityEngine;

public class UIStatTooltip : UITooltip
{
    [SerializeField] private TextMeshProUGUI description;

    public void ShowStatTooltip(string text, Vector2 position)
    {
        description.text = text;
        AdjustPosition(position);
        gameObject.SetActive(true);
    }

    public void HideTooltip()
    {
        description.text = "";
        gameObject.SetActive(false);
    }



}
