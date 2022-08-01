using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "SpawningData", menuName = "Scriptable Objects/Spawning Data", order = 0)]
public class SpawningData : ScriptableObject
{
    [SerializeField]
    private double spawningRate;
    [SerializeField]
    private int minCount;
    [SerializeField]
    private int maxCount;
    [SerializeField]
    private GameObject[] variants;

    public bool CanSpawnWith(Chunk chunk)
    {
        return (new System.Random()).NextDouble() < spawningRate;
    }

    [Obsolete]
    public void Spawn(Chunk chunk)
    {
        int spawnCount = UnityEngine.Random.RandomRange(minCount, maxCount);
        for(int i = 0; i < spawnCount; i++)
        {
            int variant = UnityEngine.Random.RandomRange(0, variants.Length - 1);
            Instantiate(variants[variant], chunk.RandomPosition().location);
        }
    }
}
