using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "Default", menuName = "Generator/BiomeGenerator/Default")]
public class DefaultBiomeGenerator : BiomeGenerator {

    [BoxGroup("Environment")]
    public bool whiteListForWorld = false;
    [BoxGroup("Environment")]
    public List<WorldTypeEnum> worlds;
    [BoxGroup("Environment")]
    public Vector2 difficulty = new Vector2(0, 1);
    [BoxGroup("Environment")]
    public Vector2 temperature = new Vector2(0, 1);

    [BoxGroup("Biome")]
    public BiomeTypeEnum biome;
    public override bool CheckConditions(ChunkLocation location, BiomeInfo info) {
        bool flag = true;
        flag &= !( whiteListForWorld ^ worlds.Contains(location.world.type) );
        flag &= difficulty.x <= info.dificulty && info.dificulty <= difficulty.y;
        flag &= temperature.x <= info.temperature && info.temperature <= temperature.y;
        return flag;
    }

    public override Biome Generate() {
        return biome;
    }
}
