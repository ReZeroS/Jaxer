using UnityEngine;

public class PlayerManager : MonoBehaviour, ISaveManager
{
    public static PlayerManager instance;
    public Player player;
    public int currency;
    
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
        }
        else
        {
            instance = this;
        }
    }



    public bool HaveEnoughCurrency(int amount)
    {
        if (currency >= amount)
        {
            currency -= amount;
            return true;
        }
        return false;
    }


    public void LoadData(GameData gameData)
    {
        currency = gameData.currency;
    }

    public void SaveData(ref GameData gameData)
    {
        gameData.currency = currency;
    }
}