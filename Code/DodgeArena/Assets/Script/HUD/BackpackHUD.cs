using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using NaughtyAttributes;

public class BackpackHUD : MonoBehaviour {
    [HideInInspector]
    public Container container;
    [HideInInspector]
    public List<SlotHUD> slots;

    [SerializeField]
    [BoxGroup("Inventory")]
    public RectTransform slotRoot;
    [SerializeField]
    [BoxGroup("Inventory")]
    public SlotHUD slotPrefab;
    [SerializeField]
    [BoxGroup("Inventory")]
    public Vector2 origin;
    [SerializeField]
    [BoxGroup("Inventory")]
    public Vector2 margin;
    [SerializeField]
    [BoxGroup("Inventory")]
    public int horizontalCount;

    [SerializeField]
    [BoxGroup("Information")]
    public GameObject infoRoot;
    [SerializeField]
    [BoxGroup("Information")]
    public SlotHUD infoItemSlot;
    [SerializeField]
    [BoxGroup("Information")]
    public TextMeshProUGUI infoItemName;
    [SerializeField]
    [BoxGroup("Information")]
    public TextMeshProUGUI infoItemText;
    [SerializeField]
    [BoxGroup("Information")]
    public GameObject equipButton;
    [SerializeField]
    [BoxGroup("Information")]
    public GameObject unequipButton;
    [SerializeField]
    [BoxGroup("Information")]
    public GameObject useButton;

    public void Enable() {
        this.container = GameManager.instance.player.backpack;
        container.changeEvent -= UpdateChange;
        container.changeEvent += UpdateChange;
        UpdateChange(container);
    }

    public void Disable() {
        HideInfo();
    }

    public void UpdateChange(Container call) {
        if(slots.Count != call.size) {
            for(int i = 0; i < slotRoot.transform.childCount; i++) {
                Destroy(slotRoot.transform.GetChild(i).gameObject);
            }
            slots.Clear();
            for(int i = 0; i < container.size; i++) {
                SlotHUD slot = Instantiate(slotPrefab, slotRoot.transform);
                slot.GetComponent<RectTransform>().localPosition = new Vector3(origin.x + ( margin.x * ( i % horizontalCount ) ), origin.y + ( margin.y * ( i / horizontalCount ) ), 0);
                slot.innerItem.itemstack = container[i];
                slots.Add(slot);
                slot.UpdateHUD();
                slot.onClick += UpdateInfo;
            }
            slotRoot.sizeDelta = new Vector2(0, -1 * (origin.y + ( margin.y * ( ( container.size - 1 ) / horizontalCount + 1 ) )));
        } else {
            for(int i = 0; i < container.size; i++) {
                SlotHUD slot = slots[i];
                slot.innerItem.itemstack = container[i];
                slot.UpdateHUD();
            }
        }
        infoItemSlot.innerItem.itemstack = ItemStack.Empty;
        infoItemSlot.UpdateHUD();
    }

    public void UpdateInfo(SlotHUD slot) {
        if(slot.innerItem.itemstack.IsEmpty()) {
            HideInfo();
        } else {
            ShowInfo(slot.innerItem.itemstack.Clone());
        }
    }

    public void HideInfo() {
        infoRoot.SetActive(false);
        infoItemSlot.innerItem.itemstack = ItemStack.Empty;
    }

    public void ShowInfo(ItemStack item) {
        infoItemSlot.innerItem.itemstack = item;
        infoItemSlot.UpdateHUD();
        infoItemName.text = item.type.name;
        infoItemText.text = item.type.information;
        if(item.type.HasAttribute(ItemAttribute.Equipable)) {
            if(item.HasTag(ItemTag.Equiped)) {
                unequipButton.SetActive(true);
                equipButton.SetActive(false);
            } else {
                equipButton.SetActive(true);
                unequipButton.SetActive(false);
            }
        } else {
            equipButton.SetActive(false);
            unequipButton.SetActive(false);
        }
        if(item.type.HasAttribute(ItemAttribute.Useable)) {
            useButton.SetActive(true);
        } else {
            useButton.SetActive(false);
        }
        infoRoot.SetActive(true);
    }

    public void EquipButtonClicked() {

    }

    public void UnequipButtonClicked() {

    }

    public void UseButtonClicked() {

    }

    public void DiscardButtonClicked() {

    }
}
