using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 자원을 얻을 수 있는 엔티티
/// </summary>
public interface IResourceSource {
    public float time {
        get;
    }
    protected bool harvesting {
        get;
        set;
    }

    /// <summary>
    /// OnSpawn에서 한번 실행시켜줘야함
    /// </summary>
    /// <param name="collider"></param>
    public void Enable(SubCollider collider) {
        collider.onTriggerEnter -= OnEnterHarvestingArea;
        collider.onTriggerEnter += OnEnterHarvestingArea;
        collider.onTriggerExit -= OnExitHarvestingArea;
        collider.onTriggerExit += OnExitHarvestingArea;
    }

    protected void OnEnterHarvestingArea(Entity other, Collider2D collider) {
        if(other is Player player) {
            if(CanHarvest(player)) {
                OnStartHarvesting(player);
                HUDManager.instance.StartHarvest(time, () => EndHarveting(player));
            }
        }
    }

    protected void OnExitHarvestingArea(Entity other, Collider2D collider) {
        if(harvesting && other is Player player) {
            HUDManager.instance.StopHarvest();
            harvesting = false;
            OnStopHarvesting(player);
        }
    }

    protected void EndHarveting(Player player) {
        GiveResource(player);
        OnSuccessHarvesting(player);
    }

    /// <summary>
    /// 플레이어에게 랜덤 아이템을 지급
    /// </summary>
    public void GiveResource(Player player);

    /// <summary>
    /// 이 객체가 채집 가능한 상태인지를 반환
    /// </summary>
    /// <param name="player">대상 플레이어</param>
    /// <returns>true라면 채집 가능</returns>
    public bool CanHarvest(Player player);

    /// <summary>
    /// 실제 채집을 시작하기 직전 실행
    /// </summary>
    public void OnStartHarvesting(Player player);

    /// <summary>
    /// 채집과 관련된 모든 처리가 성공적으로 완료 된 후 실행
    /// </summary>
    public void OnSuccessHarvesting(Player player);
    
    /// <summary>
    /// 채집과 관련도니 모든 처리가 종료된 후 실행 (실패, 취소시)
    /// </summary>
    public void OnStopHarvesting(Player player);
}