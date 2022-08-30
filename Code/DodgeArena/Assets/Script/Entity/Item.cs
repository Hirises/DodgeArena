using System.Collections;
using UnityEngine;

/// <summary>
/// 필드에 떨어진 아이템
/// </summary>
public class Item : Entity, IResourceSource {
    [SerializeField]
    public float _time;
    private bool _harvesting;
    [SerializeField]
    public ItemStack itemstack;

    float IResourceSource.time { get => _time; set => _time = value; }
    bool IResourceSource.harvesting { get => _harvesting; set => _harvesting = value; }

    public override void OnSpawn() {
        ( (IResourceSource) this ).Enable(trigger);
        spriteRenderer.sprite = itemstack.type.sprite;
    }

    public bool CanHarvest(Player player) {
        return true;
    }

    public void GiveResource(Player player) {
        Debug.Log("give " + itemstack);
        player.backpack.AddItem(itemstack);
    }

    public void OnStartHarvesting(Player player) {
        return;
    }

    public void OnStopHarvesting(Player player) {
        return;
    }

    public void OnSuccessHarvesting(Player player) {
        Remove();
    }
}