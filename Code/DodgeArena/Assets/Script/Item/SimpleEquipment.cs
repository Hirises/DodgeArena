using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName ="SimpleEquipment", menuName ="Data/ItemFunction/SimmpleEquipment")]
public class SimpleEquipment : ItemFuntion {
    [SerializeField]
    Equipments.Slot targetSlot;

    public override bool CanEquip(Equipments.Slot slot, ItemStack itemstack) {
        return Equipments.IsSimilarSlot(targetSlot, slot);
    }

    public override bool CanUse(ItemStack itemStack) {
        return false;
    }

    public override void OnEquip(Equipments.Slot slot, ItemStack itemStack) {
        return;
    }

    public override void OnUnequip(Equipments.Slot slot, ItemStack itemStack) {
        return;
    }

    public override void OnUse(ItemStack itemStack) {
        return;
    }
}