using NaughtyAttributes;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;
using System;
using UnityEngine.UI;
using static Equipments;

public class BackpackUI : MonoBehaviour {
    private Container container;
    private Equipments equipments;
    private List<BackpackSlot> slots = new List<BackpackSlot>();
    private BackpackSlot selectedSlot;
    private BackpackSlot dragSlot;
    private BackpackSlot dragTargetSlot;
    private bool runDrag = false;

    [SerializeField]
    [BoxGroup("Inventory")]
    private RectTransform canvas;
    [SerializeField]
    [BoxGroup("Inventory")]
    private ScrollRect scroll;
    [SerializeField]
    [BoxGroup("Inventory")]
    private ItemIcon dragItem;
    [SerializeField]
    [BoxGroup("Inventory")]
    private RectTransform dragItemRect;
    [SerializeField]
    [BoxGroup("Inventory")]
    private RectTransform slotRoot;
    [SerializeField]
    [BoxGroup("Inventory")]
    private BackpackSlot slotPrefab;
    [SerializeField]
    [BoxGroup("Inventory")]
    private Vector2 origin;
    [SerializeField]
    [BoxGroup("Inventory")]
    private Vector2 margin;
    [SerializeField]
    [BoxGroup("Inventory")]
    private int horizontalCount;

    [SerializeField]
    [BoxGroup("Equipments")]
    private GameObject EquipmentsRoot;
    [SerializeField]
    [BoxGroup("Equipments")]
    private SerializableDictionaryBase<Equipments.Slot, BackpackSlot> EquipmentSlots;

    [SerializeField]
    [BoxGroup("ToolTip")]
    private GameObject toolTipRoot;
    [SerializeField]
    [BoxGroup("ToolTip")]
    private NormalSlot infoItemSlot;
    [SerializeField]
    [BoxGroup("ToolTip")]
    private TextMeshProUGUI infoItemName;

    [SerializeField]
    [BoxGroup("Information")]
    private GameObject infoRoot;
    [SerializeField]
    [BoxGroup("Information")]
    private TextMeshProUGUI infoItemText;
    [SerializeField]
    [BoxGroup("Information")]
    private GameObject useButton;

    [SerializeField]
    [BoxGroup("Split")]
    private GameObject splitRoot;

