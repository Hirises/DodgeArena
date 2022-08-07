using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public abstract class ChunkDataGenerator : Generator<ChunkData> {
    public override abstract bool CheckConditions(Chunk chunk);

    public override abstract int GetWeight(Chunk chunk);

    public override abstract ChunkData Generate(Chunk chunk);
}
