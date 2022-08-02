using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : LivingEntity
{
    [SerializeField]
    private PlayerController controller;

    [SerializeField]
    private Sprite normal;
    [SerializeField]
    private Sprite hit;
}
