using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

/// <summary>
/// 씬에 생성되는 동적인 객체
/// </summary>
public class LivingEntity : Entity
{
    [SerializeField]
    [BoxGroup("LivingEntity")]
    protected new Rigidbody2D rigidbody;
}
