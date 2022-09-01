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
    [Label("originData")]
    private EntityType _type;
    public EntityType type {
        get => _type.enumType;
    }
    [SerializeField]
    [BoxGroup("Entity")]
    protected SpriteRenderer spriteRenderer;
    [SerializeField]
    [BoxGroup("Entity")]
    protected new Rigidbody2D rigidbody;
    /// <summary>
    /// 트리거 용도로 작동하는 콜라이더
    /// 원활한 감지를 위해 <see cref="collider"/>보다 더 큰 크기여야한다
    /// </summary>
    [SerializeField]
    [BoxGroup("Entity")]
    protected SubCollider trigger;
    /// <summary>
    /// 충돌 용도로 작동하는 콜라이더
    /// 원활한 감지를 위해 <see cref="trigger"/>보다 작은 크기여야한다
    /// </summary>
    [SerializeField]
    [BoxGroup("Entity")]
    protected new SubCollider collider;

    public WorldLocation location { get; protected set; }
    public Chunk chunk {
        get {
            return location.chunk;
        }
    }
    public bool initiated { get; private set; }
    public bool loaded { get; private set; }
    public bool filped { get; private set; }

    public void Initiated(WorldLocation location)
    {
        if (initiated)
        {
            return;
        }

        this.location = location;
        this.loaded = false;
        this.trigger.onTriggerEnter += OnStartTrigger;
        this.trigger.onTriggerStay += OnStayTrigger;
        this.trigger.onTriggerExit += OnEndTrigger;
        this.collider.onColliderEnter += OnStartCollide;
        this.collider.onColliderStay += OnStayCollide;
        this.collider.onColliderExit += OnEndCollide;

        FixFlip();
        chunk.entities.Add(this);
        transform.parent = chunk.gameObject.transform;
    }

    #region Controll
    /// <summary>
    /// 이 개체를 다른 위치로 순간이동시킵니다
    /// </summary>
    /// <param name="location">대상 위치</param>
    public virtual void Teleport(WorldLocation location) {
        this.transform.position = location.vector;
        FixPosition();
    }

    /// <summary>
    /// 해당 엔티티를 월드에서 제거합니다
    /// </summary>
    public void Remove() {
        OnDespawn();
        chunk.entities.Remove(this);
        Destroy(gameObject);
    } 
    #endregion

    #region Utils
    /// <summary>
    /// 플레이어와의 거리를 비교합니다
    /// </summary>
    /// <param name="distance">임계 거리</param>
    /// <returns></returns>
    public bool CheckPlayerDistance(float distance) {
        return CheckDistance(GameManager.instance.player.gameObject.transform.position, distance);
    }

    /// <summary>
    /// 대상과의 거리를 비교합니다
    /// </summary>
    /// <param name="position"></param>
    /// <param name="distance"></param>
    /// <returns></returns>
    public bool CheckDistance(Vector3 position, float distance) {
        return Vector3.Distance(position, transform.position) <= distance;
    }

    /// <summary>
    /// 2D기준으로 대상을 바라봅니다. <br/>
    /// Z회전이 0일때 오른쪽을 바라본다고 가정합니다.
    /// </summary>
    /// <param name="targetPos">바라볼 대상의 위치</param>
    public void LookAt(Vector3 targetPos) {
        LookAt(targetPos, Vector2.right);
    }

    /// <summary>
    /// 2D기준으로 대상을 바라봅니다.
    /// </summary>
    /// <param name="targetPos">바라볼 대상의 위치</param>
    /// <param name="zeroRotation">Z회전이 0일때 바라보는 방향</param>
    public void LookAt(Vector3 targetPos, Vector2 zeroRotation) {
        Vector3 angle = Util.LootAtRotation(transform.position, targetPos, zeroRotation);
        transform.rotation = Quaternion.Euler(angle);
        FixFlip();
    }
    #endregion

    #region Update

    /// <summary>
    /// 현재 위치를 보정합니다.
    /// </summary>
    protected void FixPosition() {
        FixPosition(new WorldLocation(location.world, this.transform.position));
    }

    /// <summary>
    /// 위치를 보정합니다.
    /// </summary>
    /// <param name="currentLocation">현재 위치</param>
    protected void FixPosition(WorldLocation currentLocation) {
        //위치 재정렬
        //int order = Mathf.FloorToInt(( currentLocation.vector.y - spriteRenderer.sprite.pivot.y / spriteRenderer.sprite.pixelsPerUnit - GameManager.instance.player.transform.position.y )
        //    * spriteRenderer.sprite.pixelsPerUnit);
        //spriteRenderer.sortingOrder = order;
        this.transform.position = currentLocation.vector - new Vector3(0, 0, spriteRenderer.sprite.pivot.y / spriteRenderer.sprite.pixelsPerUnit);

        //청크 업데이트
        Chunk newChunk = currentLocation.chunk;
        if(!newChunk.Equals(chunk)) {
            newChunk.entities.Add(this);
            if(chunk != null) {
                chunk.entities.Remove(this);
            }
            transform.parent = newChunk.gameObject.transform;

            //청크 상태 반영
            if(newChunk.loaded) {
                OnLoad();
            } else {
                OnUnload();
            }
        }
        location = currentLocation;
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
    #endregion

    #region Virtual
    /// <summary>
    /// 이 개체가 처음 생성되었을때 호출됩니다 <br/>
    /// <see cref="OnLoad()"/>보다 먼저 호출됩니다
    /// </summary>
    public virtual void OnSpawn() {

    }

    /// <summary>
    /// 이 개체가 포함된 청크가 로드되었을때 호출됩니다 <br/>
    /// 항상 base.OnLoad()를 호출한 후 성공여부를 확인해야합니다
    /// </summary>
    /// <returns>성공적으로 실행되었는지 여부<br/>false 반환시 더이상 실행하지 않는다</returns>
    public virtual bool OnLoad() {
        if(loaded) {
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
    public virtual bool OnUnload() {
        if(!loaded) {
            return false;
        }
        loaded = false;

        return true;
    }

    /// <summary>
    /// 이 개체가 완전히 파괴될 때 호출됩니다
    /// 항상 <see cref="OnUnload()"/> 이후에 호출됩니다
    /// </summary>
    public virtual void OnDespawn() {
        if(loaded) {
            OnUnload();
        }
    }

    /// <summary>
    /// 이 개체가 다른 개체와 충돌했을 때 호출됩니다
    /// </summary>
    /// <param name="other">충돌한 개체</param>
    /// <param name="collision">충돌 맥락</param>
    public virtual void OnStartCollide(Entity other, Collision2D collision) {

    }

    /// <summary>
    /// 이 개체가 다른 개체와 충돌중일 때 호출됩니다
    /// </summary>
    /// <param name="other">충돌중인 개체</param>
    /// <param name="collision">충돌 맥락</param>
    public virtual void OnStayCollide(Entity other, Collision2D collision) {

    }

    /// <summary>
    /// 이 개체와 다른 개체와의 충돌이 끝났을 때 호출됩니다
    /// </summary>
    /// <param name="other">충돌한 개체</param>
    /// <param name="collision">충돌 맥락</param>
    public virtual void OnEndCollide(Entity other, Collision2D collision) {

    }

    /// <summary>
    /// 이 개체가 다른 개체와 겹쳤을 때 호출됩니다
    /// </summary>
    /// <param name="other">겹친 개체</param>
    /// <param name="collider">대상 콜라이더</param>
    public virtual void OnStartTrigger(Entity other, Collider2D collider) {

    }

    /// <summary>
    /// 이 개체가 다른 개체와 겹쳐있는 중일 때 호출됩니다
    /// </summary>
    /// <param name="other">겹친 개체</param>
    /// <param name="collider">대상 콜라이더</param>
    public virtual void OnStayTrigger(Entity other, Collider2D collider) {

    }

    /// <summary>
    /// 이 개체와 다른 개체와의 겹침이 끝났을 때 호출됩니다
    /// </summary>
    /// <param name="other">겹친 개체</param>
    /// <param name="collider">대상 콜라이더</param>
    public virtual void OnEndTrigger(Entity other, Collider2D collider) {

    }
    #endregion
}
