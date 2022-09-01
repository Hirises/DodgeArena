using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : LandScape, IResourceSource
{
    [SerializeField]
    [BoxGroup("ResourceSource")]
    private float time;
    private bool harvesting;
    float IResourceSource.time { get => time; }
    bool IResourceSource.harvesting { get => harvesting; set => harvesting = value; }

    public override void OnSpawn() {
        ( (IResourceSource) this ).Enable(trigger);
    }
    public bool CanHarvest(Player player) {
        return true;
    }

    public void OnStartHarvesting(Player player) {
        return;
    }

    public void OnSuccessHarvesting(Player player) {
        Remove();
    }

    public void OnStopHarvesting(Player player) {
        return;
    }

    public void GiveResource(Player player) {
        GameManager.instance.player.AddItem(ItemStack.of(ItemTypeEnum.Log, 1));
    }
}
