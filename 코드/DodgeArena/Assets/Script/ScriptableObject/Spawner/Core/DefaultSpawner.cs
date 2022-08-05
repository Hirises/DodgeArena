using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "DefaultSpawner", menuName = "Spawner/Core/DefaultSpawner")]
public class DefaultSpawner : Spawner
{
    [BoxGroup("Environment")]
    public bool whiteList = true;
    [BoxGroup("Environment")]
    public List<WorldType.Type> worlds;

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
    public float groupDistance = 1;
    [BoxGroup("Individual")]
    public int minCount = 1;
    [BoxGroup("Individual")]
    public int maxCouint = 1;
    [BoxGroup("Individual")]
    public float distance = 1;
    [BoxGroup("Individual")]
    public float width = 1;
    [BoxGroup("Variant")]
    public Entity[] variants;

    private bool CheckConditions(Chunk chunk)
    {
        bool flag = true;
        if (UseRate)
        {
            flag &= Random.instance.CheckRate(rate);
        }
        flag &= !(whiteList ^ worlds.Contains(chunk.world.type.type));
        return flag;
    }

    public override List<Entity> Spawn(Chunk chunk)
    {
        if (CheckConditions(chunk))
        {
            List<Entity> entities = new List<Entity>();
            World world = chunk.world;

            //그룹별 생성
            int group = Random.instance.RandomRange(minGroup, maxGrounp);
            Vector2[] groupLocations = Util.SpreadLocation(group, chunk.location.center.vector2, groupDistance, GameManager.instance.chunkWeidth - width);
            for(int i = 0; i < group; i++)
            {
                //그룹 기준 위치 설정 & 그룹에 생성될 개채수 설정
                WorldLocation groupLocation = new WorldLocation(world, groupLocations[i]);
                int count = Random.instance.RandomRange(minGroup, maxGrounp);

                //보상(리턴) 수치 확인
                if (returns > 0)
                {
                    if (chunk.spawnData.returns + (returns * count) > chunk.spawnData.initialReturns)
                    {
                        break;
                    }
                    chunk.spawnData.returns += (returns * count);
                    chunk.spawnData.risk -= (returns * count);
                }
                //리스크 수치 확인
                if (risk > 0)
                {
                    if (chunk.spawnData.risk + (risk * count) > chunk.spawnData.initialRisk)
                    {
                        break;
                    }
                    chunk.spawnData.risk += (risk * count);
                }

                //실제 개체 생성
                Vector2[] locations = Util.SpreadLocation(count, groupLocation.vector2, distance, width);
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
