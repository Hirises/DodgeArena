using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : LandScape
{
    [SerializeField]
    [BoxGroup("ResourceSource")]
    public float time;
    [SerializeField]
    [BoxGroup("ResourceSource")]
    public List<ItemStack> items;
    private bool success;

    public override void OnStartTrigger(Entity other, Collider2D collider) {
        if(other is Player) {
            success = HUDManager.instance.StartHarvest(time, GiveRandomItem);
        }
    }

    public override void OnEndTrigger(Entity other, Collider2D collider) {
        if(success && other is Player) {
            HUDManager.instance.StopHarvest();
            success = false;
        }
    }

    public void GiveRandomItem() {
        GameManager.instance.player.backpack.AddItems(items);
    }
}
