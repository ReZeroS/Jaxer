using UnityEngine;
using UnityEngine.UI;

public class ParrySkill : Skill
{
   [SerializeField] private UISkillTreeSlot parryUnlockedButton;
   public bool parryUnlocked { get; private set; }
   
   
   [SerializeField] private UISkillTreeSlot parryRestoreUnlockedButton;
   public bool parryRestoreUnlocked { get; private set; }
   [Range(0, 1)]
   [SerializeField] private float restorePercentage;
   
   [SerializeField] private UISkillTreeSlot parryWithMirageUnlockedButton;
   public bool parryWithMirageUnlocked { get; private set; }


   protected override void Start()
   {
      base.Start();
      parryUnlockedButton.GetComponent<Button>().onClick.AddListener(UnlockParry);
      parryRestoreUnlockedButton.GetComponent<Button>().onClick.AddListener(UnlockParryRestore);
      parryWithMirageUnlockedButton.GetComponent<Button>().onClick.AddListener(UnlockParryWithMirage);
   }


   public override void UseSkill()
   {
      base.UseSkill();

      if (parryRestoreUnlocked)
      {
         var restoreHealth = restorePercentage * player.stat.GetMaxHealthVal();
         player.stat.IncreaseHealthBy((int)restoreHealth);
      }
      
   }

   public void UnlockParry()
   {
      if (parryUnlockedButton.unlocked)
      {
         parryUnlocked = true;
      }
   }
   public void UnlockParryRestore()
   {
      if (parryRestoreUnlockedButton.unlocked)
      {
         parryRestoreUnlocked = true;
      }
   }

   public void UnlockParryWithMirage()
   {
      if (parryWithMirageUnlockedButton.unlocked)
      {
         parryWithMirageUnlocked = true;
      }
   }


   public void MakeMirageOnParry(Transform respawnPostion)
   {
      SkillManager.instance.cloneSkill.CreateCloneWithDelay(respawnPostion);
      
   }
   


}
