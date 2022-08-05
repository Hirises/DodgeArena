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

            //그룹별 생성
            int group = Random.instance.RandomRange(minGroup, maxGrounp);
            for(int i = 0; i < group; i++)
            {
                //그룹 기준 위치 설정 & 그룹에 생성될 개채수 설정
                WorldLocation groupLocation = chunk.RandomLocation(dense);
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
