using UnityEngine;

public class ChunkLocation
{
    public readonly Vector2 location;

    public ChunkLocation(Vector2 location) 
    {
        this.location = location;
    }

    public WorldLocation CenterLocation(float z)
    {
        Vector2 center = location * GameManager.instance.chunkWeidth;
        return new WorldLocation(new Vector3(center.x, center.y, z));
    }

    public Chunk GetChunk()
    {
        return GameManager.instance.GetChunk(this);
    }
}
