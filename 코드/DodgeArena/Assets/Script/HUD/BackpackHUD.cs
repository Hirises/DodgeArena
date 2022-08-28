using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class BackpackHUD : MonoBehaviour
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
    [HideInInspector]
    public List<SlotHUD> slots;

    public void Init() {
        List<ItemStack> items = GameManager.instance.player.backpack.content;
        for(int i = 0; i < items.Count; i++) {
            SlotHUD slot = Instantiate(slotPrefab, content.transform);
            slot.GetComponent<RectTransform>().localPosition = new Vector3(origin.x + ( margin.x * ( i % horizontalCount ) ), origin.y + ( margin.y * ( i / horizontalCount ) ), 0);
            slot.innerItem.itemstack = items[i];
            slot.UpdateHUD();
        }
    }

    public void Resize() {
        for(int i = 0; i < content.transform.childCount; i++) {
            Destroy(content.transform.GetChild(i).gameObject);
        }
    }
}
