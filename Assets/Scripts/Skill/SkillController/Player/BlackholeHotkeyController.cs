using TMPro;
using UnityEngine;

public class BlackholeHotkeyController : MonoBehaviour
{

    private SpriteRenderer sr;
    private string myHotKey;
    private TextMeshProUGUI myText;

    private Transform myEnemy;
    private BlackholeSkillController blackholeSkillController;


    public void SetUpHotKey(string hotKey, Transform enemy, BlackholeSkillController blackholeController)
    {
        sr = GetComponent<SpriteRenderer>();
        
        myText = GetComponentInChildren<TextMeshProUGUI>();

        myEnemy = enemy;
        blackholeSkillController = blackholeController;
        
        myHotKey = hotKey;
        myText.text = myHotKey;

    }
    
    

    // Update is called once per frame
    void Update()
    {
        if (InputManager.instance.MatchHotKey(myHotKey))
        {
            blackholeSkillController.AddTarget(myEnemy.transform);
            myText.color = Color.clear;
            sr.color = Color.clear;
        }
        
    }
}
