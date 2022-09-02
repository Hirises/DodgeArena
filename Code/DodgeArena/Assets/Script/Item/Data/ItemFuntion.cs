using System.Collections;
using UnityEngine;

/// <summary>
/// 아이템 사용 및 효과 클래스
/// </summary>
public abstract class ItemFuntion : ScriptableObject {
    public abstract bool CanUse(ItemStack itemStack);

    /// <summary>
    /// 아이템 사용시 호출
    /// 아이템 개수 제거도 직접해야한다
    /// </summary>
    public abstract void OnUse(ItemStack itemStack);

    public abstract bool CanEquip(Equipments.Slot slot, ItemStack itemstack);

    public abstract void OnEquip(Equipments.Slot slot, ItemStack itemStack);

    public abstract void OnUnequip(Equipments.Slot slot, ItemStack itemStack);
}