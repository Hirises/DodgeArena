using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class QuickbarSlot : NormalSlot {
    public void OnClick() {
        //클릭
        ItemStack item = itemIcon.itemstack;
        if(item.type.itemFuntion != null && item.type.itemFuntion.CanUse(item)) {
            item.type.itemFuntion.OnUse(item);
            UpdateHUD();
        }
    }
}