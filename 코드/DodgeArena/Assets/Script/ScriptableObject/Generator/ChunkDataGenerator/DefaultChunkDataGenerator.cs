using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "Default", menuName = "ChunkDataGenerator/Default")]
public class DefaultChunkDataGenerator : ChunkDataGenerator {
    [BoxGroup("Common")]
    public int weight = 100;

    [BoxGroup("Environment")]
    public bool whiteListForWorld = false;
    [BoxGroup("Environment")]
    public List<WorldType.Type> worlds;
    [BoxGroup("Environment")]
    public Biome.Type biome;

    [BoxGroup("EntityGenerate")]
    public int minRisk;
    [BoxGroup("EntityGenerate")]
    public int maxRisk;
    [BoxGroup("EntityGenerate")]
    public int minReturn;
    [BoxGroup("EntityGenerate")]
    public int maxReturn;

    public override bool CheckConditions(Chunk chunk) {
        bool flag = true;
        flag &= !( whiteListForWorld ^ worlds.Contains(chunk.world.type.type) );
        flag &= chunk.biomeInfo.affectedBiomes.ContainsKey(biome);
        return flag;
    }

    public override int GetWeight(Chunk chunk) {
        return Mathf.CeilToInt(weight * chunk.biomeInfo.affectedBiomes[biome]);
    }

    public override ChunkData Generate(Chunk chunk) {
        return new ChunkData(Random.instance.RandRange(minRisk, maxRisk), Random.instance.RandRange(minReturn, maxReturn));
    }
}
