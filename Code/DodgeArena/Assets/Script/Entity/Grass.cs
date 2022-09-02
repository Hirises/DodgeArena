using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;

/// <summary>
/// 어디서나 볼 수 있는 흔한 풀이다
/// </summary>
public class Grass : LandScape
{
    [SerializeField]
    protected Sprite[] variants;

    public override void OnSpawn()
    {
        spriteRenderer.sprite = variants[Random.instance.RandInt(0, variants.Length)];
    }
}
