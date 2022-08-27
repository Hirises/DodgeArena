using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 바이옴 생성 정보
/// 다시 계산하기에는 무거워서 저장해둠
/// </summary>
public class BiomeInfo {
    public readonly float dificulty;
    public readonly float temperature;
    public List<int> potential;

    public BiomeInfo(float dificulty, float temperature) {
        this.dificulty = dificulty;
        this.temperature = temperature;
    }
}