using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "Default", menuName = "EntityGenerator/Default")]
public class DefaultEntityGenerator : EntityGenerator
{
    [BoxGroup("Environment")]
    public bool whiteListForWorld = false;
    [BoxGroup("Environment")]
    public List<WorldType.Type> worlds;
    [BoxGroup("Environment")]
    public bool whiteListForBiomes = false;
    [BoxGroup("Environment")]
    public List<Biome.Type> biomes;

    [BoxGroup("Rate")]
    public bool UseRate = false;
    [BoxGroup("Rate")]
    [ShowIf("UseRate")]
    public float rate;

    [BoxGroup("Limit")]
    public int risk;
    [BoxGroup("Limit")]
    public int returns;

    [BoxGroup("Group")]
    public int minGroup = 1;
    [BoxGroup("Group")]
    public int maxGrounp = 1;
    [BoxGroup("Group")]
    public float half_GroupDistance = 1;
    [BoxGroup("Individual")]
    public int minCount = 1;
    [BoxGroup("Individual")]
    public int maxCount = 1;
    [BoxGroup("Individual")]
    public float half_Distance = 1;
    [BoxGroup("Individual")]
    public float half_Width = 1;
    [BoxGroup("Variant")]
    public Entity[] variants;

    private bool CheckConditions(Chunk chunk)
    {
        bool flag = true;
        if (UseRate)
        {
            flag &= Random.instance.CheckRate(rate);
        }
        flag &= !(whiteListForWorld ^ worlds.Contains(chunk.world.type.type));
        return flag;
    }

    public override List<Entity> Generate(Chunk chunk)
    {
        if (CheckConditions(chunk))
        {
            List<Entity> entities = new List<Entity>();
            World world = chunk.world;

            //그룹별 생성
            int group = Random.instance.RandRange(minGroup, maxGrounp);
            List<Vector2> groupLocations = Util.SpreadLocation(group, chunk.location.center.vector2, half_GroupDistance, GameManager.instance.half_ChunkWeidth - half_Width);
            for(int i = 0; i < group; i++)
            {
                //그룹 기준 위치 설정 & 그룹에 생성될 개채수 설정
                WorldLocation groupLocation = new WorldLocation(world, groupLocations[i]);
                int count = Random.instance.RandRange(minCount, maxCount);

                //보상(리턴) 수치 확인
                if (returns > 0)
                {
                    if (chunk.chunkData.returns + (returns * count) > chunk.chunkData.initialReturns)
                    {
                        break;
                    }
                    chunk.chunkData.returns += (returns * count);
                    chunk.chunkData.risk -= (returns * count);
                }
                //리스크 수치 확인
                if (risk > 0)
                {
                    if (chunk.chunkData.risk + (risk * count) > chunk.chunkData.initialRisk)
                    {
                        break;
                    }
                    chunk.chunkData.risk += (risk * count);
                }

                //실제 개체 생성
                List<Vector2> locations = Util.SpreadLocation(count, groupLocation.vector2, half_Distance, half_Width);
                for (int j = 0; j < count; j++)
                {
                    WorldLocation location = new WorldLocation(world, locations[j]);
                    chunk.world.Spawn(variants[Random.instance.RandInt(0, variants.Length)], location);
                }
            }

            return entities;
        }
        return new List<Entity>();
    }
}
