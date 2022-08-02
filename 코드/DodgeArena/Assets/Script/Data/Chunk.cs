using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// (0,0) ���� ���簢�� �迭
public class Chunk
{
    public readonly ChunkLocation position;
    public readonly GameObject rootObject;

    public Chunk(ChunkLocation position)
    {
        this.position = position;
        this.rootObject = MonoBehaviour.Instantiate(GameManager.instance.chunkObject, position.CenterLocation().location, Quaternion.identity, GameManager.instance.objectsRoot.transform);
    }

    //�� ûũ�� ������Ʈ���� ��ȯ
    public void SpawnObjects()
    {
        foreach(SpawningData spawner in GameManager.instance.spawners)
        {
            if (spawner.CanSpawnWith(this))
            {
                spawner.Spawn(this);
            }
        }
    }

    // ûũ���� ������ ��ġ�� ��ȯ
    public WorldLocation RandomPosition()
    {
        return position.CenterLocation().Randomize(GameManager.instance.chunkWeidth / 2);
    }

    public override bool Equals(object obj)
    {
        if ((obj == null) || !this.GetType().Equals(obj.GetType()))
        {
            return false;
        }
        else
        {
            Chunk p = (Chunk)obj;
            return p.position.Equals(position);
        }
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(position);
    }
}
