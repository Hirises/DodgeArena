using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��𼭳� �� �� �ִ� ���� Ǯ�̴�
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
