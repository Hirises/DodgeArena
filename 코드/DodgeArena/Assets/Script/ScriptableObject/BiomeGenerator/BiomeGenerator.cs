using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public abstract class BiomeGenerator : ScriptableObject
{
    public abstract bool CheckConditions(Chunk chunk);

    public abstract int GetWeight(Chunk chunk);
    public abstract Biome Generate(Chunk chunk);
}
