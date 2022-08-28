using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "Default", menuName = "Generator/EntityGenerator/Default")]
public class DefaultEntityGenerator : EntityGenerator
{
    [BoxGroup("Common")]
    public int weight = 100;

    [BoxGroup("Environment")]
    public bool whiteListForWorld = false;
    [BoxGroup("Environment")]
    public List<WorldTypeEnum> worlds;
    [BoxGroup("Environment")]
    public bool whiteListForBiome = false;
    [BoxGroup("Environment")]
    public List<BiomeTypeEnum> biomes;

    [BoxGroup("Limit")]
    public int dense;
    [BoxGroup("Limit")]
    public int risk;
    [BoxGroup("Limit")]
    public int returns;
    [BoxGroup("Limit")]
    public bool useTag;
    [BoxGroup("Limit")]
    [ShowIf(nameof(useTag))]
    public bool containAll;
    [BoxGroup("Limit")]
    [ReorderableList]
    [ShowIf(nameof(useTag))]
    public List<string> tags;

    [BoxGroup("Group")]
    public Vector2Int group = new Vector2Int(1, 1);
    [BoxGroup("Group")]
    public float half_GroupDistance = 1;
    [BoxGroup("Individual")]
    public Vector2Int count = new Vector2Int(1, 1);
    [BoxGroup("Individual")]
    public float half_Distance = 1;
    [BoxGroup("Individual")]
    public float half_Width = 1;
    [BoxGroup("Variant")]
    public List<Entity> variants;

    public override bool CheckConditions(Chunk chunk)
    {
        bool flag = true;
        flag &= !(whiteListForWorld ^ worlds.Contains(chunk.world.type));
        flag &= !( whiteListForBiome ^ biomes.Contains(chunk.biome.enumType) );
        flag &= chunk.chunkData.dense + dense <= chunk.chunkData.initialDense;
        flag &= chunk.chunkData.risk + risk <= chunk.chunkData.initialRisk;
        flag &= chunk.chunkData.returns + returns <= chunk.chunkData.initialReturns;
        //태그 검사
        if(useTag) {
            if(containAll) {
                foreach(string tag in tags) {
                    flag &= chunk.chunkData.tags.Contains(tag);
                }
            } else {
                bool innerFlag = false;
                foreach(string tag in tags) {
                    if(chunk.chunkData.tags.Contains(tag)) {
                        innerFlag = true;
                        break;
                    }
                }
                flag &= innerFlag;
            }
        }
        return flag;
    }

    public override int GetWeight() {
        return weight;
    }
    public override List<Entity> Generate(Chunk chunk)
    {
        List<Entity> entities = new List<Entity>();
        World world = chunk.world;

        chunk.chunkData.dense += dense;
        chunk.chunkData.risk += risk;
        chunk.chunkData.returns += returns;
        //태그 제거
        if(useTag) {
            if(containAll) {
                foreach(string tag in tags) {
                    chunk.chunkData.tags.Remove(tag);
                }
            } else {
                foreach(string tag in tags) {
                    if(chunk.chunkData.tags.Contains(tag)) {
                        chunk.chunkData.tags.Remove(tag);
                        break;
                    }
                }
            }
        }

        //그룹별 생성
        int group = Random.instance.RandRange(this.group.x, this.group.y);
        List<Vector2> groupLocations = Util.SpreadLocation(group, chunk.location.center.vector2, half_GroupDistance, GameManager.instance.half_ChunkWeidth - half_Width);
        for(int i = 0; i < group; i++) {
            //그룹 기준 위치 설정 & 그룹에 생성될 개채수 설정
            WorldLocation groupLocation = new WorldLocation(world, groupLocations[i]);
            int count = Random.instance.RandRange(this.count.x, this.count.y);

            //실제 개체 생성
            List<Vector2> locations = Util.SpreadLocation(count, groupLocation.vector2, half_Distance, half_Width);
            for(int j = 0; j < count; j++) {
                WorldLocation location = new WorldLocation(world, locations[j]);
                chunk.world.Spawn(variants[Random.instance.RandInt(0, variants.Count)], location);
            }
        }

        return entities;
    }
}
