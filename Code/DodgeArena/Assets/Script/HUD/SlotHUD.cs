using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class SlotHUD : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerMoveHandler {
    [SerializeField]
    public ItemHUD innerItem;
    [SerializeField]
    public bool canAdd;
    [SerializeField]
    public bool canRemove;
    private Timer timer = new Timer();
    public delegate void SlotEvent(SlotHUD slot);
    public event SlotEvent onPointerDown;
    public event SlotEvent onPointerUp;
    public event SlotEvent onClick;

    public void OnPointerClick(PointerEventData eventData) {
        Debug.Log("click");
        onClick(this);
    }

    public void OnPointerDown(PointerEventData eventData) {
        timer.target = 0.5f;
        timer.Reset();
        timer.Start(null, OnPointerHold);
        //onPointerDown(this);
    }

    public void OnPointerHold() {
        timer.Stop();
        Debug.Log("hold");
    }

    public void OnPointerMove(PointerEventData eventData) {
        timer.Stop();
        //onPointerUp(this);
    }

    public void UpdateHUD() {
        innerItem.UpdateHUD();
    }
}