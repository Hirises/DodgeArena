using NaughtyAttributes;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;
using System;
using UnityEngine.UI;

public class BackpackUI : MonoBehaviour {

    //필드 영역 --------------------------------------------------------------

    #region Private Fields
    /// <summary>
    /// 플레이어 인벤토리
    /// </summary>
    private Container container;
    /// <summary>
    /// 플레이어 장비
    /// </summary>
    private Equipments equipments;
    /// <summary>
    /// 전체 인벤토리 슬롯
    /// </summary>
    private List<BackpackSlot> slots = new List<BackpackSlot>();
    /// <summary>
    /// 현재 선택된 슬롯
    /// </summary>
    private BackpackSlot selectedSlot;
    /// <summary>
    /// 현재 드레그 중인가?
    /// </summary>
    private bool runDrag = false;
    /// <summary>
    /// 현재 드레그 중인 슬롯
    /// </summary>
    private BackpackSlot dragSlot;
    /// <summary>
    /// 현재 드레그 목적지로 지정된 슬롯
    /// </summary>
    private BackpackSlot dragTargetSlot;
    /// <summary>
    /// 나뉘어질 아이템 개수 (남는 쪽)
    /// </summary>
    private int splitAmount;
    #endregion

    #region Inventory
    [SerializeField]
    [BoxGroup("Inventory")]
    private RectTransform canvas;
    [SerializeField]
    [BoxGroup("Inventory")]
    private ScrollRect inventroyScroll;
    [SerializeField]
    [BoxGroup("Inventory")]
    private RectTransform inventoryContent;
    [SerializeField]
    [BoxGroup("Inventory")]
    private BackpackSlot slotPrefab;
    /// <summary>
    /// 슬롯 시작 위치
    /// </summary>
    [SerializeField]
    [BoxGroup("Inventory")]
    private Vector2 slotOrigin;
    /// <summary>
    /// 슬롯 하나당 얼마다 떨어져서 배치할지
    /// </summary>
    [SerializeField]
    [BoxGroup("Inventory")]
    private Vector2 slotMargin;
    /// <summary>
    /// 가로 슬롯 개수
    /// </summary>
    [SerializeField]
    [BoxGroup("Inventory")]
    private int horizontalSlotCount;
    [SerializeField]
    [BoxGroup("Inventory")]
    private ItemIcon dragItemIcon;
    [SerializeField]
    [BoxGroup("Inventory")]
    private RectTransform dragItemRect; 
    #endregion

    #region Equipments
    [SerializeField]
    [BoxGroup("Equipments")]
    private GameObject EquipmentsRoot;
    /// <summary>
    /// 장비 슬롯들
    /// </summary>
    [SerializeField]
    [BoxGroup("Equipments")]
    private SerializableDictionaryBase<Equipments.Slot, BackpackSlot> EquipmentSlots; 
    #endregion

    #region ToolTip
    [SerializeField]
    [BoxGroup("ToolTip")]
    private GameObject toolTipRoot;
    [SerializeField]
    [BoxGroup("ToolTip")]
    private NormalSlot infoItemSlot;
    [SerializeField]
    [BoxGroup("ToolTip")]
    private TextMeshProUGUI infoItemName; 
    #endregion

    #region Information
    [SerializeField]
    [BoxGroup("Information")]
    private GameObject infoRoot;
    [SerializeField]
    [BoxGroup("Information")]
    private TextMeshProUGUI infoItemText;
    [SerializeField]
    [BoxGroup("Information")]
    private GameObject useButton; 
    #endregion

    #region Split
    [SerializeField]
    [BoxGroup("Split")]
    private GameObject splitRoot;
    /// <summary>
    /// 아이템 나누기 슬라이더
    /// </summary>
    [SerializeField]
    [BoxGroup("Split")]
    private Slider valueSlider;
    /// <summary>
    /// 아이템 나눌 개수 (남는 쪽)
    /// </summary>
    [SerializeField]
    [BoxGroup("Split")]
    private TMP_InputField valueInput;
    /// <summary>
    /// 아이템 나눌 개수 (나눠질 쪽)
    /// </summary>
    [SerializeField]
    [BoxGroup("Split")]
    private TMP_InputField restInput;
    [SerializeField]
    [BoxGroup("Split")]
    private GameObject splitButton;
    [SerializeField]
    [BoxGroup("Split")]
    private GameObject discardButton;
    #endregion

    //메소드 영역 ------------------------------------------------------------

