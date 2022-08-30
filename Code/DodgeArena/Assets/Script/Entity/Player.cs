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
    public bool isHarvesting;
    private ItemStack[] equipedItems = new ItemStack[4];

    public override void OnSpawn() {
        for(int i = 0; i < equipedItems.Length; i++) {
            equipedItems[i] = ItemStack.Empty;
        }
        this.hp = initialHp;
        this.backpack = new Container(backpackSize);
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

    public void Damage(int damage) {
        hp -= damage;

        if(hp <= 0) {
            GameManager.instance.EndGame();
        }
    }

    public bool HasEmptyEquipmentSlot() {
        for(int i = 0; i < equipedItems.Length; i++) {
            if(equipedItems[i].IsEmpty()) {
                return true;
            }
        }
        return false;
    }

    public void Equip(ItemStack item) {
        for(int i = 0; i < equipedItems.Length; i++) {
            if(equipedItems[i].IsEmpty()) {
                equipedItems[i] = item;
                HUDManager.instance.UpdateQuickBar();
                return;
            }
        }
    }

    public void Unequip(ItemStack item) {
        for(int i = 0; i < equipedItems.Length; i++) {
            if(equipedItems[i] == item) {
                equipedItems[i] = ItemStack.Empty;
                HUDManager.instance.UpdateQuickBar();
            }
        }
    }

    public int GetEquipedSlot(ItemStack item) {
        for(int i = 0; i < equipedItems.Length; i++) {
            if(equipedItems[i] == item) {
                return i;
            }
        }
        return -1;
    }

    public ItemStack GetEquipedItem(int slot) {
        return equipedItems[slot];
    }

    public void DropItem(ItemStack targetItem) {
        if(GetEquipedSlot(targetItem) >= 0) {
            Unequip(targetItem);
        }
        location.world.Spawn(( (EntityType) EntityTypeEnum.Item ).prefab, location.Randomize(1.5f), item => ( (Item) item ).itemstack = targetItem);
    }
}
