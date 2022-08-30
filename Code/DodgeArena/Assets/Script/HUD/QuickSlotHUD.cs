using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class QuickSlotHUD : SlotHUD {
    public void OnClick() {
        //클릭
        ItemStack item = innerItemHUD.itemstack;
        if(item.type.itemFuntion == null || !item.type.itemFuntion.CanUseOnQuickBar(item)) {
            return;
        }
        item.type.itemFuntion.OnUseOnQuickBar(item);
        UpdateHUD();
    }
}