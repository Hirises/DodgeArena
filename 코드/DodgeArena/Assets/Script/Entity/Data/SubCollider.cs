using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ݶ��̴��� ���̾ �и��Ҷ� �̺�Ʈ�� ���� ���� �� �ֵ��� ���ִ� ������Ʈ
/// </summary>
public class SubCollider : MonoBehaviour
{
    public GameObject root;
    public delegate void EventHandler(Collider2D collision);
    /// <summary>
    /// �ش� �ݶ��̴��� ���� OnTriggerEnter2D �̺�Ʈ
    /// </summary>
    public event EventHandler onTriggerEnter;
    /// <summary>
    /// �ش� �ݶ��̴��� ���� OnTriggerStay2D �̺�Ʈ
    /// </summary>
    public event EventHandler onTriggerStay;
    /// <summary>
    /// �ش� �ݶ��̴��� ���� OnTriggerExit2D �̺�Ʈ
    /// </summary>
    public event EventHandler onTriggerExit;
        
    private void OnTriggerEnter2D(Collider2D collision) {
        if(onTriggerEnter != null) {
            onTriggerEnter(collision);
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
        }
    }
}
