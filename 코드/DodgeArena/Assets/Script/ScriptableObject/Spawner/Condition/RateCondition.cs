using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RateCondition", menuName = "Spawner/Condition/RateCondition", order = 0)]
public class RateCondition : AbstractCondition
{
    [SerializeField]
    private float rate;

    public override bool CheckCondition(Chunk chunk)
    {
        return true;
    }
}
