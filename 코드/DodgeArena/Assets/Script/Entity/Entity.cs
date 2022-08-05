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
    [BoxGroup("Entity")]
    public EntityType.Type entityType;
    [SerializeField]
    [BoxGroup("Entity")]
    protected SpriteRenderer spriteRenderer;
    [SerializeField]
    [BoxGroup("Entity")]
    protected new Rigidbody2D rigidbody;
    [SerializeField]
    [BoxGroup("Entity")]
    protected SubCollider triggerCollider;
    private WorldLocation _location;
    public WorldLocation location
    {
        get => _location;
        set
        {
            //위치 재정렬
            int order = Mathf.FloorToInt((value.vector.y - spriteRenderer.sprite.pivot.y / spriteRenderer.sprite.pixelsPerUnit - GameManager.instance.player.transform.position.y)
                * spriteRenderer.sprite.pixelsPerUnit);
            spriteRenderer.sortingOrder = order;
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
        this.triggerCollider.onTriggerEnter += OnColliderEnter;
        this.triggerCollider.onTriggerExit += OnColliderExit;
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
        FixFlip();
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
    protected void FixFlip() {
        if(transform.right.x < 0 != filped) {
            filped = !filped;
            transform.localScale = new Vector3(transform.localScale.x, -1 * transform.localScale.y, transform.localScale.z);
        }
    }

    /// <summary>
    /// 다른 물체와 충돌했을 때 (이벤트에 등록)
    /// </summary>
    private void OnColliderEnter(Collider2D collision) { 
        if(collision.gameObject.TryGetComponent(out Entity other)) {
            OnStartCollide(other);
        } else if(collision.gameObject.TryGetComponent(out SubCollider sub)) {
            if(sub.root.TryGetComponent(out Entity parent)) {
                OnStartCollide(parent);
            }
        }
    }

    /// <summary>
    /// 다른 물체와 충돌이 끝났을 때 (이벤트에 등록)
    /// </summary>
    private void OnColliderExit(Collider2D collision) {
        if(collision.gameObject.TryGetComponent(out Entity other)) {
            OnEndCollide(other);
        } else if(collision.gameObject.TryGetComponent(out SubCollider sub)) {
            if(sub.root.TryGetComponent(out Entity parent)) {
                OnEndCollide(parent);
            }
        }
    }

    /// <summary>
    /// 해당 엔티티를 월드에서 제거합니다
    /// </summary>
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
    /// 이 개체가 포함된 청크가 로드되었을때 호출됩니다 <br/>
    /// 항상 base.OnLoad()를 호출한 후 성공여부를 확인해야합니다
    /// </summary>
    /// <returns>성공적으로 실행되었는지 여부<br/>false 반환시 더이상 실행하지 않는다</returns>
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
    /// 이 개체가 포함된 청크가 언로드되었을때 호출됩니다 <br/>
    /// 항상 base.OnUnload()를 호출한 후 성공여부를 확인해야합니다
    /// </summary>
    /// <returns>성공적으로 실행되었는지 여부<br/>false 반환시 더이상 실행하지 않는다</returns>
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

    /// <summary>
    /// 이 개체가 다른 개체와 충돌했을 때 호출됩니다
    /// </summary>
    /// <param name="other">충돌한 개체</param>
    public virtual void OnStartCollide(Entity other) {

    }

    /// <summary>
    /// 이 개체와 다른 개체와의 충돌이 끝났을 때 호출됩니다
    /// </summary>
    /// <param name="other">충돌한 개체</param>
    public virtual void OnEndCollide(Entity other) {

    }
}
