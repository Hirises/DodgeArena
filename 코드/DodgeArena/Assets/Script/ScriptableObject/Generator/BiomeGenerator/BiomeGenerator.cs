using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public abstract class BiomeGenerator : Generator<Biome>
{
    public override abstract bool CheckConditions(Chunk chunk);

    public override abstract int GetWeight(Chunk chunk);
    public override abstract Biome Generate(Chunk chunk);
}
