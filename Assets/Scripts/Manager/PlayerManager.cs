using ReZeros.Jaxer.Core;
using ReZeros.Jaxer.PlayerBase;
using UnityEngine.Serialization;

namespace ReZeros.Jaxer.Manager
{
    public class PlayerManager : CoreComponent, ISaveManager
    {
        public static PlayerManager instance;
        // private MainPlayer player;

        public MainPlayer Player;

        public int currency;

        protected override void Awake()
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
}