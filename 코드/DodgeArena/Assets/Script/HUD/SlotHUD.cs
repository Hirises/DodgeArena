using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SlotHUD : MonoBehaviour {
    [SerializeField]
    public ItemHUD innerItem;
    [SerializeField]
    public bool canAdd;
    [SerializeField]
    public bool canRemove;
    public Action<ItemStack, bool> itemPredicate;

    public void UpdateHUD() {
        innerItem.UpdateHUD();
    }
}