using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

// (0,0) ���� ���簢�� �迭
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
    [HideInInspector]
    public List<Entity> entities;
    public SpawnData spawnData;

    /// <summary>
    /// ûũ ���� (ó�� ������)
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
        this.spawnData = GameManager.instance.spawnDataSetter.GenerateNewSpawnData();
        entities = new List<Entity>();
        _valid = true;
    }

    /// <summary>
    /// ûũ �ʱ�ȭ (ó�� Load�� ��)
    /// </summary>
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

    //�� ûũ�� ������Ʈ���� ��ȯ
    public void SpawnObjects()
    {
        foreach(Spawner spawner in GameManager.instance.spawners)
        {
            spawner.Spawn(this);
        }
    }

    // ûũ���� ������ ��ġ�� ��ȯ
    public WorldLocation RandomLocation()
    {
        return RandomLocation(0);
    }

    public WorldLocation RandomLocation(float margin)
    {
        return location.center.Randomize((GameManager.instance.chunkWeidth - margin) / 2);
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
