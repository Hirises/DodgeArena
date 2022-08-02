using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// (0,0) 기준 정사각형 배열
public class Chunk
{
    public readonly ChunkLocation postion;

    public Chunk(ChunkLocation postion)
    {
        this.postion = postion;
    }

    //이 청크에 오브잭트들을 소환
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

    // 청크내의 랜덤한 위치를 반환
    public WorldLocation RandomPosition()
    {
        return postion.CenterLocation().Randomize(GameManager.instance.chunkWeidth / 2);
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
            return p.postion.Equals(postion);
        }
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(postion);
    }
}
