using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SaveManager : MonoBehaviour
{

    
    public static SaveManager instance;

    private GameData gameData;

    private List<ISaveManager> saveManagers;

    [SerializeField] private string fileName;  
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
    private void DeleteSaveFile()
    {
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, encryptData);
        dataHandler.DeleteData();
    }
    
    private void Start()
    {
        Debug.Log("SaveManager Start" + Application.persistentDataPath);
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
        SaveGame();
    }


    private List<ISaveManager> FindAllSaveManagers()
    {
        IEnumerable<ISaveManager> saveManagers = FindObjectsOfType<MonoBehaviour>(true).OfType<ISaveManager>();
        return saveManagers.ToList();
    }


}
