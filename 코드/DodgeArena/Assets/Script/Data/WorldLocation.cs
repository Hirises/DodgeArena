using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WorldLocation
{
    public readonly Vector3 vector;
    public ChunkLocation chunkLocation
    {
        get => new ChunkLocation(new Vector2(Mathf.Floor((vector.x + GameManager.instance.chunkWeidth / 2) / GameManager.instance.chunkWeidth),
            Mathf.Floor((vector.y + GameManager.instance.chunkWeidth / 2) / GameManager.instance.chunkWeidth)));
    }
    public Chunk chunk
    {
        get => chunkLocation.chunk;
    }
    public Vector2 flattenLocation
    {
        get => new Vector2(vector.x, vector.y);
    }

    public WorldLocation(Vector3 location)
    {
        location.z = location.y;
        this.vector = location;
    }

    public WorldLocation(Vector2 location)
    {
        this.vector = new Vector3(location.x, location.y, location.y);
    }

    public WorldLocation Randomize(float half)
    {
        if(half == 0)
        {
            return this;
        }
        return new WorldLocation(vector + new Vector3(Random.instance.NextFloat() * half * 2 - half,
           Random.instance.NextFloat() * half * 2 - half, 0));
    }

    public static WorldLocation operator +(WorldLocation o1, WorldLocation o2)
    {
        return new WorldLocation(o1.vector + o2.vector);
    }

    public static WorldLocation operator -(WorldLocation o1, WorldLocation o2)
    {
        return new WorldLocation(o1.vector - o2.vector);
    }

    public static WorldLocation operator *(WorldLocation o1, float o2)
    {
        return new WorldLocation(o1.vector * o2);
    }

    public static WorldLocation operator /(WorldLocation o1, float o2)
    {
        return new WorldLocation(o1.vector / o2);
    }

    public override bool Equals(object obj)
    {
        if ((obj == null) || !this.GetType().Equals(obj.GetType()))
        {
            return false;
        }
        else
        {
            WorldLocation p = (WorldLocation)obj;
            return p.vector.Equals(vector);
        }
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(vector);
    }
}
