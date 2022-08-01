using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldLocation
{
    public readonly Vector3 location;

    public WorldLocation(Vector3 location)
    {
        this.location = location;
    }

    public ChunkLocation ToChunkLocation()
    {
        return new ChunkLocation(new Vector2(Mathf.Floor((location.x + GameManager.instance.chunkWeidth / 2) / GameManager.instance.chunkWeidth),
            Mathf.Floor((location.y + GameManager.instance.chunkWeidth / 2) / GameManager.instance.chunkWeidth)));
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
}
