using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// (0,0) 기준 정사각형 배열
public class Chunk
{
    private bool _initiated;
    private bool _loaded;
    public bool loaded { get => _loaded; }
    public bool initiated { get => _initiated; }
    public readonly ChunkLocation location;
    public readonly GameObject rootObject;


    public Chunk(ChunkLocation position)
    {
        this._initiated = false;
        this._loaded = false;
        this.location = position;
        this.rootObject = MonoBehaviour.Instantiate(GameManager.instance.chunkObject, position.centerLocation.location, Quaternion.identity, GameManager.instance.objectsRoot.transform);
    }

    public bool CheckKeep()
    {
        Vector2 pos = GameManager.instance.player.transform.position;
        return Mathf.Abs(pos.x - location.centerLocation.location.x) <= GameManager.instance.chunkSaveRange
           && Mathf.Abs(pos.y - location.centerLocation.location.y) <= GameManager.instance.chunkSaveRange;
    }


    public bool CheckLoad()
    {
        Vector2 pos = GameManager.instance.player.transform.position;
        return Mathf.Abs(pos.x - location.centerLocation.location.x) <= GameManager.instance.chunkLoadRange
           && Mathf.Abs(pos.y - location.centerLocation.location.y) <= GameManager.instance.chunkLoadRange;
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

        rootObject.SetActive(true);
        Initiate();
        this._loaded = true;
    }

    public void Unload()
    {
        if (!loaded)
        {
            return;
        }

        this._loaded = false;
        rootObject.SetActive(false);
    }

    public void Remove()
    {
        GameManager.instance.RemoveChunk(this);
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
        return location.centerLocation.Randomize(GameManager.instance.chunkWeidth / 2);
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
