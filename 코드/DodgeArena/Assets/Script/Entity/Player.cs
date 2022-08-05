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

    public override void OnSpawn() {
        base.OnSpawn();
        this.hp = initialHp;
    }

    /// <summary>
    /// �� ��ü�� �ٸ� ��ġ�� �����̵���ŵ�ϴ�
    /// </summary>
    /// <param name="location">��� ��ġ</param>
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
