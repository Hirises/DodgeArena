using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 콜라이더의 레이어를 분리할때 이벤트를 따로 받을 수 있도록 해주는 컴포넌트
/// </summary>
public class SubCollider : MonoBehaviour
{
    /// <summary>
    /// 이 SubCollider가 적용된 루트 엔티티
    /// </summary>
    public Entity root;
    /// <summary>
    /// 충돌된 개체의 정보를 보관할지 여부
    /// </summary>
    public bool isTrackCollide = false;

    #region Events
    public delegate void TriggerEvent(Entity entity, Collider2D collision);
    public delegate void CollideEvent(Entity entity, Collision2D collision);
    /// <summary>
    /// 해당 콜라이더에 대한 OnTriggerEnter2D 이벤트
    /// </summary>
    public event TriggerEvent onTriggerEnter;
    /// <summary>
    /// 해당 콜라이더에 대한 OnTriggerStay2D 이벤트
    /// </summary>
    public event TriggerEvent onTriggerStay;
    /// <summary>
    /// 해당 콜라이더에 대한 OnTriggerExit2D 이벤트
    /// </summary>
    public event TriggerEvent onTriggerExit;
    /// <summary>
    /// 해당 콜라이더에 대한 OnTriggerEnter2D 이벤트
    /// </summary>
    public event CollideEvent onColliderEnter;
    /// <summary>
    /// 해당 콜라이더에 대한 OnTriggerStay2D 이벤트
    /// </summary>
    public event CollideEvent onColliderStay;
    /// <summary>
    /// 해당 콜라이더에 대한 OnTriggerExit2D 이벤트
    /// </summary>
    public event CollideEvent onColliderExit; 
    #endregion

    /// <summary>
    /// 현재 충돌중인 오브젝트들
    /// </summary>
    public readonly List<Entity> trigged = new List<Entity>();
    /// <summary>
    /// 현재 충돌중인 오브젝트들
    /// </summary>
    public readonly List<Entity> collided = new List<Entity>();

    #region Trigger
    private void OnTriggerEnter2D(Collider2D collision) {
        bool callEvent = onTriggerEnter != null;
        if(callEvent || isTrackCollide) {
            Entity entity;
            if(!collision.gameObject.TryGetComponent(out entity)) {
                if(collision.gameObject.TryGetComponent(out SubCollider sub)) {
                    entity = sub.root;
                } else {
                    return;
                }
            }

            if(callEvent) {
                onTriggerEnter(entity, collision);
            }
            if(isTrackCollide) {
                trigged.Add(entity);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision) {
        bool callEvent = onTriggerStay != null;
        if(callEvent) {
            Entity entity;
            if(!collision.gameObject.TryGetComponent(out entity)) {
                if(collision.gameObject.TryGetComponent(out SubCollider sub)) {
                    entity = sub.root;
                } else {
                    return;
                }
            }

            onTriggerStay(entity, collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        bool callEvent = onTriggerExit != null;
        if(callEvent || isTrackCollide) {
            Entity entity;
            if(!collision.gameObject.TryGetComponent(out entity)) {
                if(collision.gameObject.TryGetComponent(out SubCollider sub)) {
                    entity = sub.root;
                } else {
                    return;
                }
            }

            if(callEvent) {
                onTriggerExit(entity, collision);
            }
            if(isTrackCollide) {
                trigged.Remove(entity);
            }
        }
    }
    #endregion

    #region Collision
    private void OnCollisionEnter2D(Collision2D collision) {
        bool callEvent = onColliderEnter != null;
        if(callEvent || isTrackCollide) {
            Entity entity;
            if(!collision.gameObject.TryGetComponent(out entity)) {
                if(collision.gameObject.TryGetComponent(out SubCollider sub)) {
                    entity = sub.root;
                } else {
                    return;
                }
            }

            if(callEvent) {
                onColliderEnter(entity, collision);
            }
            if(isTrackCollide) {
                collided.Add(entity);
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision) {
        bool callEvent = onColliderStay != null;
        if(callEvent) {
            Entity entity;
            if(!collision.gameObject.TryGetComponent(out entity)) {
                if(collision.gameObject.TryGetComponent(out SubCollider sub)) {
                    entity = sub.root;
                } else {
                    return;
                }
            }

            onColliderStay(entity, collision);
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        bool callEvent = onColliderExit != null;
        if(callEvent || isTrackCollide) {
            Entity entity;
            if(!collision.gameObject.TryGetComponent(out entity)) {
                if(collision.gameObject.TryGetComponent(out SubCollider sub)) {
                    entity = sub.root;
                } else {
                    return;
                }
            }

            if(callEvent) {
                onColliderExit(entity, collision);
            }
            if(isTrackCollide) {
                collided.Remove(entity);
            }
        }
    } 
    #endregion
}
