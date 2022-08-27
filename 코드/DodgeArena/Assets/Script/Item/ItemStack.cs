using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// 아이템 객체
/// </summary>
[CreateAssetMenu(fileName = "ItemType", menuName = "Item/ItemStack")]
public class ItemStack : ScriptableObject
{
    [SerializeField]
    private ItemTypeEnum _type;
    public ItemType type { 
        get {
            return _type;
        } 
    }
    [SerializeField]
    private int _amount;
    public int amount {
        get {
            return _amount;
        }
        set {
            _amount = value;
        }
    }
    public static ItemStack Empty { get {
            return of(ItemTypeEnum.Empty, 0);
        } 
    }

    public static ItemStack of(ItemType type) {
        return of(type, 1);
    }

    public static ItemStack of(ItemType type, int amount) {
        ItemStack instance = CreateInstance<ItemStack>();
        instance._type = type;
        instance.amount = amount;
        return instance;
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

    /// <summary>
    /// 입력된 아이템을 추가합니다.
    /// 파라미터를 수정합니다. (추가하고 남은 아이템임)
    /// </summary>
    /// <param name="item">추가할 아이템</param>
    public ItemStack AddItem(ItemStack item) {
        if(!Stackable(item)) {
            return this;
        }
        int value = Math.Min(type.maxStackSize - this.amount, item.amount);
        OperateAmount(value);
        item.OperateAmount(-value);
        return this;
    }

    /// <summary>
    /// 입력된 아이템을 제거합니다.
    /// 파라미터를 수정합니다. (제거하고 남은 아이템임)
    /// </summary>
    /// <param name="item">제거할 아이템</param>
    public ItemStack RemoveItem(ItemStack item) {
        if(!StackableRestrict(item)) {
            return this;
        }
        int value = Math.Min(this.amount, item.amount);
        OperateAmount(-value);
        item.OperateAmount(-value);
        return this;
    }

    public ItemStack OperateAmount(int value) {
        return OperateAmount(value, false);
    }

    public ItemStack OperateAmount(int value, bool force) {
        return SetAmount(this.amount + value, force); ;
    }

    public ItemStack SetType(ItemType type) {
        this._type = type;
        return this;
    }

    public bool IsEmpty() {
        return type.enumType == ItemTypeEnum.Empty || amount == 0;
    }

    public ItemStack Clone() {
        return of(type, amount);
    }

    public bool Stackable(ItemStack item) {
        return IsEmpty() || StackableRestrict(item);
    }

    public bool StackableRestrict(ItemStack item) {
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
