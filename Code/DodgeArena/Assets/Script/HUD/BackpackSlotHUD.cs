using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;
using NaughtyAttributes;

public class BackpackSlotHUD : SlotHUD, IPointerClickHandler, IPointerDownHandler, IPointerMoveHandler {
    [SerializeField]
    public Image image;
    [SerializeField]
    public Sprite defaultSlot;
    [SerializeField]
    public Sprite[] equipedSlot;
    [SerializeField]
    public Color selectedColor;
    private Timer timer = new Timer();
    public delegate void SlotEvent(BackpackSlotHUD slot);
    public event SlotEvent onHold;
    public event SlotEvent onClick;

    public void OnPointerClick(PointerEventData eventData) {
        //클릭
        if(onClick != null) {
            onClick(this);
        }
    }

    public void OnPointerDown(PointerEventData eventData) {
        //마우스 누르기
        timer.target = 0.5f;
        timer.Reset();
        timer.Start(null, OnPointerHold);
    }

    public void OnPointerHold() {
        //홀드
        timer.Stop();
        if(onHold != null) {
            onHold(this);
        }
    }

    public void OnPointerMove(PointerEventData eventData) {
        //마우스 벗어남 & 마우스 땜
        timer.Stop();
    }

    public override void UpdateHUD() {
        base.UpdateHUD();
        switch(GameManager.instance.player.GetEquipedSlot(innerItemHUD.itemstack)) {
            case var slot when slot >= 0 :
                image.sprite = equipedSlot[slot];
                break;
            default:
                image.sprite = defaultSlot;
                break;
        }
    }

    public void Select() {
        image.color = selectedColor;
    }

    public void Unselect() {
        image.color = Color.white;
    }
}