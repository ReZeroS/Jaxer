using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BlackholeHotkeyController : MonoBehaviour
{

    private SpriteRenderer sr;
    private KeyCode myHotKey;
    private TextMeshProUGUI myText;

    private Transform myEnemy;
    private BlackholeSkillController blackholeSkillController;


    public void SetUpHotKey(KeyCode hotKey, Transform enemy, BlackholeSkillController blackholeController)
    {
        sr = GetComponent<SpriteRenderer>();
        
        myText = GetComponentInChildren<TextMeshProUGUI>();

        myEnemy = enemy;
        blackholeSkillController = blackholeController;
        
        myHotKey = hotKey;
        myText.text = myHotKey.ToString();

    }
    
    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(myHotKey))
        {
            blackholeSkillController.AddTarget(myEnemy.transform);
            myText.color = Color.clear;
            sr.color = Color.clear;
        }
        
    }
}
