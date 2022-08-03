using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnData
{
    /// <summary>
    /// �ش� ���� ������ ���赵 <br/>
    /// �� ��ġ������ ���� �����ȴ�
    /// </summary>
    public readonly int initialRisk;
    public int risk;
    /// <summary>
    /// �ش� ���� ������ ���� <br/>
    /// �� ��ġ������ ������ �����ȴ� <br/>
    /// �� ��ġ�� ������ ���赵�� �Բ� �����Ѵ�
    /// </summary>
    public readonly int initialReturns;
    public int returns;

    public SpawnData(int risk, int returns)
    {
        this.initialRisk = risk;
        this.initialReturns = returns;

        Reset();
    }

    public void Reset()
    {
        this.risk = 0;
        this.returns = 0;
    }
}
