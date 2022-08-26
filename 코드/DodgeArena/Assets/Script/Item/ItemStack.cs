using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 아이템 객체
/// </summary>
public class ItemStack
{
    public ItemType type { private set; get; }
    public int amount { private set; get; }

    public ItemStack(ItemType type) : this(type, 1) {

    }

    public ItemStack(ItemType type, int amount) {
        this.type = type;
        this.amount = amount;
    }

    public ItemStack SetAmount(int amount) {
        return SetAmount(amount, false);
    }

    public ItemStack SetAmount(int amount, bool force) {
        if(amount < 0) {
            amount = 0;
        }
        if(!force) {
            if(amount > type.maxStackSize) {
                amount = type.maxStackSize;
            }
        }
        this.amount = amount;
        return this;
    }

    public ItemStack AddAmount(int amount) {
        return AddAmount(amount, false);
    }

    public ItemStack AddAmount(int amount, bool force) {
        return SetAmount(this.amount + amount, force); ;
    }

    public ItemStack SetType(ItemType type) {
        this.type = type;
        return this;
    }

    public bool IsEmpty() {
        return type.enumType == ItemTypeEnum.Empty || amount == 0;
    }

    public ItemStack Clone() {
        return new ItemStack(type, amount);
    }

    public bool Stackable(ItemStack item) {
        return item.type == type;
    }

    public override bool Equals(object obj) {
        if(( obj == null ) || !this.GetType().Equals(obj.GetType())) {
            return false;
        } else {
            ItemStack t = (ItemStack) obj;
            return t.type == type && t.amount == amount;
        }
    }

    public override int GetHashCode() {
        return HashCode.Combine(type, amount);
    }
}
