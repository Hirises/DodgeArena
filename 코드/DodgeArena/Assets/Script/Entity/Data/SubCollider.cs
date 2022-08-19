using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 콜라이더의 레이어를 분리할때 이벤트를 따로 받을 수 있도록 해주는 컴포넌트
/// </summary>
public class SubCollider : MonoBehaviour
{
    public GameObject root;
    public delegate void EventHandler(Collider2D collision);
    /// <summary>
    /// 해당 콜라이더에 대한 OnTriggerEnter2D 이벤트
    /// </summary>
    public event EventHandler onTriggerEnter;
    /// <summary>
    /// 해당 콜라이더에 대한 OnTriggerStay2D 이벤트
    /// </summary>
    public event EventHandler onTriggerStay;
    /// <summary>
    /// 해당 콜라이더에 대한 OnTriggerExit2D 이벤트
    /// </summary>
    public event EventHandler onTriggerExit;
    /// <summary>
    /// 현재 충돌중인 오브젝트들
    /// </summary>
    public List<GameObject> collides;
        
    private void OnTriggerEnter2D(Collider2D collision) {
        if(onTriggerEnter != null) {
            onTriggerEnter(collision);
            collides.Add(collision.gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if(onTriggerStay != null) {
            onTriggerStay(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if(onTriggerExit != null) {
            onTriggerExit(collision);
            collides.Remove(collision.gameObject);
        }
    }
}
