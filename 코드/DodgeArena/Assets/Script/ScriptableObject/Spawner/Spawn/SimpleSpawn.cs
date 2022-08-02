using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SimpleSpawn", menuName = "Spawner/Spawn/SimpleSpawn", order = 0)]
public class SimpleSpawn : AbstractSpawn
{
    [SerializeField]
    private Entity entity;

    public override List<Entity> Spawn(Chunk chunk)
    {
        List<Entity> entities = new List<Entity>();
        entities.Add(GameManager.instance.Spawn(entity, chunk.location.center));
        return entities;
    }
}
