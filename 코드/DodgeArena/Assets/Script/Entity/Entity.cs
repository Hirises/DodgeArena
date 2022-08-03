using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System;

/// <summary>
/// ���� �����Ǵ� ��� ��ü�� �θ�
/// </summary>
public abstract class Entity : MonoBehaviour
{
    [SerializeField]
    protected EntityType.Type entityType;
    [SerializeField]
    protected SpriteRenderer spriteRenderer;
    [SerializeField]
    protected Collider2D[] innerColliders;
    private WorldLocation _location;
    public WorldLocation location
    {
        get => _location;
        set
        {
            //��ġ ������
            transform.position = value.vector - new Vector3(0, 0, spriteRenderer.sprite.pivot.y / spriteRenderer.sprite.pixelsPerUnit);
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
                transform.parent = newChunk.gameObject.transform;
                _chunk = newChunk;

                //ûũ ���� �ݿ�
                if (newChunk.loaded)
                {
                    OnLoad();
                }
                else
                {
                    OnUnload();
                }
            }
        }
    }
    private Chunk _chunk;
    public Chunk chunk
    {
        get => _chunk;
    }
    private bool _initiated = false;
    public bool initiated { get => _initiated; }

    private bool _loaded = false;
    public bool loaded { get => _loaded; }

    public void Initiated(WorldLocation location, Chunk chunk)
    {
        if (initiated)
        {
            return;
        }

        this._location = location;
        this._chunk = chunk;
        this._loaded = false;
        chunk.entities.Add(this);
        transform.parent = chunk.gameObject.transform;
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

    public void Remove()
    {
        OnDespawn();
        chunk.entities.Remove(this);
        Destroy(gameObject);
    }

    /// <summary>
    /// �� ��ü�� ó�� �����Ǿ����� ȣ��˴ϴ� <br/>
    /// <see cref="OnLoad()"/>���� ���� ȣ��˴ϴ�
    /// </summary>
    public virtual void OnSpawn()
    {
        
    }

    /// <summary>
    /// �� ��ü�� ���Ե� ûũ�� �ε�Ǿ����� ȣ��˴ϴ�
    /// </summary>
    /// <returns>���������� ����Ǿ����� ����</returns>
    public virtual bool OnLoad()
    {
        if (loaded)
        {
            return false;
        }
        _loaded = true;

        return true;
    }

    /// <summary>
    /// �� ��ü�� ���Ե� ûũ�� ��ε�Ǿ����� ȣ��˴ϴ�
    /// </summary>
    public virtual bool OnUnload()
    {
        if (!loaded)
        {
            return false;
        }
        _loaded = false;

        return true;
    }

    /// <summary>
    /// �� ��ü�� ������ �ı��� �� ȣ��˴ϴ�
    /// �׻� <see cref="OnUnload()"/> ���Ŀ� ȣ��˴ϴ�
    /// </summary>
    public virtual void OnDespawn()
    {
        if (loaded)
        {
            OnUnload();
        }
    }
}
