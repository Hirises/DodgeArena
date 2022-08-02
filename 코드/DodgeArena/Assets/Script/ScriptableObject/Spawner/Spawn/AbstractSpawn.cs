using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractSpawn : ScriptableObject
{
    public abstract List<Entity> Spawn(Chunk chunk);
}
