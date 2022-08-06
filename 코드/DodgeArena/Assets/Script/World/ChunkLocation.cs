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
    /// �Էµ� ��ġ�� �������� �� ûũ�� �����Ǿ�� �ϴ��� ���θ� ��ȯ�մϴ�
    /// </summary>
    /// <param name="pos">�÷��̾� ��ġ</param>
    /// <returns>���� ����</returns>
    public bool CheckKeep(Vector2 pos)
    {
        return Mathf.Abs(pos.x - center.vector.x) <= GameManager.instance.half_ChunkSaveRange
           && Mathf.Abs(pos.y - center.vector.y) <= GameManager.instance.half_ChunkSaveRange;
    }

    /// <summary>
    /// �Էµ� ��ġ�� �������� �� ûũ�� �ε�Ǿ�� �Ǵ��� ���θ� ��ȯ�մϴ�
    /// </summary>
    /// <param name="pos">�÷��̾� ��ġ</param>
    /// <returns>�ε� ����</returns>
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
