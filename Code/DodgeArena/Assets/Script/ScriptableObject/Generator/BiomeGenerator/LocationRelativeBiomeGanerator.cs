using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LocationRelativeBiomeGenerator", menuName = "Generator/BiomeGenerator/LocationRelative")]
public class LocationRelativeBiomeGanerator : BiomeGenerator {
    [BoxGroup("Environment")]
    public bool whiteListForWorld = false;
    [BoxGroup("Environment")]
    public List<WorldTypeEnum> worlds;
    [BoxGroup("Environment")]
    [SerializeField]
    private Vector2 from;
    [BoxGroup("Environment")]
    [SerializeField]
    private Vector2 to;

    [BoxGroup("Biome")]
    [SerializeField]
    private BiomeTypeEnum biome;

    public override bool CheckConditions(ChunkLocation chunk, BiomeInfo info) {
        bool flag = true;
        flag &= !( whiteListForWorld ^ worlds.Contains(chunk.world.type) );
        flag &= Util.IsIn(chunk.vector, from, to);
        return flag;
    }

    public override Biome Generate() {
        return biome;
    }
}