using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������ ���õ� ûũ�� ���� ����
/// </summary>
[System.Serializable]
public class ChunkData{
    /// <summary>
    /// �ش� ���� ������ ���赵 <br/>
    /// �� ��ġ������ ���� �����ȴ�
    /// </summary>
    public int initialRisk;
    public int risk;
    /// <summary>
    /// �ش� ���� ������ ���� <br/>
    /// �� ��ġ������ ������ �����ȴ� <br/>
    /// �� ��ġ�� ������ ���赵�� �Բ� �����Ѵ�
    /// </summary>
    public int initialReturns;
    public int returns;

    public ChunkData(int risk, int returns) {
        this.initialRisk = risk;
        this.initialReturns = returns;

        Reset();
    }

    public void Reset() {
        this.risk = 0;
        this.returns = 0;
    }
}
