using UnityEngine;
using System;

public class ChunkLocation
{
    public readonly Vector2 location;
    public WorldLocation centerLocation
    {
        get => new WorldLocation(location * GameManager.instance.chunkWeidth);
    }

    public ChunkLocation(Vector2 location) 
    {
        location.x = Mathf.Floor(location.x);
        location.y = Mathf.Floor(location.y);
        this.location = location;
    }

    public Chunk GetChunk()
    {
        return GameManager.instance.GetChunk(this);
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
            return p.location.Equals(location);
        }
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(location);
    }
}
