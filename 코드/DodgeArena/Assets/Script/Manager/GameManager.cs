using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField]
    public readonly SpawningData[] spawners;
    private Dictionary<ChunkLocation, Chunk> chunks;
    [SerializeField]
    public readonly float chunkLoadRange;
    [SerializeField]
    public readonly float chunkWeidth;

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

        int loadRange = (int) Mathf.Floor((chunkLoadRange - chunkWeidth / 2) / chunkWeidth);
        for(int x = -loadRange; x <= loadRange; x++)
        {
            for (int y = -loadRange; y <= loadRange; y++)
            {
                ChunkLocation chunkLocation = new ChunkLocation(new Vector2(x, y));
                Chunk chunk = new Chunk(chunkLocation);
                chunks.Add(chunkLocation, chunk);
            }
        }
    }

    private void Update()
    {
        
    }

    public Chunk GetChunk(ChunkLocation location)
    {
        return chunks[location];
    }
}
