using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : LandScape, IResourceSource
{
    [SerializeField]
    [BoxGroup("ResourceSource")]
    protected float time;
    [SerializeField]
    [BoxGroup("ResourceSource")]
    protected List<ItemStack> items;
    protected bool harvesting;
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
        GameManager.instance.player.backpack.AddItems(items);
    }
}
