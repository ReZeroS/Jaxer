using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIStatSlot : MonoBehaviour
{

    [SerializeField] private string statName;
    [SerializeField] private StatType statType;
    [SerializeField] private TextMeshProUGUI statValueText;
    [SerializeField] private TextMeshProUGUI statNameText;


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
    
    
}
