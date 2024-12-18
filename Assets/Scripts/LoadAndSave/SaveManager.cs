using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SaveManager : MonoBehaviour
{

    
    public static SaveManager instance;

    private GameData gameData;

    private List<ISaveManager> saveManagers;

    [SerializeField] private string fileName = "idbfs/save.sap";  
    [SerializeField] private bool encryptData;
    private FileDataHandler dataHandler;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    
    [ContextMenu("Delete Save File")]
    public void DeleteSaveFile()
    {
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, encryptData);
        dataHandler.DeleteData();
    }
    
    private void Start()
    {
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, encryptData);
        saveManagers = FindAllSaveManagers();

        LoadGame();
    }
    

    public void NewGame()
    {
        gameData = new GameData();
    }

    public void LoadGame()
    {
        gameData = dataHandler.LoadData();

        if (gameData == null)
        {
            NewGame();
        }
        
        saveManagers.ForEach(saveManager => saveManager.LoadData(gameData));
        
    }


    public void SaveGame()
    {
        saveManagers.ForEach(saveManager => saveManager.SaveData(ref gameData));
        dataHandler.SaveData(gameData);
    }

    private void OnApplicationQuit()
    {
        // TODO:
        // SaveGame();
    }


    private List<ISaveManager> FindAllSaveManagers()
    {
        IEnumerable<ISaveManager> curSaveManagers = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<ISaveManager>();
        return curSaveManagers.ToList();
    }


    public bool HasSaveData()
    {
        if (dataHandler.LoadData() != null)
        {
            return true;
        }
        return false;
    }


}
