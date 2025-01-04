using BehaviorDesigner.Runtime.Tasks;
using ReZeros.Jaxer.Config;
using ReZeros.Jaxer.Manager;
using Sound.SoundManager;

namespace ReZeros.Jaxer.Core.AI.Tasks
{
    
    public class InitBoss : EnemyAction
    {
        private BossConfig bossConfig;
        public override void OnAwake()
        {
            base.OnAwake();
            bossConfig = GetComponent<BossConfig>();
        }

        public override TaskStatus OnUpdate()
        {
            GuiManager.Instance.ShowBossName(bossConfig.bossName);
            //play music 
            MusicManager.Instance.PlayMusic(bossConfig.bossDefaultBgm);
            
            return TaskStatus.Success;
        }
    }
}