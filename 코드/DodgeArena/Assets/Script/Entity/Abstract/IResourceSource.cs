using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IResourceSource {
    public float time {
        get;
        protected set;
    }
    public List<ItemStack> items {
        get;
        protected set;
    }
    protected bool harvesting {
        get;
        set;
    }

    /// <summary>
    /// OnSpawn에서 한번 실행시켜줘야함
    /// </summary>
    /// <param name="collider"></param>
    public void Enable(SubCollider collider) {
        collider.onTriggerEnter -= OnEnterHarvestingArea;
        collider.onTriggerEnter += OnEnterHarvestingArea;
        collider.onTriggerExit -= OnExitHarvestingArea;
        collider.onTriggerExit += OnExitHarvestingArea;
    }

    protected void OnEnterHarvestingArea(Entity other, Collider2D collider) {
        if(other is Player) {
            harvesting = HUDManager.instance.StartHarvest(time, GiveRandomItem);
        }
    }

    protected void OnExitHarvestingArea(Entity other, Collider2D collider) {
        if(harvesting && other is Player) {
            HUDManager.instance.StopHarvest();
            harvesting = false;
        }
    }

    public void GiveRandomItem() {
        GameManager.instance.player.backpack.AddItems(items);
    }
}