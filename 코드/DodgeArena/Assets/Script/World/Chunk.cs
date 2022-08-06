using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

// (0,0) 기준 정사각형 배열
public class Chunk : MonoBehaviour
{
    public bool loaded { get; private set; }
    public bool initiated { get; private set; }
    public bool valid { get; private set; }
    public World world { private set; get; }
    public Biome biome { private set; get; }
    [SerializeField]
    [ReadOnly]
    private ChunkLocation _location;
    public ChunkLocation location
    {
        get => _location;
    }
    [HideInInspector]
    public List<Entity> entities;
    public ChunkData chunkData;

    /// <summary>
    /// 청크 리셋 (처음 생성시)
    /// </summary>
    public void ResetProperties(ChunkLocation position, Biome biome)
    {
        if (valid)
        {
            return;
        }

        gameObject.SetActive(false);
        this.loaded = false;
        this.initiated = false;
        this._location = position;
        world = position.world;
        this.biome = biome;
        entities = new List<Entity>();
        this.chunkData = GameManager.instance.GetChunkDataGenerator(this).Generate(this);
        valid = true;
#if UNITY_EDITOR
        gameObject.name = position.vector.ToString();
#endif
    }

    /// <summary>
    /// 청크 초기화 (처음 Load될 때)
    /// </summary>
    public void Initiate()
    {
        if (initiated)
        {
            return;
        }

        SpawnObjects();
        initiated = true;
    }

    public void Load()
    {
        if (loaded)
        {
            return;
        }

        gameObject.SetActive(true);
        Initiate();
        foreach(Entity entity in entities)
        {
            entity.OnLoad();
        }
        this.loaded = true;
    }

    public void Unload()
    {
        if (!loaded)
        {
            return;
        }

        this.loaded = false;
        foreach (Entity entity in entities)
        {
            entity.OnUnload();
        }
        gameObject.SetActive(false);
    }

    public void Remove()
    {
        world.RemoveChunk(this);
    }

    //이 청크에 오브잭트들을 소환
    public void SpawnObjects()
    {
        foreach(EntityGenerator spawner in GameManager.instance.entityGenerators)
        {
            spawner.Generate(this);
        }
    }

    // 청크내의 랜덤한 위치를 반환
    public WorldLocation RandomLocation()
    {
        return RandomLocation(0);
    }

    public WorldLocation RandomLocation(float margin)
    {
        return location.center.Randomize((GameManager.instance.half_ChunkWeidth - margin) / 2);
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
