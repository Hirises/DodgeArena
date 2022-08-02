using UnityEngine;
using System;

public class ChunkLocation
{
    public readonly Vector2 vector;
    public Chunk chunk
    {
        get => GameManager.instance.GetChunk(this);
    }
    public WorldLocation center
    {
        get => new WorldLocation(vector * GameManager.instance.chunkWeidth);
    }

    public ChunkLocation(Vector2 location) 
    {
        location.x = Mathf.Floor((location.x + GameManager.instance.chunkWeidth / 2) / GameManager.instance.chunkWeidth);
        location.y = Mathf.Floor((location.y + GameManager.instance.chunkWeidth / 2) / GameManager.instance.chunkWeidth);
        this.vector = location;
    }

    public override bool Equals(object obj)
    {
        if ((obj == null) || !this.GetType().Equals(obj.GetType()))
        {
            return false;
        }
        else
        {
            ChunkLocation p = (ChunkLocation)obj;
            return p.vector.Equals(vector);
        }
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(vector);
    }
}
