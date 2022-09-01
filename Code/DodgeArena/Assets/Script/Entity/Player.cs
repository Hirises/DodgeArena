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
    public Container backpack {
        get;
        private set;
    }
    public Equipments equipments {
        get;
        private set;
    }
    [HideInInspector]
    public bool isHarvesting;

    public override void OnSpawn() {
        this.hp = initialHp;
        this.backpack = new Container(backpackSize);
        this.equipments = new Equipments();
        this.equipments.changeEvent += HUDManager.instance.UpdateQuickBar;
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

    /// <summary>
    /// 플레이어에게 아이템을 지급합니다
    /// 퀵슬롯을 먼저 검사합니다
    /// </summary>
    /// <param name="item">추가할 아이템</param>
    /// <returns>추가하고 남은 아이템 (복사본)</returns>
    public ItemStack AddItem(ItemStack item) {
        ItemStack copy = equipments.AddItem(item);
        if(copy.IsEmpty()) {
            return copy;
        }
        return backpack.AddItem(copy);
    }
}