    /// <summary>
    /// 초기화
    /// </summary>
    public void Enable() {
        if(this.container == null) {
            this.container = GameManager.instance.player.backpack;
        }
        if(this.equipments == null) {
            this.equipments = GameManager.instance.player.equipments;
            EquipmentInventroyRegister();
        }
        if(selectedSlot != null) {
            UnselectSlot();
        }
        container.changeEvent -= MainInventoryChange;
        container.changeEvent += MainInventoryChange;
        equipments.changeEvent -= EquipmentInventroyChange;
        equipments.changeEvent += EquipmentInventroyChange;
        EndDrag();
        MainInventoryChange();
        EquipmentInventroyChange();
        EquipmentsRoot.SetActive(true);
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 숨기기
    /// </summary>
    public void Disable() {
        CancelDrag();
        gameObject.SetActive(false);
        if(selectedSlot != null) {
            UnselectSlot();
        }
        EquipmentsRoot.SetActive(false);
        container.changeEvent -= MainInventoryChange;
        equipments.changeEvent -= EquipmentInventroyChange;
    }

    /// <summary>
    /// 인벤토리의 변경사항 반영
    /// </summary>
    /// <param name="self">이벤트에 등록하기 위해 파리미터 조건을 맞출려고 들어감</param>
    public void MainInventoryChange(Container self) {
        MainInventoryChange();
    }

    /// <summary>
    /// 인벤토리의 변경사항 반영
    /// </summary>
    public void MainInventoryChange() {
        if(slots.Count != container.size) { //리사이즈
            //기존 슬롯 제거
            for(int i = 0; i < slotRoot.transform.childCount; i++) {
                Destroy(slotRoot.transform.GetChild(i).gameObject);
            }
            slots.Clear();

            //슬롯 초기화
            slotRoot.sizeDelta = new Vector2(0, -1 * ( origin.y + ( margin.y * ( ( container.size - 1 ) / horizontalCount + 1 ) ) ));
            for(int i = 0; i < container.size; i++) {
                BackpackSlot slot = Instantiate(slotPrefab, slotRoot.transform);
                slot.GetComponent<RectTransform>().localPosition = new Vector3(origin.x + ( margin.x * ( i % horizontalCount ) ), origin.y + ( margin.y * ( i / horizontalCount ) ), 0);
                slots.Add(slot);
                slot.onClick -= OnClickSlot;
                slot.onClick += OnClickSlot;
                slot.onHold -= OnHoldSlot;
                slot.onHold += OnHoldSlot;
                slot.onEnter -= OnSlotIn;
                slot.onEnter += OnSlotIn;
                slot.onExit -= OnSlotOut;
                slot.onExit += OnSlotOut;

                if(slot == dragSlot) {
                    dragItem.itemstack = container[i];
                    dragItem.UpdateHUD();
                    if(container[i].IsEmpty()) {
                        CancelDrag();
                    }
                } else {
                    slot.itemstack = container[i];
                    slot.UpdateHUD();
                }
            }
        } else {
            //슬롯 업데이트
            for(int i = 0; i < container.size; i++) {
                BackpackSlot slot = slots[i];
                if(slot == dragSlot) {
                    dragItem.itemstack = container[i];
                    dragItem.UpdateHUD();
                    if(container[i].IsEmpty()) {
                        CancelDrag();
                    }
                } else {
                    slot.itemstack = container[i];
                    slot.UpdateHUD();
                }
            }
        }
        //팝업창 업데이트
        UpdateToolTip();
    }

    /// <summary>
    /// 장비 인벤토리의 변경사항 반영
    /// <param name="self">이벤트에 등록하기 위해 파리미터 조건을 맞출려고 들어감</param>
    /// </summary>
    public void EquipmentInventroyChange(Equipments self) {
        EquipmentInventroyChange();
    }

    /// <summary>
    /// 장비 인벤토리 이벤트 등록
    /// </summary>
    public void EquipmentInventroyRegister() {
        foreach(Equipments.Slot type in Enum.GetValues(typeof(Equipments.Slot))) {
            BackpackSlot slot = EquipmentSlots[type];
            slot.onClick -= OnClickSlot;
            slot.onClick += OnClickSlot;
            slot.onHold -= OnHoldSlot;
            slot.onHold += OnHoldSlot;
            slot.onEnter -= OnSlotIn;
            slot.onEnter += OnSlotIn;
            slot.onExit -= OnSlotOut;
            slot.onExit += OnSlotOut;
        }
    }

    /// <summary>
    /// 장비 인벤토리의 변경사항 반영
    /// </summary>
    public void EquipmentInventroyChange() {
        foreach(Equipments.Slot type in Enum.GetValues(typeof(Equipments.Slot))) {
            BackpackSlot slot = EquipmentSlots[type];
            slot.itemstack = equipments.GetEquipment(type);
            slot.UpdateHUD();
        }
    }

    /// <summary>
    /// 슬롯 클릭시
    /// </summary>
    /// <param name="clickedSlot">클릭된 슬롯</param>
    public void OnClickSlot(BackpackSlot clickedSlot) {
        if(selectedSlot == clickedSlot) {
            //이미 선택되었었다면 취소
            UnselectSlot();
        } else {
            //선택되지 않았었다면 선택
            SelectSlot(clickedSlot);
        }
    }

    /// <summary>
    /// 슬롯 홀드시
    /// </summary>
    /// <param name="holdedSlot">홀드한 슬롯</param>
    public void OnHoldSlot(BackpackSlot holdedSlot) {
        StartDrag(holdedSlot);
    }

    /// <summary>
    /// UI 전체에 대한 마우스 놓기 감지
    /// </summary>
    public void OnMouseUp() {
        EndDrag();
    }

    /// <summary>
    /// 해당 슬롯 드레그 시작
    /// </summary>
    /// <param name="slot">대상 슬롯</param>
    public void StartDrag(BackpackSlot slot) {
        if(runDrag) {
            CancelDrag();
        }
        scroll.enabled = false;
        dragTargetSlot = slot;
        dragSlot = slot;
        dragItem.itemstack = slot.itemstack;
        dragItem.UpdateHUD();
        FixDragItemHUD();
        slot.itemstack = ItemStack.Empty;
        slot.UpdateHUD();
        dragItem.gameObject.SetActive(true);
        runDrag = true;
        Vibration.VibrateShort();
    }

    private void Update() {
        if(runDrag) {
            FixDragItemHUD();
        }
    }

    public void FixDragItemHUD() {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, Input.mousePosition, GameManager.instance.camera, out Vector2 localPos);
        dragItemRect.anchoredPosition = localPos;
    }

    /// <summary>
    /// 슬롯 드레그 인
    /// </summary>
    /// <param name="slot">대상 슬롯</param>
    public void OnSlotIn(BackpackSlot slot) {
        if(!runDrag) {
            return;
        }
        dragTargetSlot = slot;
    }

    /// <summary>
    /// 슬롯 드레그 아웃
    /// </summary>
    /// <param name="holdedSlot">대상 슬롯</param>
    public void OnSlotOut(BackpackSlot slot) {
        if(!runDrag) {
            return;
        }
        if(dragTargetSlot == slot) {
            dragTargetSlot = null;
        }
    }

    /// <summary>
    /// 드레그 종료
    /// </summary>
    public void EndDrag() {
        if(!runDrag) {
            return;
        }
        if(dragTargetSlot != null) {
            RunDrag();
        } else {
            CancelDrag();
        }
    }

