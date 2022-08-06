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
        flag &= !(whiteList ^ worlds.Contains(chunk.world.type.type));
        return flag;
    }

    public override List<Entity> Spawn(Chunk chunk)
    {
        if (CheckConditions(chunk))
        {
            List<Entity> entities = new List<Entity>();
            World world = chunk.world;

            //�׷캰 ����
            int group = Random.instance.RandomRange(minGroup, maxGrounp);
            Vector2[] groupLocations = Util.SpreadLocation(group, chunk.location.center.vector2, half_GroupDistance, GameManager.instance.half_ChunkWeidth - half_Width);
            for(int i = 0; i < group; i++)
            {
                //�׷� ���� ��ġ ���� & �׷쿡 ������ ��ä�� ����
                WorldLocation groupLocation = new WorldLocation(world, groupLocations[i]);
                int count = Random.instance.RandomRange(minCount, maxCount);

                //����(����) ��ġ Ȯ��
                if (returns > 0)
                {
                    if (chunk.spawnData.returns + (returns * count) > chunk.spawnData.initialReturns)
                    {
                        break;
                    }
                    chunk.spawnData.returns += (returns * count);
                    chunk.spawnData.risk -= (returns * count);
                }
                //����ũ ��ġ Ȯ��
                if (risk > 0)
                {
                    if (chunk.spawnData.risk + (risk * count) > chunk.spawnData.initialRisk)
                    {
                        break;
                    }
                    chunk.spawnData.risk += (risk * count);
                }

                //���� ��ü ����
                Vector2[] locations = Util.SpreadLocation(count, groupLocation.vector2, half_Distance, half_Width);
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
