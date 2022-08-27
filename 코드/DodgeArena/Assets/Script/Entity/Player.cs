using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class Player : LivingEntity {

    [SerializeField]
    [BoxGroup("Player")]
    private PlayerController controller;

    [SerializeField]
    [BoxGroup("Player")]
    private Sprite normal;
    [SerializeField]
    [BoxGroup("Player")]
    private Sprite hit;
    [SerializeField]
    [BoxGroup("Player")]
    private int initialHp;
    public int hp { get; private set; }
    public Container backpack;
    public bool isHarvesting;

    public override void OnSpawn() {
        base.OnSpawn();
        this.hp = initialHp;
        this.backpack = new Container(9);
        this.isHarvesting = false;
    }

    /// <summary>
    /// 이 개체를 다른 위치로 순간이동시킵니다
    /// </summary>
    /// <param name="location">대상 위치</param>
    public override void Teleport(WorldLocation location) {
        if(location.world.Equals(this.location.world)) {
            this.transform.position = location.vector;
            this.location = location;
            FixPosition();
        } else {
            World world = this.location.world;
            this.transform.position = location.vector;
            this.location = location;
            FixPosition();
            world.Unload();
        }
    }

    public void Damage(int damage) {
        hp -= damage;

        if(hp <= 0) {
            GameManager.instance.GameEnd();
        }
    }
}
