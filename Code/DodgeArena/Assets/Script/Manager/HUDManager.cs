using System.Collections;
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
                break;
            case GameManager.GameState.UI:
                joyStick.Disable();
                break;
            case GameManager.GameState.Stop:
                joyStick.Disable();
                break;
        }
    }

    public bool StartHarvest(float time, Util.Runnable harvestCallback) {
        if(GameManager.instance.player.isHarvesting) {
            return false;
        }
        GameManager.instance.player.isHarvesting = true;
        this.HarvestCallback = harvestCallback;
        this.HarvestTimer.Reset();
        this.HarvestTimer.target = time;
        HarvestHUDFill.fillAmount = 0;
        this.HarvestHUD.SetActive(true);
        HarvestTimer.Start(DrawHarvest, EndHarvest);
        return true;
    }

    private void DrawHarvest(float time) {
        HarvestHUDFill.fillAmount = HarvestTimer.rate;
    }

    private void EndHarvest() {
        HarvestTimer.Stop();
        this.HarvestHUD.SetActive(false);
        HarvestCallback();
        GameManager.instance.player.isHarvesting = false;
    }

    public void StopHarvest() {
        HarvestTimer.Stop();
        this.HarvestHUD.SetActive(false);
        GameManager.instance.player.isHarvesting = false;
    }

    public void ShowQuickBar() {
        quickBar.SetActive(true);
    }

    public void HideQuickBar() {
        quickBar.SetActive(false);
    }

    /// <summary>
    /// ????????? ????????? ??????...
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