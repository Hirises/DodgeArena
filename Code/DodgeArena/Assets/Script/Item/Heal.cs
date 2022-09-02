using System.Collections;
using UnityEngine;

namespace Assets.Script.Item {
    [CreateAssetMenu(fileName = "Heal", menuName = "Data/ItemFunction/Heal")]
    public class Heal : ItemFuntion {
        [SerializeField]
        public int heal;

        public override bool CanEquip(Equipments.Slot slot, ItemStack itemstack) {
            return Equipments.IsQuickbarSlot(slot);
        }

        public override bool CanUse(ItemStack itemStack) {
            return true;
        }

        public override void OnEquip(Equipments.Slot slot, ItemStack itemStack) {
            return;
        }

        public override void OnUnequip(Equipments.Slot slot, ItemStack itemStack) {
            return;
        }

        public override void OnUse(ItemStack itemStack) {
            GameManager.instance.player.Heal(heal);
            itemStack.OperateAmount(-1);
        }
    }
}