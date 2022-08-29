using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;
using NaughtyAttributes;

public class BackpackSlotHUD : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerMoveHandler {
    [SerializeField]
    public ItemHUD innerItemHUD;
    [ReadOnly]
    public int index;
    private Timer timer = new Timer();
    public delegate void SlotEvent(BackpackSlotHUD slot);
    public event SlotEvent onPointerDown;
    public event SlotEvent onPointerUp;
    public event SlotEvent onClick;

    public void OnPointerClick(PointerEventData eventData) {
        //Debug.Log("click");
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
        //Debug.Log("hold");
    }

    public void OnPointerMove(PointerEventData eventData) {
        timer.Stop();
        //onPointerUp(this);
    }

    public void UpdateHUD() {
        innerItemHUD.UpdateHUD();
    }
}