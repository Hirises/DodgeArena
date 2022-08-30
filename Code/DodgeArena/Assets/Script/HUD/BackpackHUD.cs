using NaughtyAttributes;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.Progress;

public class BackpackHUD : MonoBehaviour {
    [HideInInspector]
    public Container container;
    [HideInInspector]
    public List<BackpackSlotHUD> slots;
    [HideInInspector]
    public BackpackSlotHUD selectedSlot;

    [SerializeField]
    [BoxGroup("Inventory")]
    public RectTransform slotRoot;
    [SerializeField]
    [BoxGroup("Inventory")]
    public BackpackSlotHUD slotPrefab;
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
    [SerializeField]
    [BoxGroup("Information")]
    public GameObject discardButton;

    public void Enable() {
        this.container = GameManager.instance.player.backpack;
        selectedSlot = null;
        container.changeEvent -= UpdateChange;
        container.changeEvent += UpdateChange;
        UpdateChange();
        infoItemSlot.innerItemHUD.itemstack = ItemStack.Empty;
        infoItemSlot.UpdateHUD();
    }

    public void Disable() {
        HideInfo();
        container.changeEvent -= UpdateChange;
    }

    public void UpdateChange(Container self) {
        UpdateChange();
    }

    public void UpdateChange() {
        if(slots.Count != container.size) {
            for(int i = 0; i < slotRoot.transform.childCount; i++) {
                Destroy(slotRoot.transform.GetChild(i).gameObject);
            }
            slots.Clear();
            for(int i = 0; i < container.size; i++) {
                BackpackSlotHUD slot = Instantiate(slotPrefab, slotRoot.transform);
                slot.GetComponent<RectTransform>().localPosition = new Vector3(origin.x + ( margin.x * ( i % horizontalCount ) ), origin.y + ( margin.y * ( i / horizontalCount ) ), 0);
                slot.innerItemHUD.itemstack = container[i];
                slots.Add(slot);
                slot.UpdateHUD();
                slot.onClick += OnClickSlot;
                if(slot.innerItemHUD.itemstack == infoItemSlot.innerItemHUD.itemstack) {
                    UpdateInfo(slot.innerItemHUD.itemstack);
                    slot.Select();
                } else {
                    slot.Unselect();
                }
            }
            slotRoot.sizeDelta = new Vector2(0, -1 * ( origin.y + ( margin.y * ( ( container.size - 1 ) / horizontalCount + 1 ) ) ));
        } else {
            for(int i = 0; i < container.size; i++) {
                BackpackSlotHUD slot = slots[i];
                slot.innerItemHUD.itemstack = container[i];
                slot.UpdateHUD();
                if(slot.innerItemHUD.itemstack == infoItemSlot.innerItemHUD.itemstack) {
                    UpdateInfo(slot.innerItemHUD.itemstack);
                    slot.Select();
                } else {
                    slot.Unselect();
                }
            }
        }
    }

    public void OnClickSlot(BackpackSlotHUD clickedSlot) {
        UpdateInfo(clickedSlot.innerItemHUD.itemstack);
        if(selectedSlot != null) {
            selectedSlot.Unselect();
        }
        clickedSlot.Select();
        selectedSlot = clickedSlot;
    }

    public void UpdateInfo(ItemStack targetItem) {
        if(targetItem.IsEmpty()) {
            HideInfo();
        } else {
            ShowInfo(targetItem);
        }
    }

    public void HideInfo() {
        infoRoot.SetActive(false);
        infoItemSlot.innerItemHUD.itemstack = ItemStack.Empty;
    }

    public void ShowInfo(ItemStack item) {
        infoItemSlot.innerItemHUD.itemstack = item;
        infoItemSlot.UpdateHUD();
        infoItemName.text = item.type.name;
        infoItemText.text = item.type.information;
        if(item.type.HasAttribute(ItemAttribute.Equipable) && GameManager.instance.player.HasEmptyEquipmentSlot() && (item.type.itemFuntion?.CanEquip(item) ?? true)) {
            if(GameManager.instance.player.GetEquipedSlot(item) >= 0) {
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
        if(item.type.HasAttribute(ItemAttribute.Useable) && ( item.type.itemFuntion?.CanUse(item) ?? true )) {
            useButton.SetActive(true);
        } else {
            useButton.SetActive(false);
        }
        if(( item.type.itemFuntion?.CanDiscard(item) ?? true )) {
            discardButton.SetActive(true);
        } else {
            discardButton.SetActive(false);
        }
        infoRoot.SetActive(true);
    }

    public void EquipButtonClicked() {
        Player player = GameManager.instance.player;
        ItemStack item = infoItemSlot.innerItemHUD.itemstack;
        if(item.type.itemFuntion == null) {
            player.Equip(item);
        } else {
            item.type.itemFuntion.OnEquip(item);
        }
        UpdateInfo(item);
        for(int i = 0; i < container.size; i++) {
            BackpackSlotHUD slot = slots[i];
            slot.UpdateHUD();
        }
    }

    public void UnequipButtonClicked() {
        Player player = GameManager.instance.player;
        ItemStack item = infoItemSlot.innerItemHUD.itemstack;
        if(item.type.itemFuntion == null) {
            player.Unequip(item);
        } else {
            item.type.itemFuntion.OnUnequip(item);
        }
        UpdateInfo(item);
        for(int i = 0; i < container.size; i++) {
            BackpackSlotHUD slot = slots[i];
            slot.UpdateHUD();
        }
    }

    public void UseButtonClicked() {
        ItemStack item = infoItemSlot.innerItemHUD.itemstack;
        item.type.itemFuntion?.OnUse(item);
        UpdateChange();
        HUDManager.instance.UpdateQuickBar();
    }

    public void DiscardButtonClicked() {
        Player player = GameManager.instance.player;
        ItemStack targetItem = infoItemSlot.innerItemHUD.itemstack;
        if(targetItem.type.itemFuntion == null) {
            player.DropItem(targetItem);
            player.backpack.RemoveItemRestrict(targetItem);
            HideInfo();
        } else {
            targetItem.type.itemFuntion.OnDiscard(targetItem);
            UpdateInfo(targetItem);
        }
        UpdateChange();
    }
}
