using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public abstract class BiomeGenerator : ScriptableObject, IHasWeight {
    [BoxGroup("Common")]
    public int weight = 100;
    [BoxGroup("Common")]
    public int priority = 1;

    public abstract bool CheckConditions(ChunkLocation chunk, BiomeInfo info);

    public int GetWeight() {
        return weight;
    }

    public int GetPriority() {
        return priority;
    }

    public abstract Biome Generate();
}
