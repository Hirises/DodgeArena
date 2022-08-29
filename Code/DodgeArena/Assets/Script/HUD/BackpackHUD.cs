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
    public List<BackpackSlotHUD> slots;
    [ReadOnly]
    public int currentSlotIndex;

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
    public BackpackSlotHUD infoItemSlot;
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
        currentSlotIndex = -1;
        UpdateChange(container);
    }

    public void Disable() {
        HideInfo();
    }

    public void UpdateChange(Container self) {
        if(slots.Count != container.size) {
            for(int i = 0; i < slotRoot.transform.childCount; i++) {
                Destroy(slotRoot.transform.GetChild(i).gameObject);
            }
            slots.Clear();
            for(int i = 0; i < container.size; i++) {
                BackpackSlotHUD slot = Instantiate(slotPrefab, slotRoot.transform);
                slot.GetComponent<RectTransform>().localPosition = new Vector3(origin.x + ( margin.x * ( i % horizontalCount ) ), origin.y + ( margin.y * ( i / horizontalCount ) ), 0);
                slot.innerItemHUD.itemstack = container[i];
                slot.index = i;
                slots.Add(slot);
                slot.UpdateHUD();
                slot.onClick += UpdateInfo;
                if(currentSlotIndex == i) {
                    UpdateInfo(slots[currentSlotIndex]);
                }
            }
            slotRoot.sizeDelta = new Vector2(0, -1 * (origin.y + ( margin.y * ( ( container.size - 1 ) / horizontalCount + 1 ) )));
        } else {
            for(int i = 0; i < container.size; i++) {
                BackpackSlotHUD slot = slots[i];
                slot.innerItemHUD.itemstack = container[i];
                slot.UpdateHUD();
                if(currentSlotIndex == i) {
                    UpdateInfo(slots[currentSlotIndex]);
                }
            }
        }
        infoItemSlot.innerItemHUD.itemstack = ItemStack.Empty;
        infoItemSlot.UpdateHUD();
    }

    public void UpdateInfo(BackpackSlotHUD slot) {
        if(slot.innerItemHUD.itemstack.IsEmpty()) {
            HideInfo();
        } else {
            currentSlotIndex = slot.index;
            ShowInfo(slot.innerItemHUD.itemstack.Clone());
        }
    }

    public void HideInfo() {
        infoRoot.SetActive(false);
        currentSlotIndex = -1;
        infoItemSlot.innerItemHUD.itemstack = ItemStack.Empty;
    }

    public void ShowInfo(ItemStack item) {
        infoItemSlot.innerItemHUD.itemstack = item;
        infoItemSlot.UpdateHUD();
        infoItemName.text = item.type.name;
        infoItemText.text = item.type.information;
        if(item.type.HasAttribute(ItemAttribute.Equipable) && GameManager.instance.player.HasEmptyEquipmentSlot()) {
            Debug.Log(item);
            if(!GameManager.instance.player.IsEquiped(item)) {
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
        if(currentSlotIndex < 0) {
            return;
        }
        EquipItem(currentSlotIndex);
    }

    public void EquipItem(int slot) {
        Player player = GameManager.instance.player;
        player.Equip(player.backpack[slot]);
        if(currentSlotIndex == slot) {
            Debug.Log(player.backpack[slot]);
            UpdateInfo(slots[currentSlotIndex]);
        }
    }

    public void UnequipButtonClicked() {
        if(currentSlotIndex < 0) {
            return;
        }
        UnequipItem(currentSlotIndex);
    }

    public void UnequipItem(int slot) {
        Player player = GameManager.instance.player;
        player.Unequip(player.backpack[slot]);
        if(currentSlotIndex == slot) {
            UpdateInfo(slots[currentSlotIndex]);
        }
    }

    public void UseButtonClicked() {
        if(currentSlotIndex < 0) {
            return;
        }

    }

    public void DiscardButtonClicked() {
        if(currentSlotIndex < 0) {
            return;
        }
        DiscardItem(currentSlotIndex);
    }

    public void DiscardItem(int slot) {
        Player player = GameManager.instance.player;
        WorldLocation location = player.location;
        location.world.Spawn(( (EntityType) EntityTypeEnum.Item ).prefab, location.Randomize(1.5f), item => ( (Item) item ).itemstack = player.backpack[slot]);
        container[slot] = ItemStack.Empty;
        HideInfo();
        UpdateChange(container);
    }
}
