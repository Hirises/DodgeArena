using System.Collections;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.UI;

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
    private GameData.Runnable HarvestCallback;
    private Timer HarvestTimer = new Timer();

    private void OnEnable() {
        if(instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }

        HarvestTimer.count = Timer.Count.Up;
    }

    private void OnDisable() {
        if(instance == this) {
            instance = null;
        }
    }

    public bool StartHarvest(float time, GameData.Runnable harvestCallback) {
        if(IsHarvest()) {
            return false;
        }
        this.HarvestCallback = harvestCallback;
        this.HarvestTimer.Reset();
        this.HarvestTimer.target = time;
        HarvestHUDFill.fillAmount = 0;
        this.HarvestHUD.SetActive(true);
        StartCoroutine(HarvestTimer.Start(DrawHarvest, EndHarvest));
        return true;
    }

    private void DrawHarvest(float time) {
        HarvestHUDFill.fillAmount = HarvestTimer.rate;
    }

    private void EndHarvest() {
        this.HarvestHUD.SetActive(false);
        HarvestCallback();
    }

    public void StopHarvest() {
        StopCoroutine(HarvestTimer.instance);
    }

    public bool IsHarvest() {
        return HarvestHUD.activeSelf;
    }
}