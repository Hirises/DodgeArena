using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using NaughtyAttributes;

public abstract class EntityGenerator : ScriptableObject
{
    public abstract List<Entity> Generate(Chunk chunk);
}
