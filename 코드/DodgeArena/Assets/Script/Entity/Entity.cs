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
    /// �� ��ü�� ó�� �����Ǿ����� ȣ��˴ϴ� <br/>
    /// <see cref="OnLoad()"/>���� ���� ȣ��˴ϴ�
    /// </summary>
    public virtual void OnSpawn()
    {
        _location = new WorldLocation(transform.position);
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
