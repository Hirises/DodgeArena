using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;

public class InventoryHUD : MonoBehaviour
{
    [SerializeField]
    public GameObject content;
    [SerializeField]
    public SlotHUD slotPrefab;
    [SerializeField]
    public Vector2 origin;
    [SerializeField]
    public Vector2 margin;
    [SerializeField]
    public int horizontalCount;
    [SerializeField]
    public SlotHUD info;
    [SerializeField]
    public ScrollRect scroll;
    [HideInInspector]
    public List<SlotHUD> slots;
    [HideInInspector]
    public Container container;

    public void Init(Container container) {
        this.container = container;
        container.changeEvent -= UpdateChange;
        container.changeEvent += UpdateChange;
        UpdateChange(container);
    }

    public void UpdateChange(Container call) {
        if(slots.Count != call.size) {
            for(int i = 0; i < content.transform.childCount; i++) {
                Destroy(content.transform.GetChild(i).gameObject);
            }
            slots.Clear();
            for(int i = 0; i < container.size; i++) {
                SlotHUD slot = Instantiate(slotPrefab, content.transform);
                slot.GetComponent<RectTransform>().localPosition = new Vector3(origin.x + ( margin.x * ( i % horizontalCount ) ), origin.y + ( margin.y * ( i / horizontalCount ) ), 0);
                slot.innerItem.itemstack = container[i];
                slots.Add(slot);
                slot.UpdateHUD();
                slot.onPointerDown += DisableDrag;
                slot.onPointerUp += EnableDrag;
                slot.onClick += ShowInfo;
            }
        } else {
            for(int i = 0; i < container.size; i++) {
                SlotHUD slot = slots[i];
                slot.innerItem.itemstack = container[i];
                slot.UpdateHUD();
            }
        }
        info.innerItem.itemstack = ItemStack.Empty;
        info.UpdateHUD();
    }

    private void DisableDrag(SlotHUD slot) {
        scroll.enabled = false;
    }

    private void EnableDrag(SlotHUD slot) {
        scroll.enabled = true;
    }

    private void ShowInfo(SlotHUD slot) {
        if(!slot.innerItem.itemstack.IsEmpty()) {
            info.innerItem.itemstack = slot.innerItem.itemstack;
            info.UpdateHUD();
        }
    }
}
