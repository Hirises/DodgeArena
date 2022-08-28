using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 스폰과 관련된 청크의 여러 정보
/// </summary>
[System.Serializable]
public class ChunkData{
    /// <summary>
    /// 해당 스폰 지역의 밀도 <br/>
    /// 이 수치까지만 오브젝트가
    /// </summary>
    public int initialDense;
    public int dense;
    /// <summary>
    /// 해당 스폰 지역의 위험도 <br/>
    /// 이 수치까지만 적이 스폰된다
    /// </summary>
    public int initialRisk;
    public int risk;
    /// <summary>
    /// 해당 스폰 지역의 보상 <br/>
    /// 이 수치까지만 보상이 스폰된다 <br/>
    /// 이 수치가 높으면 위험도도 함께 증가한다
    /// </summary>
    public int initialReturns;
    public int returns;
    /// <summary>
    /// 이 스폰지역의 태그
    /// </summary>
    public List<string> initialTag;
    public List<string> tags;

    public ChunkData(int dense, int risk, int returns, List<string> tag) {
        this.initialDense = dense;
        this.initialRisk = risk;
        this.initialReturns = returns;
        this.initialTag = tag;

        Reset();
    }

    public void Reset() {
        this.dense = 0;
        this.risk = 0;
        this.returns = 0;
        this.tags = new List<string>();
        tags.AddRange(initialTag);
    }
}
