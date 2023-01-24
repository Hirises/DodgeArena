using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

/// <summary>
/// HUD(Head Up Display) Manager
/// </summary>
public class HUDManager : MonoBehaviour {
    public static HUDManager instance;

    [SerializeField]
    [BoxGroup("Harvest")]
    private GameObject HarvestHUD;
    [SerializeField]
    [BoxGroup("Harvest")]
    private Image HarvestHUDFill;
    private Util.Runnable HarvestCallback;
    private Timer HarvestTimer = new Timer();
    private Queue<Util.Runnable> HarvestQueue;
    private bool RunHarveting = false;

    [SerializeField]
    [BoxGroup("Quick Bar")]
    private GameObject quickBar;
    [SerializeField]
    [BoxGroup("Quick Bar")]
    private QuickbarSlot[] quickBarSlots;

    [SerializeField]
    [BoxGroup("Backpack")]
    private BackpackUI backpack;

    [SerializeField]
    [BoxGroup("JoyStick")]
    private JoyStickHUD joyStick;

    private void Awake() {
        if(instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
            return;
        }

        HarvestTimer.type = Timer.Count.DependedUp;
    }

    private void Update() {
        switch (GameManager.instance.state) {
            case GameManager.GameState.Run:
                if(Input.GetMouseButtonDown(0)) {
                    bool gameTouch = EventSystem.current.IsPointerOverGameObject(0);
                    #if UNITY_EDITOR
                        gameTouch = EventSystem.current.IsPointerOverGameObject();
                    #endif
                    if(!gameTouch) {
                        joyStick.Enable(Input.mousePosition);
                    }
                } else if(Input.GetMouseButton(0)) {
                    joyStick.Run(Input.mousePosition);
                } else if(Input.GetMouseButtonUp(0)) {
                    joyStick.Disable();
                }

                if(!RunHarveting && HarvestQueue.TryDequeue(out Util.Runnable next)) {
                    next();
                }
                break;
            case GameManager.GameState.UI:
                joyStick.Disable();
                break;
            case GameManager.GameState.Stop:
                joyStick.Disable();
                break;
        }
    }

    public void StartHarvest(float time, Util.Runnable harvestCallback) {
        HarvestQueue.Enqueue(() => {
            StartQueuedHarvet(time, harvestCallback);
        });
    }

    public void StartQueuedHarvet(float time, Util.Runnable harvestCallback) {
        RunHarveting = true;
        this.HarvestCallback = harvestCallback;
        this.HarvestTimer.Reset();
        this.HarvestTimer.target = time;
        HarvestHUDFill.fillAmount = 0;
        this.HarvestHUD.SetActive(true);
        HarvestTimer.Start(DrawHarvest, EndHarvest);
    }

    private void DrawHarvest(float time) {
        HarvestHUDFill.fillAmount = HarvestTimer.rate;
    }

    private void EndHarvest() {
        HarvestTimer.Stop();
        this.HarvestHUD.SetActive(false);
        HarvestCallback();
        RunHarveting = false;
    }

    public void StopHarvest() {
        HarvestTimer.Stop();
        this.HarvestHUD.SetActive(false);
        RunHarveting = false;
    }

    public void ShowQuickBar() {
        quickBar.SetActive(true);
    }

    public void HideQuickBar() {
        quickBar.SetActive(false);
    }

    /// <summary>
    /// 이벤트 참조를 위해...
    /// </summary>
    public void UpdateQuickBar(Equipments self) {
        UpdateQuickBar();
    }

    public void UpdateQuickBar() {
        Player player = GameManager.instance.player;
        for(int i = 0; i < 4; i++) {
            quickBarSlots[i].itemstack = player.equipments.GetQuickbarItem(i);
            quickBarSlots[i].UpdateHUD();
        }
    }

    public void ShowBackpack() {
        GameManager.instance.state = GameManager.GameState.UI;
        HideQuickBar();
        backpack.Active();
    }

    public void HideBackpack() {
        backpack.Disactive();
        ShowQuickBar();
        GameManager.instance.state = GameManager.GameState.Run;
    }
}