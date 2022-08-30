using System.Collections;
using UnityEngine;

/// <summary>
/// 아이템 사용 및 효과 클래스
/// </summary>
public abstract class IItemFuntion : ScriptableObject {
    public abstract bool CanUse(ItemStack item);

    public abstract void OnUse(ItemStack item);

    public abstract bool CanUseOnQuickBar(ItemStack item);

    public abstract void OnUseOnQuickBar(ItemStack item);

    public abstract bool CanEquip(ItemStack item);

    public virtual void OnEquip(ItemStack item) {
        GameManager.instance.player.Equip(item);
    }

    public abstract bool CanUnequip(ItemStack item);

    public virtual void OnUnequip(ItemStack item) {
        GameManager.instance.player.Unequip(item);
    }

    public abstract bool CanDiscard(ItemStack item);

    public virtual void OnDiscard(ItemStack item) {
        GameManager.instance.player.DropItem(item);
        item.Clear();
    }
}