using System;
using System.Collections.Generic;

[Serializable]
public class GameData
{

    public int currency;

    public SerializableDictionary<string, bool> skillTree;
    public SerializableDictionary<string, int> inventory;
    public List<string> equipmentIdList;
    

    public GameData()
    {
        currency = 0;
        skillTree = new SerializableDictionary<string, bool>();
        inventory = new SerializableDictionary<string, int>();
        equipmentIdList = new List<string>();
    }
}