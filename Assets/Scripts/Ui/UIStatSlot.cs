using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIStatSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    private UI ui;
    
    [SerializeField] private string statName;
    [SerializeField] private StatType statType;
    [SerializeField] private TextMeshProUGUI statValueText;
    [SerializeField] private TextMeshProUGUI statNameText;

    [TextArea]
    [SerializeField] private string statDescription;

    
    private void OnValidate()
    {
        gameObject.name = "Stat - " + statName;

        if (statNameText != null)
        {
            statNameText.text = statName;
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateStatValueUI();

        ui = GetComponentInParent<UI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateStatValueUI()
    {
        PlayerStat playerStat = PlayerManager.instance.player.GetComponent<PlayerStat>();
        if (playerStat)
        {
            statValueText.text = playerStat.StatOfType(statType).GetValue().ToString();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.statTooltip.ShowStatTooltip(statDescription, eventData.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.statTooltip.HideTooltip();
    }
}
