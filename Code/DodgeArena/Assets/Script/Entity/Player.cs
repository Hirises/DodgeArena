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
    [SerializeField]
    [BoxGroup("Player")]
    private int backpackSize;
    public int hp { get; private set; }
    [HideInInspector]
    public Container backpack;
    [HideInInspector]
    public Equipments equipments;
    [HideInInspector]
    public bool isHarvesting;

    public override void OnSpawn() {
        this.hp = initialHp;
        this.backpack = new Container(backpackSize);
        this.equipments = new Equipments();
        this.backpack.changeEvent += HUDManager.instance.UpdateQuickBar;
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

    public void OperateHP(int value) {
        if(value == 0) {
            return;
        }else if(value < 0) {
            Damage(-value);
        } else {
            Heal(value);
        }
    }

    public void Damage(int damage) {
        hp -= damage;

        if(hp <= 0) {
            GameManager.instance.EndGame();
        }
    }

    public void Heal(int heal) {
        hp += heal;
    }

    public void DropItem(ItemStack targetItem) {
        if(equipments.IsEquiped(targetItem, out Equipments.Slot slot)) {
            equipments.Unequip(slot, targetItem);
        }
        location.world.Spawn(( (EntityType) EntityTypeEnum.Item ).prefab, location.Randomize(1.5f), item => ( (Item) item ).itemstack = targetItem);
    }
}
