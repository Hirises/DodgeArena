using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ItemIcon : MonoBehaviour {
    [SerializeField]
    private Image image;
    [SerializeField]
    private TextMeshProUGUI amountText;
    private ItemStack _itemstack;
    public ItemStack itemstack {
        get {
            if(_itemstack == null) {
                _itemstack = ItemStack.Empty;
            }
            return _itemstack;
        }
        set => _itemstack = value;
    }
    public bool showAmount = true;

    public void UpdateHUD() {
        if(itemstack == null) {
            itemstack = ItemStack.Empty;
        }
        image.sprite = itemstack.type.sprite;
        if(showAmount) {
            if(itemstack.IsEmpty() || itemstack.amount <= 1) {
                amountText.text = "";
            } else {
                amountText.text = itemstack.amount.ToString();
            }
        }
    }
}