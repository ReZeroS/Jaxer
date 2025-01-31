using ReZeros.Jaxer.Manager;
using ReZeros.Jaxer.PlayerBase;
using Sound.SoundManager;
using UnityEngine;

public class PlayerStat : CharacterStat
{

    private MainPlayer mainPlayer;
    protected override void Start()
    {
        base.Start();

        mainPlayer = PlayerManager.instance.Player;
    }

    public override void TakeDamage(int dam)
    {
        base.TakeDamage(dam);
    }

    protected override void Die()
    {
        base.Die();
        mainPlayer.Die();
        GetComponent<PlayerItemDrop>()?.GenerateDrop();
    }

    public override void OnEvasion()
    {
        base.OnEvasion();
        mainPlayer.skillManager.dodgeSkill.CreateMirageDodge();
    }

    protected override void DecreaseHealthBy(int dam)
    {
        base.DecreaseHealthBy(dam);

        if (dam > GetMaxHealthVal() * 0.3f)
        {
            mainPlayer.SetKnockBackPower(new Vector2(6, 10));
            mainPlayer.fx.ScreenShakeForHighDamageImpact(mainPlayer.facingDir);
            // 暴击重声
            SoundManager.PlaySound(SoundType.HEAVY_ATTACK);
        }
        
        ItemDataEquipment curEquipment = Inventory.instance.GetEquipment(EquipmentType.Armor);
        if (curEquipment != null)
        {
            curEquipment.Effect(mainPlayer.transform);
        }
    }


    public void CloneDoDamage(CharacterStat targetsStat, float attackMultiplier)
    {
        if (TargetCanAvoidAttack(targetsStat)) return;


        int totalDamage = damage.GetValue() + strength.GetValue();

        if (attackMultiplier > 0)
        {
            totalDamage = (int)(totalDamage * attackMultiplier);
        }
        
        if (CanCrit())
        {
            totalDamage = CalculateCriticalDamage(totalDamage);
        }


        totalDamage = CheckTargetArmor(targetsStat, totalDamage);
        targetsStat.TakeDamage(totalDamage);

        DoMagicalDamage(targetsStat); // remove if you do not want to do magical damage on primary attack
    }
    
    
    
    
    
}
