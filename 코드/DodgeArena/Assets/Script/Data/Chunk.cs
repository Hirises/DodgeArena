using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

// (0,0) 기준 정사각형 배열
public class Chunk : MonoBehaviour
{
    private bool _valid = false;
    private bool _initiated = false;
    private bool _loaded;
    public bool loaded { get => _loaded; }
    public bool initiated { get => _initiated; }
    public bool valid { get => _valid; }
    [SerializeField]
    [ReadOnly]
    private ChunkLocation _location;
    public ChunkLocation location
    {
        get => _location;
    }
    public readonly GameObject rootObject;
    [ReadOnly]
    public List<Entity> entities;

    /// <summary>
    /// 청크 초기화 (처음 생성)
    /// </summary>
    public void ResetProperties(ChunkLocation position)
    {
        if (valid)
        {
            return;
        }

        gameObject.SetActive(false);
        this._loaded = false;
        this._initiated = false;
        this._location = position;
        entities = new List<Entity>();
        _valid = true;
    }

    public void Initiate()
    {
        if (initiated)
        {
            return;
        }

        SpawnObjects();
        _initiated = true;
    }

    public void Load()
    {
        if (loaded)
        {
            return;
        }

        gameObject.SetActive(true);
        this._loaded = true;
        Initiate();
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
        gameObject.SetActive(false);
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
