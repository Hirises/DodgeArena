using System.Collections;
using UnityEngine;

public class SlotHUD : MonoBehaviour {
    [SerializeField]
    public ItemHUD innerItemHUD;

    /// <summary>
    /// 슬롯 내부 HUD를 업데이트 합니다
    /// </summary>
    public virtual void UpdateHUD() {
        innerItemHUD.UpdateHUD();
    }
}