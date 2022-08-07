using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Generator<T> : ScriptableObject
{
    public abstract bool CheckConditions(Chunk chunk);

    public abstract int GetWeight(Chunk chunk);
    public abstract T Generate(Chunk chunk);
}