    /// <summary>
    /// 초기화
    /// </summary>
    public void Active() {
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
        container.changeEvent -= UpdateMainInventory;
        container.changeEvent += UpdateMainInventory;
        equipments.changeEvent -= UpdateEquipmentInventroy;
        equipments.changeEvent += UpdateEquipmentInventroy;
        EndDrag();
        UpdateMainInventory();
        UpdateEquipmentInventroy();
        EquipmentsRoot.SetActive(true);
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 숨기기
    /// </summary>
    public void Disactive() {
        CancelDrag();
        gameObject.SetActive(false);
        if(selectedSlot != null) {
            UnselectSlot();
        }
        EquipmentsRoot.SetActive(false);
        container.changeEvent -= UpdateMainInventory;
        equipments.changeEvent -= UpdateEquipmentInventroy;
    }

    #region UpdateUI
    /// <summary>
    /// 인벤토리의 변경사항 반영
    /// </summary>
    /// <param name="self">이벤트에 등록하기 위해 파리미터 조건을 맞출려고 들어감</param>
    public void UpdateMainInventory(Container self) {
        UpdateMainInventory();
    }

    /// <summary>
    /// 인벤토리의 변경사항 반영
    /// </summary>
    public void UpdateMainInventory() {
        if(slots.Count != container.size) { //리사이즈
            //기존 슬롯 제거
            for(int i = 0; i < inventoryContent.transform.childCount; i++) {
                Destroy(inventoryContent.transform.GetChild(i).gameObject);
            }
            slots.Clear();

            //슬롯 초기화
            inventoryContent.sizeDelta = new Vector2(0, -1 * ( slotOrigin.y + ( slotMargin.y * ( ( container.size - 1 ) / horizontalSlotCount + 1 ) ) ));
            for(int i = 0; i < container.size; i++) {
                BackpackSlot slot = Instantiate(slotPrefab, inventoryContent.transform);
                slot.GetComponent<RectTransform>().localPosition = new Vector3(slotOrigin.x + ( slotMargin.x * ( i % horizontalSlotCount ) ), slotOrigin.y + ( slotMargin.y * ( i / horizontalSlotCount ) ), 0);
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
                    dragItemIcon.itemstack = container[i];
                    dragItemIcon.UpdateHUD();
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
                    dragItemIcon.itemstack = container[i];
                    dragItemIcon.UpdateHUD();
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
    /// <param name="self">이벤트에 등록하기 위해 파리미터 조건을 맞출려고 들어감</param>
    /// </summary>
    public void UpdateEquipmentInventroy(Equipments self) {
        UpdateEquipmentInventroy();
    }

    /// <summary>
    /// 장비 인벤토리의 변경사항 반영
    /// </summary>
    public void UpdateEquipmentInventroy() {
        foreach(Equipments.Slot type in Enum.GetValues(typeof(Equipments.Slot))) {
            BackpackSlot slot = EquipmentSlots[type];
            slot.itemstack = equipments.GetEquipment(type);
            slot.UpdateHUD();
        }
    }
    #endregion

    #region DragControll
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
        inventroyScroll.enabled = false;
        dragTargetSlot = slot;
        dragSlot = slot;
        dragItemIcon.itemstack = slot.itemstack;
        dragItemIcon.UpdateHUD();
        FixDragItemHUD();
        slot.itemstack = ItemStack.Empty;
        slot.UpdateHUD();
        dragItemIcon.gameObject.SetActive(true);
        runDrag = true;
        Vibration.VibrateShort();
    }

    private void Update() {
        if(runDrag) {
            FixDragItemHUD();
        }
    }

    /// <summary>
    /// 드레그시 아이템 아이콘 마우스 위치로 고정
    /// </summary>
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
        ItemStack from = dragItemIcon.itemstack;
        ItemStack to = dragTargetSlot.itemstack;
        if(Util.GetKey(EquipmentSlots, dragTargetSlot, out Equipments.Slot slot)) {
            if(from.type.itemFuntion == null || !from.type.itemFuntion.CanEquip(slot, from)) {
                CancelDrag();
                UpdateMainInventory();
                UpdateEquipmentInventroy();
                HUDManager.instance.UpdateQuickBar();
                return;
            } else {
                if(!to.IsEmpty() && to.type.itemFuntion != null) {
                    to.type.itemFuntion.OnUnequip(slot, to);
                }
                from.type.itemFuntion.OnEquip(slot, from);
            }
        }

        if(to.Stackable(from)) {
            int count = to.AddItem(from);
            from.OperateAmount(-count);
        } else {
            ItemStack tmp = to.Clone();
            to.CopyFrom(from);
            from.CopyFrom(tmp);
        }
        if(dragSlot == selectedSlot) {
            if(splitRoot.activeSelf) {
                SelectSlot(dragTargetSlot);
                ShowSplit();
            } else {
                SelectSlot(dragTargetSlot);
            }
        }
        CancelDrag();
        UpdateMainInventory();
        UpdateEquipmentInventroy();
        HUDManager.instance.UpdateQuickBar();
    }

    /// <summary>
    /// 드레그 취소
    /// </summary>
    public void CancelDrag() {
        runDrag = false;
        dragItemIcon.gameObject.SetActive(false);
        if(dragSlot != null) {
            dragSlot.itemstack = dragItemIcon.itemstack;
            dragSlot.UpdateHUD();
        }
        dragItemIcon.itemstack = ItemStack.Empty;
        dragItemIcon.UpdateHUD();
        dragSlot = null;
        inventroyScroll.enabled = true;
        dragTargetSlot = null;
    }
    #endregion

    #region SelectSlot
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
        if(selectedSlot != null) {
            selectedSlot.OnUnselect();
        }
        HideInfo();
    } 
    #endregion

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
        UpdateSplitStateForValue(splitAmount);

        if(item.type.itemFuntion?.CanUse(item) ?? false) {
            useButton.SetActive(true);
        } else {
            useButton.SetActive(false);
        }
    }

