using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemHUD : MonoBehaviour {
    [SerializeField]
    public Image image;
    [SerializeField]
    public TextMeshProUGUI amount;
    public ItemStack itemstack;

    public void UpdateHUD() {
        if(itemstack == null) {
            itemstack = ItemStack.Empty;
        }
        image.sprite = itemstack.type.GetSprite("dict");
        if(itemstack.IsEmpty() || itemstack.amount <= 1) {
            amount.text = "";
        } else {
            amount.text = itemstack.amount.ToString();
        }
    }
}