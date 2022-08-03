using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System;

/// <summary>
/// 씬에 스폰되는 모든 객체의 부모
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
            //위치 재정렬
            transform.position = value.vector - new Vector3(0, 0, spriteRenderer.sprite.pivot.y / spriteRenderer.sprite.pixelsPerUnit);
            _location = value;

            //청크 업데이트
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

                //청크 상태 반영
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
    /// 플레이어와의 거리를 비교합니다
    /// </summary>
    /// <param name="distance">임계 거리</param>
    /// <returns></returns>
    protected bool CheckPlayerDistance(float distance)
    {
        return CheckDistance(GameManager.instance.player.gameObject.transform.position, distance);
    }

    /// <summary>
    /// 대상과의 거리를 비교합니다
    /// </summary>
    /// <param name="position"></param>
    /// <param name="distance"></param>
    /// <returns></returns>
    protected bool CheckDistance(Vector3 position, float distance)
    {
        return Vector3.Distance(position, transform.position) <= distance;
    }

    /// <summary>
    /// 2D기준으로 대상을 바라봅니다. <br/>
    /// Z회전이 0일때 오른쪽을 바라본다고 가정합니다.
    /// </summary>
    /// <param name="targetPos">바라볼 대상의 위치</param>
    protected void LookAt(Vector3 targetPos)
    {
        LookAt(targetPos, Vector2.right);
    }

    /// <summary>
    /// 2D기준으로 대상을 바라봅니다.
    /// </summary>
    /// <param name="targetPos">바라볼 대상의 위치</param>
    /// <param name="zeroRotation">Z회전이 0일때 바라보는 방향</param>
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
    /// 현재 위치를 보정합니다.
    /// </summary>
    protected void FixPosition()
    {
        WorldLocation loc = new WorldLocation(this.transform.position);
        location = loc;
    }

    /// <summary>
    /// 2D 회전시 위아래 반전을 보정합니다
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
    /// 이 개체가 처음 생성되었을때 호출됩니다 <br/>
    /// <see cref="OnLoad()"/>보다 먼저 호출됩니다
    /// </summary>
    public virtual void OnSpawn()
    {
        
    }

    /// <summary>
    /// 이 개체가 포함된 청크가 로드되었을때 호출됩니다
    /// </summary>
    /// <returns>성공적으로 실행되었는지 여부</returns>
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
    /// 이 개체가 포함된 청크가 언로드되었을때 호출됩니다
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
    /// 이 개체가 완전히 파괴될 때 호출됩니다
    /// 항상 <see cref="OnUnload()"/> 이후에 호출됩니다
    /// </summary>
    public virtual void OnDespawn()
    {
        if (loaded)
        {
            OnUnload();
        }
    }
}
