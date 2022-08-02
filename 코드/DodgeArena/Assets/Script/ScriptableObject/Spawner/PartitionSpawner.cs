using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PartitionSpawner", menuName = "Spawner/Runner/PartitonSpawner", order = 0)]
public class PartitionSpawner : Spawner
{
    [SerializeField]
    private AbstractCondition[] conditions;
    [SerializeField]
    private AbstractSpawn spawn;

    public override bool CanSpawnWith(Chunk chunk)
    {
        bool flag = true;
        foreach(AbstractCondition condition in conditions){
            flag &= condition.CheckCondition(chunk);
        }
        return flag;
    }

    public override List<Entity> Spawn(Chunk chunk)
    {
        return spawn.Spawn(chunk);
    }
}
