using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "DefaultSpawner", menuName = "Spawner/Core/DefaultSpawner")]
public class DefaultSpawner : Spawner
{
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
    [BoxGroup("Individual")]
    public int minCount = 1;
    [BoxGroup("Individual")]
    public int maxCouint = 1;
    [BoxGroup("Individual")]
    public float dense = 1;
    [BoxGroup("Variant")]
    public Entity[] variants;

    private bool CheckConditions(Chunk chunk)
    {
        bool flag = true;
        if (UseRate)
        {
            flag &= Random.instance.CheckRate(rate);
        }
        return flag;
    }

    public override List<Entity> Spawn(Chunk chunk)
    {
        if (CheckConditions(chunk))
        {
            List<Entity> entities = new List<Entity>();

            //�׷캰 ����
            int group = Random.instance.RandomRange(minGroup, maxGrounp);
            for(int i = 0; i < group; i++)
            {
                //�׷� ���� ��ġ ���� & �׷쿡 ������ ��ä�� ����
                WorldLocation groupLocation = chunk.RandomLocation(dense);
                int count = Random.instance.RandomRange(minGroup, maxGrounp);

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
                for (int j = 0; j < count; j++)
                {
                    WorldLocation location = groupLocation.Randomize(dense);
                    chunk.world.Spawn(variants[Random.instance.RandInt(0, variants.Length)] ,location);
                }
            }

            return entities;
        }
        return new List<Entity>();
    }
}
