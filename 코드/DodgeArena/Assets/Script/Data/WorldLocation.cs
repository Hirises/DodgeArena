using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WorldLocation
{
    public readonly Vector3 location;
    public Vector2 flattenLocation
    {
        get => new Vector2(location.x, location.y);
    }

    public WorldLocation(Vector3 location)
    {
        location.z = location.y;
        this.location = location;
    }

    public WorldLocation(Vector2 location)
    {
        this.location = new Vector3(location.x, location.y, location.y);
    }

    public ChunkLocation ToChunkLocation()
    {
        return new ChunkLocation(new Vector2(Mathf.Floor((location.x + GameManager.instance.chunkWeidth / 2) / GameManager.instance.chunkWeidth),
            Mathf.Floor((location.y + GameManager.instance.chunkWeidth / 2) / GameManager.instance.chunkWeidth)));
    }

    public WorldLocation Randomize(float half)
    {
        System.Random random = new System.Random();
        return new WorldLocation(location + new Vector3(Convert.ToSingle(random.NextDouble()) * half * 2 - half,
           Convert.ToSingle(random.NextDouble()) * half * 2 - half, 0));
    }

    public static WorldLocation operator +(WorldLocation o1, WorldLocation o2)
    {
        return new WorldLocation(o1.location + o2.location);
    }

    public static WorldLocation operator -(WorldLocation o1, WorldLocation o2)
    {
        return new WorldLocation(o1.location - o2.location);
    }

    public static WorldLocation operator *(WorldLocation o1, float o2)
    {
        return new WorldLocation(o1.location * o2);
    }

    public static WorldLocation operator /(WorldLocation o1, float o2)
    {
        return new WorldLocation(o1.location / o2);
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
            return p.location.Equals(location);
        }
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(location);
    }
}
