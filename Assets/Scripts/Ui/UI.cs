using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI : MonoBehaviour, ISaveManager
{
    
    [SerializeField] public UIFadeScreen fadeScreen;

    [SerializeField] private GameObject menuHeader;
    [SerializeField] private GameObject backGround;
    [SerializeField] private GameObject characterUI;
    [SerializeField] private GameObject skillTreeUI;
    [SerializeField] private GameObject craftingUI;
    [SerializeField] private GameObject optionsUI;
    [SerializeField] private GameObject inGameUI;
    [SerializeField] private GameObject firstTab;
    
    public UIItemtooltip itemTooltip;
    public UIStatTooltip statTooltip;
    public UISkillTooltip skillTooltip;

    public UICraftWindow craftWindow;

    [SerializeField] private UIVolumeSlider[] volumeSettings;
    
    private int currentMenuIndex; 
    private List<GameObject> menuList = new();
    
    

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
        
        menuList.Add(characterUI);
        menuList.Add(skillTreeUI);
        menuList.Add(craftingUI);
        menuList.Add(optionsUI);
    }

    // Update is called once per frame
    void Update()
    {
        if (InputManager.instance.menuJustPressed)
        {
            if (currentMenuIndex == 0)
            {
                SwitchWithKeysTo(characterUI);
                ActiveBackground(true);
                InputManager.instance.SwitchActionMap(InputManager.InputMapType.UI);
            }
            else
            {
                currentMenuIndex = 0;
                SwitchWithKeysTo(inGameUI);
                ActiveBackground(false);
                InputManager.instance.SwitchActionMap(InputManager.InputMapType.GamePlay);
            }
        }
        
    }

    private void ActiveBackground(bool active)
    {
        menuHeader.gameObject.SetActive(active);
        backGround.gameObject.SetActive(active);
        EventSystem.current.SetSelectedGameObject(firstTab);
    }


    public void SwitchTo(GameObject menu)
    {
        // 关闭所有的菜单，只开当前的菜单
        menuList.ForEach(x => x.SetActive(false));

        if (menu)
        {
            AudioManager.instance?.PlaySFX(7);
            menu.SetActive(true);
        }
        
        GameManager.instance?.PauseGame(menu != inGameUI);
    }

    public void SwitchWithKeysTo(GameObject menu)
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
