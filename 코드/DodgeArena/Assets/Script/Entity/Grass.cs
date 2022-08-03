using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 어디서나 볼 수 있는 흔한 풀이다
/// </summary>
public class Grass : LandScape
{
    [SerializeField]
    public Sprite[] variants;

    public override void OnSpawn()
    {
        base.OnSpawn();
        spriteRenderer.sprite = variants[Random.instance.RandInt(0, variants.Length)];
    }
}