    /// <summary>
    /// 드레그 실행
    /// </summary>
    public void RunDrag() {
        if(!runDrag) {
            return;
        }
        ItemStack from = dragItem.itemstack;
        ItemStack to = dragTargetSlot.itemstack;
        if(to.Stackable(from)) {
            int count = to.AddItem(from);
            from.OperateAmount(-count);
        } else {
            ItemStack tmp = to.Clone();
            to.CopyFrom(from);
            from.CopyFrom(tmp);
        }
        if(dragSlot == selectedSlot) {
            SelectSlot(dragTargetSlot);
        }
        CancelDrag();
        MainInventoryChange();
        EquipmentInventroyChange();
        HUDManager.instance.UpdateQuickBar();
    }

    /// <summary>
    /// 드레그 취소
    /// </summary>
    public void CancelDrag() {
        runDrag = false;
        dragItem.gameObject.SetActive(false);
        if(dragSlot != null) {
            dragSlot.itemstack = dragItem.itemstack;
            dragSlot.UpdateHUD();
        }
        dragItem.itemstack = ItemStack.Empty;
        dragItem.UpdateHUD();
        dragSlot = null;
        scroll.enabled = true;
        dragTargetSlot = null;
    }

    /// <summary>
    /// 슬롯 선택
    /// </summary>
    /// <param name="slot">대상 슬롯</param>
    public void SelectSlot(BackpackSlot slot) {
        if(slot.itemstack.IsEmpty()) {
            if(selectedSlot != null) {
                UnselectSlot();
            }
            return;
        }

        //이전에 선택된 슬롯 제거
        if(selectedSlot != null) {
            UnselectSlot();
        }

        ShowInfo(slot);
        slot.OnSelect();
    }

    /// <summary>
    /// 현재 슬롯 선택 취소
    /// </summary>
    public void UnselectSlot() {
        selectedSlot.OnUnselect();
        HideInfo();
    }

    /// <summary>
    /// 아이템 정보창 띄우기
    /// </summary>
    /// <param name="item">대상 아이템</param>
    public void ShowInfo(BackpackSlot slot) {
        if(slot == selectedSlot) {
            return;
        }
        ItemStack item = slot.itemstack;
        if(item.IsEmpty()) {
            return;
        }
        selectedSlot = slot;
        SelectSlot(slot);
        infoItemSlot.itemstack = item;

        UpdateToolTip();

        EquipmentsRoot.SetActive(false);
        splitRoot.SetActive(false);
        toolTipRoot.SetActive(true);
        infoRoot.SetActive(true);
    }

    /// <summary>
    /// 아이템 정보창 업데이트
    /// </summary>
    public void UpdateToolTip() {
        ItemStack item = infoItemSlot.itemstack;
        if(item.IsEmpty()) {
            HideInfo();
            HideSplit();
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
    }

    /// <summary>
    /// 아이템 정보창 숨기기
    /// </summary>
    public void HideInfo() {
        if(!toolTipRoot.activeSelf) {
            return;
        }
        toolTipRoot.SetActive(false);
        HideSplit();
        EquipmentsRoot.SetActive(true);
        infoItemSlot.itemstack = ItemStack.Empty;
        infoItemSlot.UpdateHUD();
        UnselectSlot();
        selectedSlot = null;
    }

    public void ShowSplit() {
        if(selectedSlot == null) {
            return;
        }
        if(splitRoot.activeSelf) {
            return;
        }

        EquipmentsRoot.SetActive(false);
        infoRoot.SetActive(false);
        toolTipRoot.SetActive(true);
        splitRoot.SetActive(true);
    }

    public void HideSplit() {
        if(selectedSlot == null) {
            return;
        }
        if(!splitRoot.activeSelf) {
            return;
        }
        splitRoot.SetActive(false);

        UpdateToolTip();

        infoRoot.SetActive(true);
        toolTipRoot.SetActive(true);
    }

    public void SplitCancelButtonClick() {
        HideSplit();
    }

    public void SplitDoneButtonClick() {

    }

    /// <summary>
    /// 아이템 분할 버튼 클릭시
    /// </summary>
    public void SplitButtonClicked() {
        ShowSplit();
    }

    /// <summary>
    /// 아이템 사용 버튼 클릭시
    /// </summary>
    public void UseButtonClicked() {
        ItemStack item = infoItemSlot.itemstack;
        item.type.itemFuntion?.OnUse(item);
        MainInventoryChange();
        EquipmentInventroyChange();
    }

    /// <summary>
    /// 아이템 버리기 버튼 클릭시
    /// </summary>
    public void DiscardButtonClicked() {
        //TODO 버리기 로직 수정

        Player player = GameManager.instance.player;
        ItemStack targetItem = infoItemSlot.itemstack;
        if(targetItem.type.itemFuntion == null) {
            player.DropItem(targetItem);
            player.backpack.RemoveItemRestrict(targetItem);
            HideInfo();
        } else {
            targetItem.type.itemFuntion.OnDiscard(targetItem);
            UpdateToolTip();
        }
        MainInventoryChange();
        EquipmentInventroyChange();
    }
}
