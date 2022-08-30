using System.Collections;
using UnityEngine;

public class SlotHUD : MonoBehaviour {
    [SerializeField]
    public ItemHUD innerItemHUD;

    private void Awake() {
        if(innerItemHUD.itemstack == null) {
            innerItemHUD.itemstack = ItemStack.Empty;
        }
    }

    public virtual void UpdateHUD() {
        innerItemHUD.UpdateHUD();
    }
}