using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] private GameObject characterUI;
    [SerializeField] private GameObject skillTreeUI;
    [SerializeField] private GameObject craftingUI;
    [SerializeField] private GameObject optionsUI;
    
    public UIItemtooltip itemTooltip;
    public UIStatTooltip statTooltip;
    public UISkillTooltip skillTooltip;

    public UICraftWindow craftWindow;
    
    private void Start()
    {
        SwitchTo(null);
        itemTooltip.gameObject.SetActive(false);
        statTooltip.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitWithKeysTo(characterUI);
        }
        
        
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwitWithKeysTo(skillTreeUI);
        }
        
        
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SwitWithKeysTo(craftingUI);
        }
        
        
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SwitWithKeysTo(optionsUI);
        }
        
        
        
    }


    public void SwitchTo(GameObject menu)
    {
        for (int i = 0; i <  transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        if (menu)
        {
            menu.SetActive(true);
        }
        
    }

    public void SwitWithKeysTo(GameObject menu)
    {
        if (menu && menu.activeSelf)
        {
            menu.SetActive(false);
            return;
        }
        SwitchTo(menu);
    }
    
    
}
