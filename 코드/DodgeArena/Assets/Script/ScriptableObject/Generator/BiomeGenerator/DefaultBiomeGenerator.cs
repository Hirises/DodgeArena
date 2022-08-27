using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "Default", menuName = "Generator/BiomeGenerator/Default")]
public class DefaultBiomeGenerator : BiomeGenerator {
    [BoxGroup("Common")]
    public int weight = 100;

    [BoxGroup("Environment")]
    public bool whiteListForWorld = false;
    [BoxGroup("Environment")]
    public List<WorldType.Type> worlds;
    [BoxGroup("Environment")]
    public Vector2 difficulty;
    [BoxGroup("Environment")]
    public Vector2 temperature;

    [BoxGroup("Biome")]
    public BiomeTypeEnum biome;
    public override bool CheckConditions(ChunkLocation location, BiomeInfo info) {
        bool flag = true;
        flag &= !( whiteListForWorld ^ worlds.Contains(location.world.type) );
        flag &= difficulty.x <= info.dificulty && info.dificulty <= difficulty.y;
        flag &= temperature.x <= info.temperature && info.temperature <= temperature.y;
        return flag;
    }

    public override int GetWeight() {
        return weight;
    }

    public override Biome Generate() {
        return biome;
    }
}
