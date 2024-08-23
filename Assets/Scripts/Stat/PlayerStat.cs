using System.Collections;
using System.Collections.Generic;
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

    public override void Die()
    {
        base.Die();
        player.Die();
        GetComponent<PlayerItemDrop>()?.GenerateDrop();
    }

    protected override void DecreaseHealthBy(int dam)
    {
        base.DecreaseHealthBy(dam);
        ItemDataEquipment curEquipment = Inventory.instance.GetEquipment(EquipmentType.Armor);
        if (curEquipment != null)
        {
            curEquipment.Effect(player.transform);
        }
    }
}
