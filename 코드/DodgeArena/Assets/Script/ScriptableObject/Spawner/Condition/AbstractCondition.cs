using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractCondition : ScriptableObject
{
    public abstract bool CheckCondition(Chunk chunk);
}
