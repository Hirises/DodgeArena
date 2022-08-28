using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SlotHUD : MonoBehaviour {
    [SerializeField]
    public ItemHUD innerItem;

    public void UpdateHUD() {
        innerItem.UpdateHUD();
    }
}