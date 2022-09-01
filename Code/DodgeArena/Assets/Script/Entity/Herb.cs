using System.Collections;
using UnityEngine;

public class Herb : LandScape, IResourceSource {
    [SerializeField]
    private float time;
    [SerializeField]
    private Vector2Int drop;
    private bool _harvesting;
    float IResourceSource.time { get => time; }
    bool IResourceSource.harvesting { get => _harvesting; set => _harvesting = value; }

    public override void OnSpawn() {
        ( (IResourceSource) this ).Enable(trigger);
    }

    bool IResourceSource.CanHarvest(Player player) {
        return true;
    }

    void IResourceSource.GiveResource(Player player) {
        player.AddItem(ItemStack.of(ItemTypeEnum.Herb, Random.instance.RandRange(drop)));
    }

    void IResourceSource.OnStartHarvesting(Player player) {
        return;
    }

    void IResourceSource.OnStopHarvesting(Player player) {
        return;
    }

    void IResourceSource.OnSuccessHarvesting(Player player) {
        Remove();
    }
}