using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField]
    public Spawner[] spawners;
    [SerializeField]
    public GameObject objectsRoot;
    [SerializeField]
    public Chunk chunkObject;
    [SerializeField]
    public Player player;
    [SerializeField]
    public float chunkUpdateRange;
    [SerializeField]
    public float chunkSaveRange;
    [SerializeField]
    public float chunkLoadRange;
    [SerializeField]
    public float chunkWeidth;
    [SerializeField]
    public SpawnDataSetter spawnDataSetter;

    private Dictionary<ChunkLocation, Chunk> chunks = new Dictionary<ChunkLocation, Chunk>();

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(transform);
        }

        UpdateChunk();
    }

    private void Update()
    {
        UpdateChunk();
    }

    public T Spawn<T>(T target, WorldLocation location) where T : Entity
    {
        Chunk chunk = location.chunk;
        T instance = Instantiate(target, location.vector, Quaternion.identity, chunk.gameObject.transform);
        instance.location = location;
        instance.OnSpawn();
        if (chunk.loaded)
        {
            instance.OnLoad();
        }
        else
        {
            instance.OnUnload();
        }
        return instance;
    }

    public void UpdateChunk()
    {
        int loadRange = (int)Mathf.Floor(chunkUpdateRange / chunkWeidth);
        Vector2 offset = new WorldLocation(player.transform.position).chunkLocation.vector;
        for (int x = -loadRange; x <= loadRange; x++)
        {
            for (int y = -loadRange; y <= loadRange; y++)
            {
                ChunkLocation location = new ChunkLocation(new Vector2(x + offset.x, y + offset.y));
                CheckChunk(location);
            }
        }
    }

    public void CheckChunk(ChunkLocation location)
    {
        if (location.CheckLoad())
        {
            if (!chunks.ContainsKey(location))
            {
                SpawnChunk(location);
            }
            LoadChunk(location);
        }
        else if (location.CheckKeep())
        {
            if (!chunks.ContainsKey(location))
            {
                SpawnChunk(location);
                return;
            }
            UnloadChunk(location);
        }
        else
        {
            RemoveChunk(location);
        }
    }

    public Chunk GetChunk(ChunkLocation location)
    {
        if (!chunks.ContainsKey(location))
        {
            return SpawnChunk(location);
        }
        Chunk chunk = chunks[location];
        return chunk;
    }

    public Chunk SpawnChunk(ChunkLocation location)
    {
        if (chunks.ContainsKey(location))
        {
            return GetChunk(location);
        }

        Chunk chunk = Instantiate(chunkObject, location.center.vector, Quaternion.identity, objectsRoot.transform);
        chunks.Add(location, chunk);
        chunk.ResetProperties(location);
        return chunk;
    }

    public Chunk LoadChunk(ChunkLocation location)
    {
        if (chunks.ContainsKey(location) && GetChunk(location).loaded)
        {
            return GetChunk(location);
        }
        Chunk chunk = GetChunk(location);
        chunk.Load();
        return chunk;
    }

    public void UnloadChunk(ChunkLocation location)
    {
        if (!chunks.ContainsKey(location) || !GetChunk(location).loaded)
        {
            return;
        }

        GetChunk(location).Unload();
    }

    public void RemoveChunk(ChunkLocation location)
    {
        if (!chunks.ContainsKey(location))
        {
            return;
        }

        RemoveChunk(GetChunk(location));
    }

    public void RemoveChunk(Chunk chunk)
    {
        chunk.Unload();
        foreach (Entity entity in chunk.entities)
        {
            entity.Remove();
        }
        chunks.Remove(chunk.location);
        Destroy(chunk.gameObject);
    }
}
