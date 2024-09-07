using TMPro;
using UnityEngine;

public class UIStatTooltip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI description;

    public void ShowStatTooltip(string text)
    {
        description.text = text;
        gameObject.SetActive(true);
    }

    public void HideTooltip()
    {
        description.text = "";
        gameObject.SetActive(false);
    }



}
