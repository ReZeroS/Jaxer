using UnityEngine;

public class PlayerStat : CharacterStat
{

    private Player player;
    protected override void Start()
    {
        base.Start();

        player = PlayerManager.instance.player;
    }

    public override void TakeDamage(int dam)
    {
        base.TakeDamage(dam);
    }

    protected override void Die()
    {
        base.Die();
        player.Die();
        GetComponent<PlayerItemDrop>()?.GenerateDrop();
    }

    public override void OnEvasion()
    {
        base.OnEvasion();
        player.skillManager.dodgeSkill.CreateMirageDodge();
    }

    protected override void DecreaseHealthBy(int dam)
    {
        base.DecreaseHealthBy(dam);

        if (dam > GetMaxHealthVal() * 0.3f)
        {
            player.SetKnockBackPower(new Vector2(6, 10));
            player.fx.ScreenShakeForHighDamageImpact(player.facingDir);
            AudioManager.instance.PlaySFX(34);
        }
        
        ItemDataEquipment curEquipment = Inventory.instance.GetEquipment(EquipmentType.Armor);
        if (curEquipment != null)
        {
            curEquipment.Effect(player.transform);
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
