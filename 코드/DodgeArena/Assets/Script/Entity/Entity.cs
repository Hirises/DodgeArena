using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System;

/// <summary>
/// ���� �����Ǵ� ��� ��ü�� �θ�
/// </summary>
public abstract class Entity : MonoBehaviour {
    [SerializeField]
    protected Collider2D[] innerColliders;
    [SerializeField]
    [BoxGroup("Entity")]
    protected EntityType.Type entityType;
    [SerializeField]
    [BoxGroup("Entity")]
    protected SpriteRenderer spriteRenderer;
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
                chunk = newChunk;

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
    public Chunk chunk { get; private set; }
    public bool initiated { get; private set; }
    public bool loaded { get; private set; }
    public bool filped { get; private set; }

    public void Initiated(WorldLocation location, Chunk chunk)
    {
        if (initiated)
        {
            return;
        }

        this._location = location;
        this.chunk = chunk;
        this.loaded = false;
        FixFlip();
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
        FixFlip();
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
    /// 2D ȸ���� ���Ʒ� ������ �����մϴ�
    /// </summary>
    protected void FixFlip()
    {
        if (Mathf.Abs(transform.rotation.eulerAngles.z) > 90 != filped)
        {
            filped = !filped;
            transform.localScale = new Vector3(transform.localScale.x, -1 * transform.localScale.y, transform.localScale.z);
        }
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
        loaded = true;

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
        loaded = false;

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
