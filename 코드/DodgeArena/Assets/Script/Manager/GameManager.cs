using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField]
    public SpawnerData[] spawners;
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

    public void Spawn<T>(T target, WorldLocation location) where T : Entity
    {
        Entity instance = Instantiate(target, location.vector, Quaternion.identity, location.chunk.rootObject.transform);
        instance.location = location;
        instance.OnSpawn();
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
        Chunk chunk = GetChunk(location);
        if (chunk.CheckLoad())
        {
            chunk.Load();
        }
        else if (chunk.CheckKeep())
        {
            chunk.Unload();
        }
        else
        {
            chunk.Remove();
        }
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

    public Chunk SpawnChunk(ChunkLocation location)
    {
        if (chunks.ContainsKey(location))
        {
            return GetChunk(location);
        }

        Chunk chunk = Instantiate(chunkObject, location.center.vector, Quaternion.identity, objectsRoot.transform);
        chunk.Initiate(location);
        chunks.Add(location, chunk);
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
            entity.OnDespawn();
        }
        chunks.Remove(chunk.location);
        Destroy(chunk.rootObject);
    }

    public Chunk GetChunk(ChunkLocation location)
    {
        if (!chunks.ContainsKey(location))
        {
            return SpawnChunk(location);
        }
        Chunk chunk =  chunks[location];
        return chunk;
    }
}
