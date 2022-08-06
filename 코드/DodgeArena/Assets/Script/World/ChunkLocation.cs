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
        get => new WorldLocation(world, vector * GameManager.instance.half_ChunkWeidth * 2);
    }

    public ChunkLocation(World world, Vector2 location) {
        this.world = world;
        location.x = Mathf.Floor(location.x);
        location.y = Mathf.Floor(location.y);
        this.vector = location;
    }

    /// <summary>
    /// 입력된 위치를 기준으로 이 청크가 보관되어야 하는지 여부를 반환합니다
    /// </summary>
    /// <param name="pos">플레이어 위치</param>
    /// <returns>보관 여부</returns>
    public bool CheckKeep(Vector2 pos)
    {
        return Mathf.Abs(pos.x - center.vector.x) <= GameManager.instance.half_ChunkSaveRange
           && Mathf.Abs(pos.y - center.vector.y) <= GameManager.instance.half_ChunkSaveRange;
    }

    /// <summary>
    /// 입력된 위치를 기준으로 이 청크가 로드되어야 되는지 여부를 반환합니다
    /// </summary>
    /// <param name="pos">플레이어 위치</param>
    /// <returns>로드 여부</returns>
    public bool CheckLoad(Vector2 pos)
    {
        return Mathf.Abs(pos.x - center.vector.x) <= GameManager.instance.half_ChunkLoadRange
           && Mathf.Abs(pos.y - center.vector.y) <= GameManager.instance.half_ChunkLoadRange;
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
