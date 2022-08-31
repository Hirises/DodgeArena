using System.Collections;
using UnityEngine;

namespace Assets.Script.Item {
    [CreateAssetMenu(fileName = "Heal", menuName = "Data/ItemFunction/Heal")]
    public class Heal : ItemFuntion {
        [SerializeField]
        public int heal;

        public override bool CanDiscard(ItemStack item) {
            return true;
        }

        public override bool CanEquip(ItemStack item) {
            return true;
        }

        public override bool CanUnequip(ItemStack item) {
            return true;
        }

        public override bool CanUse(ItemStack item) {
            return true;
        }

        public override bool CanUseOnQuickBar(ItemStack item) {
            return true;
        }

        public override void OnUse(ItemStack item) {
            GameManager.instance.player.Heal(heal);
            item.OperateAmount(-1);
        }

        public override void OnUseOnQuickBar(ItemStack item) {
            OnUse(item);
        }
    }
}