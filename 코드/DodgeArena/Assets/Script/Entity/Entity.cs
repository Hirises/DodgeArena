using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� �����Ǵ� ��� ��ü�� �θ�
/// </summary>
public abstract class Entity : MonoBehaviour
{
    [SerializeField]
    protected SpriteRenderer spriteRenderer;
    private WorldLocation _location;
    public WorldLocation location
    {
        get => _location;
        set
        {
            transform.position = value.vector;
            _location = value;

            //ûũ ������Ʈ
            Chunk newChunk = value.chunk;
            if (!newChunk.Equals(chunk))    
            {
                newChunk.entities.Add(this);
                if(chunk != null)
                {
                    chunk.entities.Remove(this);
                }
                transform.parent = newChunk.rootObject.transform;
                _chunk = newChunk;
            }
        }
    }
    private Chunk _chunk;
    public Chunk chunk
    {
        get => _chunk;
    }

    /// <summary>
    /// �÷��̾���� �Ÿ��� ���մϴ�
    /// </summary>
    /// <param name="distance">�Ӱ� �Ÿ�</param>
    /// <returns></returns>
    protected bool CheckPlayerDistance(float distance)
    {
        return CheckDistance(GameManager.instance.player.gameObject.transform.position, distance);
    }

    /// <summary>
    /// ������ �Ÿ��� ���մϴ�
    /// </summary>
    /// <param name="position"></param>
    /// <param name="distance"></param>
    /// <returns></returns>
    protected bool CheckDistance(Vector3 position, float distance)
    {
        return Vector3.Distance(position, transform.position) <= distance;
    }

    /// <summary>
    /// 2D�������� ����� �ٶ󺾴ϴ�. <br/>
    /// Zȸ���� 0�϶� �������� �ٶ󺻴ٰ� �����մϴ�.
    /// </summary>
    /// <param name="targetPos">�ٶ� ����� ��ġ</param>
    protected void LookAt(Vector3 targetPos)
    {
        LookAt(targetPos, Vector2.right);
    }

    /// <summary>
    /// 2D�������� ����� �ٶ󺾴ϴ�.
    /// </summary>
    /// <param name="targetPos">�ٶ� ����� ��ġ</param>
    /// <param name="zeroRotation">Zȸ���� 0�϶� �ٶ󺸴� ����</param>
    protected void LookAt(Vector3 targetPos, Vector2 zeroRotation)
    {
        Vector3 angle = Util.LootAtRotation(transform.position, targetPos, zeroRotation);
        transform.rotation = Quaternion.Euler(angle);
        spriteRenderer.flipY = Mathf.Abs(angle.z) > 90;
    }

    private void LateUpdate()
    {
        FixPosition();
    }

    /// <summary>
    /// ���� ��ġ�� �����մϴ�.
    /// </summary>
    protected void FixPosition()
    {
        WorldLocation loc = new WorldLocation(this.transform.position);
        location = loc;
    }

    /// <summary>
    /// �� ��ü�� ó�� �����Ǿ����� ȣ��˴ϴ� <br/>
    /// <see cref="OnLoad()"/>���� ���� ȣ��˴ϴ�
    /// </summary>
    public virtual void OnSpawn()
    {
        
    }

    /// <summary>
    /// �� ��ü�� �ε�Ǿ����� ȣ��˴ϴ�
    /// </summary>
    public virtual void OnLoad()
    {

    }

    /// <summary>
    /// �� ��ü�� ��ε�Ǿ����� ȣ��˴ϴ�
    /// </summary>
    public virtual void OnUnload()
    {

    }

    /// <summary>
    /// �� ��ü�� ������ �ı��� �� ȣ��˴ϴ�
    /// </summary>
    public virtual void OnDespawn()
    {

    }
}
