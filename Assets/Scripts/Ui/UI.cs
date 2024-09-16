using UnityEngine;

public class UI : MonoBehaviour, ISaveManager
{
    
    [SerializeField] public UIFadeScreen fadeScreen;
    
    
    [SerializeField] private GameObject characterUI;
    [SerializeField] private GameObject skillTreeUI;
    [SerializeField] private GameObject craftingUI;
    [SerializeField] private GameObject optionsUI;
    [SerializeField] private GameObject inGameUI;
    
    public UIItemtooltip itemTooltip;
    public UIStatTooltip statTooltip;
    public UISkillTooltip skillTooltip;

    public UICraftWindow craftWindow;

    [SerializeField] private UIVolumeSlider[] volumeSettings;
    

    private void Awake()
    {
        SwitchTo(skillTreeUI); // we need to assign events on skill tree slots before assign events on skill scripts
        fadeScreen.gameObject.SetActive(true);
    }

    private void Start()
    {
        SwitchTo(inGameUI);
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
            bool fadeScreen = transform.GetChild(i).gameObject.GetComponent<UIFadeScreen>();
            if (!fadeScreen)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        if (menu)
        {
            AudioManager.instance.PlaySFX(7);
            menu.SetActive(true);
        }
        
    }

    public void SwitWithKeysTo(GameObject menu)
    {
        if (menu && menu.activeSelf)
        {
            menu.SetActive(false);
            CheckForInGameUI();
            return;
        }
        SwitchTo(menu);
    }

    private void CheckForInGameUI()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeSelf
                && !transform.GetChild(i).gameObject.GetComponent<UIFadeScreen>())
            {
                return;
            }
        }
        SwitchTo(inGameUI);
    }

    public void LoadData(GameData gameData)
    {
        var savedVolumeSettingMap = gameData.volumeSettings;
        foreach (var savedVolumeSetting in savedVolumeSettingMap)
        {
            foreach (var volumeSlider in volumeSettings)
            {
                if (savedVolumeSetting.Key == volumeSlider.parameter)
                {
                    volumeSlider.SetVolume(savedVolumeSetting.Value);
                }
            }
        }
    }

    public void SaveData(ref GameData gameData)
    {
        gameData.volumeSettings.Clear();
        foreach (var setting in volumeSettings)
        {
            gameData.volumeSettings.Add(setting.parameter, setting.slider.value);
        }
    }
}
