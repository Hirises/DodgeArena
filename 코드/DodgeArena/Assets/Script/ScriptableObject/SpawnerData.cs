using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "SpawnerData", menuName = "Scriptable Objects/Spawner Data", order = 0)]
public class SpawnerData : ScriptableObject
{
    [SerializeField]
    [Range(min:0, max:1)]
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

    public bool checkValidVariants(GameObject[] v)
    {
        return v != null && v.Length > 0;
    }

    public bool CanSpawnWith(Chunk chunk)
    {
        return (new System.Random()).NextDouble() < spawningRate;
    }

    #pragma warning disable CS0618 // 형식 또는 멤버는 사용되지 않습니다.
    public List<Entity> Spawn(Chunk chunk)
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
                Entity instance = Instantiate(variants[variant], baseLocation.Randomize(density).location, Quaternion.identity, chunk.rootObject.transform);
                instance.OnSpawn();
                entities.Add(instance);
            }
        }
        return entities;
    }
}
