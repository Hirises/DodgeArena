using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public abstract class BiomeGenerator : ScriptableObject, IHasWeight
{
    public abstract bool CheckConditions(ChunkLocation chunk, BiomeInfo info);

    public abstract int GetWeight();

    public abstract Biome Generate();
}
