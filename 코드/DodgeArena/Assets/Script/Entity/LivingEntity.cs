using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

/// <summary>
/// ���� �����Ǵ� ������ ��ü
/// </summary>
public class LivingEntity : Entity
{
    [SerializeField]
    [BoxGroup("LivingEntity")]
    protected new Rigidbody2D rigidbody;
}
