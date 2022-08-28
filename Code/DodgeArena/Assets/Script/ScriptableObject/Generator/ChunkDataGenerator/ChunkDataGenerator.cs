using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public abstract class ChunkDataGenerator : ScriptableObject, IHasWeight {
    public abstract bool CheckConditions(Chunk chunk);

    public abstract int GetWeight();

    public abstract ChunkData Generate(Chunk chunk);
}
