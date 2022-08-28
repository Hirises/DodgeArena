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
    public GameObject HarvestHUD;
    [SerializeField]
    [BoxGroup("Harvest")]
    public Image HarvestHUDFill;
    private Util.Runnable HarvestCallback;
    private Timer HarvestTimer = new Timer();

    [SerializeField]
    [BoxGroup("Quick Bar")]
    public GameObject quickBar;

    [SerializeField]
    [BoxGroup("Backpack")]
    public InventoryHUD backpack;

    [SerializeField]
    [BoxGroup("JoyStick")]
    public JoyStickHUD joyStick;

    private void Awake() {
        if(instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }

        HarvestTimer.count = Timer.Count.Up;
    }

    private void Update() {
        if(GameManager.instance.state == GameManager.GameState.Run) {
            if(Input.GetMouseButtonDown(0)) {
                if(EventSystem.current.IsPointerOverGameObject() == false) {
                    joyStick.Enable(Input.mousePosition);
                }
            } else if(Input.GetMouseButton(0)) {
                joyStick.Run(Input.mousePosition);
            } else if(Input.GetMouseButtonUp(0)) {
                joyStick.Disable();
            }
        } else {
            joyStick.Disable();
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

    public void ShowBackpack() {
        GameManager.instance.state = GameManager.GameState.Stop;
        backpack.Init(GameManager.instance.player.backpack);
        backpack.gameObject.SetActive(true);
    }

    public void HideBackpack() {
        backpack.gameObject.SetActive(false);
        GameManager.instance.state = GameManager.GameState.Run;
    }
}