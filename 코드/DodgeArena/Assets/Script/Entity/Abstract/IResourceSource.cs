using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IResourceSource {
    public float time {
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
        if(other is Player player) {
            if(CanHarvest(player)) {
                OnStartHarvesting();
                harvesting = HUDManager.instance.StartHarvest(time, EndHarveting);
            }
        }
    }

    protected void OnExitHarvestingArea(Entity other, Collider2D collider) {
        if(harvesting && other is Player) {
            HUDManager.instance.StopHarvest();
            harvesting = false;
            OnStopHarvesting();
        }
    }

    protected void EndHarveting() {
        GiveRandomItem();
        OnSuccessHarvesting();
    }

    public void GiveRandomItem();

    public bool CanHarvest(Player player);

    public void OnStartHarvesting();

    public void OnSuccessHarvesting();

    public void OnStopHarvesting();
}