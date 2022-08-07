using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public abstract class ChunkDataGenerator : ScriptableObject {
    public abstract bool CheckConditions(Chunk chunk);

    public abstract int GetWeight(Chunk chunk);

    public abstract ChunkData Generate(Chunk chunk);
}
