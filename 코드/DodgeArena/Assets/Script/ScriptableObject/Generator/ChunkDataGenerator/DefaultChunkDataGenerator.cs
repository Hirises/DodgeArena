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
    public Vector2Int dense = new Vector2Int(50, 100);
    [BoxGroup("EntityGenerate")]
    public Vector2Int risk = new Vector2Int(50, 100);
    [BoxGroup("EntityGenerate")]
    public Vector2Int _return = new Vector2Int(50, 100);
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
        return new ChunkData(Random.instance.RandRange(dense.x, dense.y), Random.instance.RandRange(risk.x, risk.y), Random.instance.RandRange(_return.x, _return.y), tagList);
    }
}
