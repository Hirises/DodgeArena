using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 씬에 스폰되는 모든 객체의 부모
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

            //청크 업데이트
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
        spriteRenderer.flipY = Mathf.Abs(angle.z) > 90;
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
    /// 이 개체가 처음 생성되었을때 호출됩니다 <br/>
    /// <see cref="OnLoad()"/>보다 먼저 호출됩니다
    /// </summary>
    public virtual void OnSpawn()
    {
        
    }

    /// <summary>
    /// 이 개체가 로드되었을때 호출됩니다
    /// </summary>
    public virtual void OnLoad()
    {

    }

    /// <summary>
    /// 이 개체가 언로드되었을때 호출됩니다
    /// </summary>
    public virtual void OnUnload()
    {

    }

    /// <summary>
    /// 이 개체가 완전히 파괴될 때 호출됩니다
    /// </summary>
    public virtual void OnDespawn()
    {

    }
}
