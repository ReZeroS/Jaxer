using System;
using System.Collections.Generic;

[Serializable]
public class GameData
{

    public int currency;

    public SerializableDictionary<string, bool> skillTree;
    public SerializableDictionary<string, int> inventory;
    public List<string> equipmentIdList;
    

    public SerializableDictionary<string, bool> checkpoint;
    public string closestCheckpointId;


    public SerializableDictionary<string, float> volumeSettings;
    
    
    public GameData()
    {
        currency = 0;
        skillTree = new SerializableDictionary<string, bool>();
        inventory = new SerializableDictionary<string, int>();
        equipmentIdList = new List<string>();
        
        closestCheckpointId = string.Empty;
        checkpoint = new SerializableDictionary<string, bool>();
        
        volumeSettings = new SerializableDictionary<string, float>();
    }
}