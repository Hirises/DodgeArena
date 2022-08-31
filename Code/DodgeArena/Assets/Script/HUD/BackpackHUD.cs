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
    public GameObject useButton;
    [SerializeField]
    [BoxGroup("Information")]
    public GameObject discardButton;

    /// <summary>
    /// 초기화
    /// </summary>
    public void Enable() {
        if(this.container == null) {
            this.container = GameManager.instance.player.backpack;
        }
        if(selectedSlot != null) {
            UnselectSlot();
        }
        container.changeEvent -= UpdateChange;
        container.changeEvent += UpdateChange;
        UpdateChange();
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 숨기기
    /// </summary>
    public void Disable() {
        gameObject.SetActive(false);
        if(selectedSlot != null) {
            UnselectSlot();
        }
        container.changeEvent -= UpdateChange;
    }

    /// <summary>
    /// 인벤토리의 변경사항 반영
    /// </summary>
    /// <param name="self">이벤트에 등록하기 위해 파리미터 조건을 맞출려고 들어감</param>
    public void UpdateChange(Container self) {
        UpdateChange();
    }

    /// <summary>
    /// 인벤토리의 변경사항 반영
    /// </summary>
    public void UpdateChange() {
        if(slots.Count != container.size) { //리사이즈
            //기존 슬롯 제거
            for(int i = 0; i < slotRoot.transform.childCount; i++) {
                Destroy(slotRoot.transform.GetChild(i).gameObject);
            }
            slots.Clear();

            //슬롯 초기화
            slotRoot.sizeDelta = new Vector2(0, -1 * ( origin.y + ( margin.y * ( ( container.size - 1 ) / horizontalCount + 1 ) ) ));
            for(int i = 0; i < container.size; i++) {
                BackpackSlotHUD slot = Instantiate(slotPrefab, slotRoot.transform);
                slot.GetComponent<RectTransform>().localPosition = new Vector3(origin.x + ( margin.x * ( i % horizontalCount ) ), origin.y + ( margin.y * ( i / horizontalCount ) ), 0);
                slot.innerItemHUD.itemstack = container[i];
                slots.Add(slot);
                slot.UpdateHUD();
                slot.onClick += OnClickSlot;
            }
        } else {
            //슬롯 업데이트
            for(int i = 0; i < container.size; i++) {
                BackpackSlotHUD slot = slots[i];
                slot.innerItemHUD.itemstack = container[i];
                slot.UpdateHUD();
            }
        }
        //팝업창 업데이트
        UpdateInfo();
    }

    /// <summary>
    /// 슬롯 클릭시
    /// </summary>
    /// <param name="clickedSlot">클릭된 슬롯</param>
    public void OnClickSlot(BackpackSlotHUD clickedSlot) {
        if(selectedSlot == clickedSlot) {
            //이미 선택되었었다면 취소
            UnselectSlot();
        } else {
            //선택되지 않았었다면 선택

            //이전에 선택된 슬롯 제거
            if(selectedSlot != null) {
                UnselectSlot();
            }
            SelectSlot(clickedSlot);
        }
    }

    /// <summary>
    /// 슬롯 선택
    /// </summary>
    /// <param name="slot">대상 슬롯</param>
    public void SelectSlot(BackpackSlotHUD slot) {
        ShowInfo(slot.innerItemHUD.itemstack);
        slot.OnSelect();
        selectedSlot = slot;
    }

    /// <summary>
    /// 현재 슬롯 선택 취소
    /// </summary>
    public void UnselectSlot() {
        HideInfo();
        selectedSlot.OnUnselect();
        selectedSlot = null;
    }

    /// <summary>
    /// 아이템 정보창 띄우기
    /// </summary>
    /// <param name="item">대상 아이템</param>
    public void ShowInfo(ItemStack item) {
        infoItemSlot.innerItemHUD.itemstack = item;

        UpdateInfo();

        infoRoot.SetActive(true);
    }

    /// <summary>
    /// 아이템 정보창 업데이트
    /// </summary>
    public void UpdateInfo() {
        if(!infoRoot.activeSelf) {
            return;
        }
        ItemStack item = infoItemSlot.innerItemHUD.itemstack;
        if(item.IsEmpty()) {
            HideInfo();
            return;
        }

        infoItemSlot.UpdateHUD();
        infoItemName.text = item.type.name;
        infoItemText.text = item.type.information;

        //TODO ItemFunction 수정
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
    }

    /// <summary>
    /// 아이템 정보창 숨기기
    /// </summary>
    public void HideInfo() {
        if(!infoRoot.activeSelf) {
            return;
        }
        infoRoot.SetActive(false);
        infoItemSlot.innerItemHUD.itemstack = ItemStack.Empty;
        infoItemSlot.UpdateHUD();
    }

    /// <summary>
    /// 아이템 사용 버튼 클릭시
    /// </summary>
    public void UseButtonClicked() {
        ItemStack item = infoItemSlot.innerItemHUD.itemstack;
        item.type.itemFuntion?.OnUse(item);
        UpdateChange();
        HUDManager.instance.UpdateQuickBar();
    }

    /// <summary>
    /// 아이템 버리기 버튼 클릭시
    /// </summary>
    public void DiscardButtonClicked() {
        //TODO 버리기 로직 수정

        Player player = GameManager.instance.player;
        ItemStack targetItem = infoItemSlot.innerItemHUD.itemstack;
        if(targetItem.type.itemFuntion == null) {
            player.DropItem(targetItem);
            player.backpack.RemoveItemRestrict(targetItem);
            HideInfo();
        } else {
            targetItem.type.itemFuntion.OnDiscard(targetItem);
            UpdateInfo();
        }
        UpdateChange();
    }
}
