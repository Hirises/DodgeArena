using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using RotaryHeart.Lib.SerializableDictionary;

[CreateAssetMenu(fileName = "Default", menuName = "Generator/ChunkDataGenerator/Default")]
public class DefaultChunkDataGenerator : ChunkDataGenerator {
    [BoxGroup("Common")]
    public int weight = 100;

    [BoxGroup("Environment")]
    public bool whiteListForWorld = false;
    [BoxGroup("Environment")]
    public List<WorldType.Type> worlds;
    [BoxGroup("Environment")]
    public bool whiteListForBiome = false;
    [BoxGroup("Environment")]
    public List<BiomeTypeEnum> biomes;

    [BoxGroup("EntityGenerate")]
    public int minDense;
    [BoxGroup("EntityGenerate")]
    public int maxDense;
    [BoxGroup("EntityGenerate")]
    public int minRisk;
    [BoxGroup("EntityGenerate")]
    public int maxRisk;
    [BoxGroup("EntityGenerate")]
    public int minReturn;
    [BoxGroup("EntityGenerate")]
    public int maxReturn;
    [BoxGroup("EntityGenerate")]
    public SerializableDictionaryBase<string, float> tags;

    public override bool CheckConditions(Chunk chunk) {
        bool flag = true;
        flag &= !( whiteListForWorld ^ worlds.Contains(chunk.world.type) );
        flag &= !( whiteListForBiome ^ biomes.Contains(chunk.biome.enumType) );
        return flag;
    }

    public override int GetWeight() {
        return weight;
    }

    public override ChunkData Generate(Chunk chunk) {
        List<string> tagList = new List<string>();
        foreach(string tag in tags.Keys) {
            if(Random.instance.CheckRate(tags[tag])) {
                tagList.Add(tag);
            }
        }
        return new ChunkData(Random.instance.RandRange(minDense, maxDense), Random.instance.RandRange(minRisk, maxRisk), Random.instance.RandRange(minReturn, maxReturn), tagList);
    }
}
