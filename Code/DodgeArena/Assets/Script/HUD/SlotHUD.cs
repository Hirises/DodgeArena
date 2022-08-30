using System.Collections;
using UnityEngine;

public class SlotHUD : MonoBehaviour {
    [SerializeField]
    public ItemHUD innerItemHUD;

    public virtual void UpdateHUD() {
        innerItemHUD.UpdateHUD();
    }
}