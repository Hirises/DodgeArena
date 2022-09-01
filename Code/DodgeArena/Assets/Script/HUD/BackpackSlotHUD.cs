using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;
using NaughtyAttributes;

public class BackpackSlotHUD : SlotHUD, IPointerClickHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler {
    [SerializeField]
    public Image image;
    [SerializeField]
    public Color selectedColor;
    private Timer timer = new Timer();
    public delegate void SlotEvent(BackpackSlotHUD slot);
    public event SlotEvent onHold;
    public event SlotEvent onClick;
    public event SlotEvent onEnter;
    public event SlotEvent onExit;

    public void OnPointerClick(PointerEventData eventData) {
        //클릭
        timer.Stop();
        if(onClick != null) {
            onClick(this);
        }
    }

    public void OnPointerDown(PointerEventData eventData) {
        //마우스 누르기
        timer.target = 0.3f;
        timer.type = Timer.Count.IndependedUp;
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


    public void OnPointerUp(PointerEventData eventData) {
        //마우스 땜
        timer.Stop();
    }
    public void OnPointerEnter(PointerEventData eventData) {
        //마우스 들어옴 (드레그 인)
        if(onEnter != null) {
            onEnter(this);
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        //마우스 나감 (드레그 아웃)
        timer.Stop();
        if(onExit != null) {
            onExit(this);
        }
    }

    /// <summary>
    /// 이 슬롯 선택시
    /// </summary>
    public void OnSelect() {
        //하이라이팅
        image.color = selectedColor;
    }

    /// <summary>
    /// 이 슬롯 선택 해제시
    /// </summary>
    public void OnUnselect() {
        //하이라이팅 해제
        image.color = Color.white;
    }
}