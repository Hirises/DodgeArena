using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

/// <summary>
/// 어디서나 볼 수 있는 흔한 풀이다
/// </summary>
public class Grass : LandScape
{

    public override void OnSpawn()
    {
        base.OnSpawn();
        spriteRenderer.sprite = originData.GetSprite(Random.instance.RandRange(0, 1) == 0 ? "variant0" : "variant1");
    }
}
