using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

/// <summary>
/// 자원 소스 인터페이스
/// </summary>
public interface IResourceSouce {

    /// <summary>
    /// 이 자원 소스에 대한 랜덤 드롭 아이템을 반환합니다
    /// </summary>
    /// <returns>드롭 아이템</returns>
    public List<ItemStack> GetRandomSourceItems();

    /// <summary>
    /// 이 자원 소스를 채취하기 위한 시간을 반환합니다
    /// </summary>
    /// <returns>채취 시간(초)</returns>
    public float GetHarvestingTime();
}