    #region Infomation Popup
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
    #endregion

    #region Split Popup
    private void ResetSplitState(bool forDiscard) {
        if(selectedSlot == null) {
            return;
        }
        if(splitRoot.activeSelf) {
            return;
        }

        if(forDiscard) {
            UpdateSplitStateForValue(selectedSlot.itemstack.amount);
            discardButton.SetActive(true);
            splitButton.SetActive(false);
        } else {
            UpdateSplitStateForValue(( selectedSlot.itemstack.amount + 1 ) / 2);
            discardButton.SetActive(false);
            splitButton.SetActive(true);
        }
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

    public void UpdateSplitState() {
        int maxAmount = infoItemSlot.itemstack.amount;
        UpdateSplitStateForValue(Convert.ToInt32(valueSlider.value * maxAmount));
    }

    public void UpdateSplitStateForRest(int value) {
        UpdateSplitStateForValue(infoItemSlot.itemstack.amount - value);
    }

    public void UpdateSplitStateForValue(int value) {
        int maxAmount = infoItemSlot.itemstack.amount;
        if(value < 1) {
            value = 1;
        }
        if(value > maxAmount) {
            value = maxAmount;
        }
        valueSlider.value = ( (float) value ) / maxAmount;
        valueInput.text = "" + value;
        restInput.text = "" + ( maxAmount - value );
        splitAmount = value;
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
    #endregion

    #region Split Input
    public void SplitAddButtonClick() {
        UpdateSplitStateForValue(splitAmount + 1);
    }

    public void SplitRemoveButtonClick() {
        UpdateSplitStateForValue(splitAmount - 1);
    }

    public void SplitValueSliderChange() {
        UpdateSplitState();
    }

    public void SplitValueInputChange() {
        if(int.TryParse(valueInput.text, out int number)) {
            UpdateSplitStateForValue(number);
        } else {
            UpdateSplitStateForValue(splitAmount);
        }
    }

    public void SplitRestInputChange() {
        if(int.TryParse(restInput.text, out int number)) {
            UpdateSplitStateForRest(number);
        } else {
            UpdateSplitStateForValue(splitAmount);
        }
    }

    public void SplitMinButtonClicked() {
        UpdateSplitStateForValue(1);
    }

    public void SplitHalfButtonClicked() {
        UpdateSplitStateForValue(( selectedSlot.itemstack.amount + 1 ) / 2);
    }

    public void SplitMaxButtonClicked() {
        UpdateSplitStateForValue(selectedSlot.itemstack.amount);
    }

    public void SplitCancelButtonClick() {
        HideSplit();
    }

    public void SplitDiscardButtonClick() {
        Player player = GameManager.instance.player;
        ItemStack targetItem = infoItemSlot.itemstack;

        player.DropItem(targetItem.Clone().SetAmount(splitAmount));
        targetItem.OperateAmount(-splitAmount);
        HideSplit();

        UpdateMainInventory();
        UpdateEquipmentInventroy();
    }

    public void SplitSplitButtonClick() {
        Player player = GameManager.instance.player;
        ItemStack targetItem = infoItemSlot.itemstack;

        ItemStack rest = player.backpack.AddItemAtEmptySlot(targetItem.Clone().SetAmount(targetItem.amount - splitAmount));
        targetItem.SetAmount(splitAmount);
        targetItem.AddItem(rest);

        HideSplit();
        UpdateMainInventory();
        UpdateEquipmentInventroy();
    }

    /// <summary>
    /// 아이템 분할 버튼 클릭시
    /// </summary>
    public void SplitButtonClicked() {
        ResetSplitState(false);
        ShowSplit();
    }

    /// <summary>
    /// 아이템 버리기 버튼 클릭시
    /// </summary>
    public void DiscardButtonClicked() {
        ResetSplitState(true);
        ShowSplit();
    }
    #endregion

    /// <summary>
    /// 아이템 사용 버튼 클릭시
    /// </summary>
    public void UseButtonClicked() {
        ItemStack item = infoItemSlot.itemstack;
        if(item.type.itemFuntion != null && item.type.itemFuntion.CanUse(item)) {
            item.type.itemFuntion.OnUse(item);
            UpdateMainInventory();
            UpdateEquipmentInventroy();
        }
    }
}
