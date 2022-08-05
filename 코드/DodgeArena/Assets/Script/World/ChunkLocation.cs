using UnityEngine;
using System;

public class ChunkLocation
{
    public readonly Vector2 vector;
    public readonly World world;
    public Chunk chunk
    {
        get => world.GetChunk(this);
    }
    public WorldLocation center
    {
        get => new WorldLocation(world, vector * GameManager.instance.chunkWeidth);
    }

    public ChunkLocation(World world, Vector2 location) {
        this.world = world;
        location.x = Mathf.Floor(location.x);
        location.y = Mathf.Floor(location.y);
        this.vector = location;
    }

    public bool CheckKeep()
    {
        Vector2 pos = GameManager.instance.player.transform.position;
        return Mathf.Abs(pos.x - center.vector.x) <= GameManager.instance.chunkSaveRange
           && Mathf.Abs(pos.y - center.vector.y) <= GameManager.instance.chunkSaveRange;
    }

    public bool CheckLoad()
    {
        Vector2 pos = GameManager.instance.player.transform.position;
        return Mathf.Abs(pos.x - center.vector.x) <= GameManager.instance.chunkLoadRange
           && Mathf.Abs(pos.y - center.vector.y) <= GameManager.instance.chunkLoadRange;
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
            return p.vector.Equals(vector) && p.world.Equals(world);
        }
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(vector, world);
    }

    public override string ToString()
    {
        return world.name + vector.ToString();
    }
}
