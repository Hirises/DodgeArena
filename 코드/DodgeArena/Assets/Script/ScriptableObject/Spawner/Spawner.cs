using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using NaughtyAttributes;

public abstract class Spawner : ScriptableObject
{
    public abstract bool CanSpawnWith(Chunk chunk);

    public abstract List<Entity> Spawn(Chunk chunk);

/*    [SerializeField]
    [Range(min: 0, max: 1)]
    private double spawningRate;
    [SerializeField]
    private int minGroupCount = 1;
    [SerializeField]
    private int maxGroupCount = 1;
    [SerializeField]
    private int minCount = 1;
    [SerializeField]
    private int maxCount = 1;
    [SerializeField]
    private float density = 2.0f;
    [SerializeField]
    [ValidateInput("checkValidVariants")]
    private Entity[] variants;

    public override bool checkValidVariants(GameObject[] v)
    {
        return v != null && v.Length > 0;
    }

    public override bool CanSpawnWith(Chunk chunk)
    {
        return (new System.Random()).NextDouble() < spawningRate;
    }

#pragma warning disable CS0618 // 형식 또는 멤버는 사용되지 않습니다.
    public override List<Entity> Spawn(Chunk chunk)
    {
        List<Entity> entities = new List<Entity>();
        int groupCount = UnityEngine.Random.RandomRange(minGroupCount, maxGroupCount + 1);
        for (int j = 0; j < groupCount; j++)
        {
            WorldLocation baseLocation = chunk.RandomPosition();
            int spawnCount = UnityEngine.Random.RandomRange(minCount, maxCount + 1);
            for (int i = 0; i < spawnCount; i++)
            {
                int variant = UnityEngine.Random.RandomRange(0, variants.Length);
                GameManager.instance.Spawn(variants[variant], baseLocation.Randomize(density), chunk);
            }
        }
        return entities;
    }*/
}
