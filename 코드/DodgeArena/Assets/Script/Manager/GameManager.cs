using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField]
    public SpawningData[] spawners;
    [SerializeField]
    public GameObject objectsRoot;
    [SerializeField]
    public GameObject chunkObject;
    [SerializeField]
    public PlayerController player;
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

    public void UpdateChunk()
    {
        int loadRange = (int)Mathf.Floor(chunkUpdateRange / chunkWeidth);
        Vector2 offset = new WorldLocation(player.transform.position).ToChunkLocation().location;
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

        if (!chunks.ContainsKey(location))
        {
            chunks.Add(location, new Chunk(location));
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
        chunks.Remove(chunk.location);
        Destroy(chunk.rootObject);
    }

    public Chunk GetChunk(ChunkLocation location)
    {
        if (!chunks.ContainsKey(location))
        {
            return LoadChunk(location);
        }
        Chunk chunk =  chunks[location];
        return chunk;
    }
}
