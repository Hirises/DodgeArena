using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public abstract class ChunkDataGenerator : ScriptableObject {
    [BoxGroup("Common")]
    public int weight = 100;

    public abstract bool CheckConditions(Chunk chunk);

    public abstract ChunkData Generate(Chunk chunk);
}
