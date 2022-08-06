using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WorldLocation
{
    public readonly World world;
    public readonly Vector3 vector;
    public Vector2 vector2 {
        get => Util.FlattenLocation(vector);
    }
    public ChunkLocation chunkLocation
    {
        get => new ChunkLocation(world, new Vector2(Mathf.Floor((vector.x + GameManager.instance.half_ChunkWeidth) / (GameManager.instance.half_ChunkWeidth * 2)),
            Mathf.Floor((vector.y + GameManager.instance.half_ChunkWeidth) / (GameManager.instance.half_ChunkWeidth * 2))));
    }
    public Chunk chunk
    {
        get => chunkLocation.chunk;
    }
    public Vector2 flattenLocation
    {
        get => new Vector2(vector.x, vector.y);
    }

    public WorldLocation(World world, Vector2 location) : this(world, new Vector3(location.x, location.y, location.y)) {

    }

    public WorldLocation(World world, Vector3 location)
    {
        this.world = world;
        location.z = location.y;
        this.vector = location;
    }

    public WorldLocation Randomize(float half)
    {
        if(half == 0)
        {
            return this;
        }
        return new WorldLocation(world, Util.Randomize(vector, half));
    }

    public static WorldLocation operator +(WorldLocation o1, WorldLocation o2)
    {
        return new WorldLocation(o1.world, o1.vector + o2.vector);
    }

    public static WorldLocation operator -(WorldLocation o1, WorldLocation o2)
    {
        return new WorldLocation(o1.world, o1.vector - o2.vector);
    }

    public static WorldLocation operator *(WorldLocation o1, float o2)
    {
        return new WorldLocation(o1.world, o1.vector * o2);
    }

    public static WorldLocation operator /(WorldLocation o1, float o2)
    {
        return new WorldLocation(o1.world, o1.vector / o2);
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
            return p.vector.Equals(vector) && p.world.Equals(world);
        }
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(vector);
    }

    public override string ToString() {
        return world.name + vector.ToString();
    }
}
