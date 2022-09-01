using System.Collections;
using UnityEngine;

public class NormalSlot : MonoBehaviour {
    [SerializeField]
    private ItemIcon _itemIcon;
    public ItemIcon itemIcon {
        get => _itemIcon;
    }
    public ItemStack itemstack {
        get => _itemIcon.itemstack;
        set => _itemIcon.itemstack = value;
    }

    /// <summary>
    /// 슬롯 내부 HUD를 업데이트 합니다
    /// </summary>
    public virtual void UpdateHUD() {
        itemIcon.UpdateHUD();
    }
}