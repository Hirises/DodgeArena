using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using NaughtyAttributes;

public abstract class EntityGenerator : Generator<List<Entity>>
{
    public override abstract bool CheckConditions(Chunk chunk);

    public override abstract int GetWeight(Chunk chunk);

    public override abstract List<Entity> Generate(Chunk chunk);
}
