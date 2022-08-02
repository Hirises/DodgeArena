using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

// (0,0) 기준 정사각형 배열
public class Chunk : MonoBehaviour
{
    private bool _initiated;
    private bool _loaded;
    public bool loaded { get => _loaded; }
    public bool initiated { get => _initiated; }
    private ChunkLocation _location;
    public ChunkLocation location
    {
        get => _location;
    }
    public readonly GameObject rootObject;
    [ReadOnly]
    public List<Entity> entities;

    public bool CheckKeep()
    {
        Vector2 pos = GameManager.instance.player.transform.position;
        return Mathf.Abs(pos.x - location.center.vector.x) <= GameManager.instance.chunkSaveRange
           && Mathf.Abs(pos.y - location.center.vector.y) <= GameManager.instance.chunkSaveRange;
    }


    public bool CheckLoad()
    {
        Vector2 pos = GameManager.instance.player.transform.position;
        return Mathf.Abs(pos.x - location.center.vector.x) <= GameManager.instance.chunkLoadRange
           && Mathf.Abs(pos.y - location.center.vector.y) <= GameManager.instance.chunkLoadRange;
    }

    /// <summary>
    /// 청크 초기화 (처음 생성)
    /// </summary>
    public void Initiate(ChunkLocation position)
    {
        if (initiated)
        {
            return;
        }

        this._initiated = false;
        this._loaded = false;
        this._location = position;
        entities = new List<Entity>();
        SpawnObjects();
        _initiated = true;
    }

    public void Load()
    {
        if (loaded)
        {
            return;
        }

        rootObject.SetActive(true);
        this._loaded = true;
        foreach(Entity entity in entities)
        {
            entity.OnLoad();
        }
    }

    public void Unload()
    {
        if (!loaded)
        {
            return;
        }

        this._loaded = false;
        foreach (Entity entity in entities)
        {
            entity.OnUnload();
        }
        rootObject.SetActive(false);
    }

    public void Remove()
    {
        GameManager.instance.RemoveChunk(this);
    }

    //이 청크에 오브잭트들을 소환
    public void SpawnObjects()
    {
        foreach(SpawnerData spawner in GameManager.instance.spawners)
        {
            if (spawner.CanSpawnWith(this))
            {
                entities.AddRange(spawner.Spawn(this));
            }
        }
    }

    // 청크내의 랜덤한 위치를 반환
    public WorldLocation RandomPosition()
    {
        return location.center.Randomize(GameManager.instance.chunkWeidth / 2);
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
            return p.location.Equals(location);
        }
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(location);
    }
}
