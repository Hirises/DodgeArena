using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "Default", menuName = "ChunkDataGenerator/Default")]
public class DefaultChunkDataGenerator : ChunkDataGenerator {
    [BoxGroup("Environment")]
    public bool whiteListForWorld = false;
    [BoxGroup("Environment")]
    public List<WorldType.Type> worlds;
    [BoxGroup("Environment")]
    public bool whiteListForBiomes = false;
    [BoxGroup("Environment")]
    public List<Biome.Type> biomes;

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
        flag &= !( whiteListForBiomes ^ biomes.Contains(chunk.biome.type) );
        return flag;
    }

    public override ChunkData Generate(Chunk chunk) {
        return new ChunkData(Random.instance.RandomRange(minRisk, maxRisk), Random.instance.RandomRange(minReturn, maxReturn));
    }
}
