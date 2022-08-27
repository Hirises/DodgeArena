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
    private Util.Runnable HarvestCallback;
    private Timer HarvestTimer = new Timer();

    private void Awake() {
        if(instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }

        HarvestTimer.count = Timer.Count.Up;
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
}