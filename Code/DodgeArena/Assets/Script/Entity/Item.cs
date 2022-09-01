using System.Collections;
using UnityEngine;

/// <summary>
/// 필드에 떨어진 아이템
/// </summary>
public class Item : Entity, IResourceSource {
    [SerializeField]
    private float _time;
    private bool _harvesting;
    [SerializeField]
    private ItemStack _itemstack;
    public ItemStack itemstack {
        get => _itemstack;
        set {
            _itemstack = value;
            if(spriteRenderer != null) {
                spriteRenderer.sprite = _itemstack.type.sprite;
            }
        }
    }

    float IResourceSource.time { get => _time; }
    bool IResourceSource.harvesting { get => _harvesting; set => _harvesting = value; }

    public override void OnSpawn() {
        ( (IResourceSource) this ).Enable(trigger);
        spriteRenderer.sprite = itemstack.type.sprite;
    }

    public bool CanHarvest(Player player) {
        return true;
    }

    public void GiveResource(Player player) {
        player.AddItem(itemstack);
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