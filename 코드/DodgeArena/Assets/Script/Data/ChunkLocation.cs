using UnityEngine;

public class ChunkLocation
{
    public readonly Vector2 location;

    public ChunkLocation(Vector2 location) 
    {
        this.location = location;
    }

    public WorldLocation CenterLocation()
    {
        return new WorldLocation(location * GameManager.instance.chunkWeidth);
    }

    public Chunk GetChunk()
    {
        return GameManager.instance.GetChunk(this);
    }
}
