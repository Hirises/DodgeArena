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
            transform.position = value.location;
            _location = location;
        }
    }

    protected bool CheckPlayerDistance(float distance)
    {
        return Vector3.Distance(GameManager.instance.player.gameObject.transform.position, transform.position) < distance;
    }

    private void LateUpdate()
    {
        FixPosition();
    }

    protected void FixPosition()
    {
        Vector3 pos = this.transform.position;
        this.transform.position = new Vector3(pos.x, pos.y, pos.y);
    }

    /// <summary>
    /// 이 개체가 처음 생성되었을때 호출됩니다 <br/>
    /// <see cref="OnLoad()"/>보다 먼저 호출됩니다
    /// </summary>
    public virtual void OnSpawn()
    {
        _location = new WorldLocation(transform.position);
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
