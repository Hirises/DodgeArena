using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class DefaultBiomeGenerator : BiomeGenerator {
    [BoxGroup("Common")]
    public int weight = 100;

    [BoxGroup("Environment")]
    public bool whiteListForWorld = false;
    [BoxGroup("Environment")]
    public List<WorldType.Type> worlds;

    [BoxGroup("Biome")]
    public Biome.Type biome;
    public override bool CheckConditions(Chunk chunk) {
        bool flag = true;
        flag &= !( whiteListForWorld ^ worlds.Contains(chunk.world.type.type) );
        return flag;
    }

    public override int GetWeight(Chunk chunk) {
        return weight;
    }

    public override Biome Generate(Chunk chunk) {
        return biome;
    }
}
