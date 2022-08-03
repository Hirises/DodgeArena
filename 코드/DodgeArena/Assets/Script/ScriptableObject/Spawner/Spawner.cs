using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using NaughtyAttributes;

public abstract class Spawner : ScriptableObject
{
    public abstract List<Entity> Spawn(Chunk chunk);
}
