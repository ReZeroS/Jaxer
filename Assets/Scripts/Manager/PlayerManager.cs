using ReZeros.Jaxer.Core;

namespace ReZeros.Jaxer.Manager
{
    public class PlayerManager : CoreComponent, ISaveManager
    {
        public static PlayerManager instance;
        public Player player;

        public Player Player
        {
            get => player ?? core.GetCoreComponent(ref player);
        }

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