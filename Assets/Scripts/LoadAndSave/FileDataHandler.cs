using System;
using System.IO;
using UnityEngine;

public class FileDataHandler
{
    private string dataDirParth = "";
    private string dataFileName = "";
    
    private bool encryptionEnabled = false;
    private string encryptionKey = "jaxer0";

    public FileDataHandler(string dataDirParth, string dataFileName, bool encryptionEnabled)
    {
        this.dataDirParth = dataDirParth;
        this.dataFileName = dataFileName;
        this.encryptionEnabled = encryptionEnabled;
    }

    public void SaveData(GameData data)
    {
        string fullPath = Path.Combine(dataDirParth, dataFileName);

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            string dataToStore = JsonUtility.ToJson(data, true);
            if (encryptionEnabled)
            {
                dataToStore = EncryptDecrypt(dataToStore);
            }
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter streamWriter = new StreamWriter(stream))
                {
                    streamWriter.Write(dataToStore);
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log("Error while saving data: " + fullPath + "\n" + e.Message);
        }
    }


    public GameData LoadData()
    {
        string fullPath = Path.Combine(dataDirParth, dataFileName);
        GameData gameData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad;
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader streamReader = new StreamReader(stream))
                    {
                        dataToLoad = streamReader.ReadToEnd();
                    }
                }

                if (encryptionEnabled)
                {
                    dataToLoad = EncryptDecrypt(dataToLoad);
                }
                gameData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.Log("Error while loading data: " + fullPath + "\n" + e.Message);
            }
        }
        return gameData;
    }


    public void DeleteData()
    {
        string fullPath = Path.Combine(dataDirParth, dataFileName);
        if (File.Exists(fullPath))
        {
            try
            {
                File.Delete(fullPath);
            }
            catch (Exception e)
            {
                Debug.Log("Error while deleting data: " + fullPath + "\n" + e.Message);
            }
        }
    }


    public string EncryptDecrypt(string dataToEncrypt)
    {
        string modifiedData = "";
        for (int i = 0; i < dataToEncrypt.Length; i++)
        {
            char c = dataToEncrypt[i];
            int keyAsciiValue = encryptionKey[i % encryptionKey.Length];
            modifiedData += (char)(c ^ keyAsciiValue);
        }
        return modifiedData;
    }
    
    
    
}