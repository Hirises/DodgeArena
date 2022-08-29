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
    [SerializeField]
    public List<ItemTag> tags;

    public static ItemStack Empty { get {
            return of(ItemTypeEnum.Empty, 0);
        } 
    }

    public static ItemStack of(ItemType type) {
        return of(type, 1);
    }

    public static ItemStack of(ItemType type, int amount) {
        return of(type, amount, new List<ItemTag>());
    }

    public static ItemStack of(ItemType type, int amount, List<ItemTag> tags) {
        ItemStack instance = CreateInstance<ItemStack>();
        instance._type = type;
        instance.amount = amount;
        instance.tags = tags;
        return instance;
    }

    public bool HasTag(ItemTag tag) {
        return tags.Contains(tag);
    }

    public void AddTag(ItemTag tag) {
        tags.Add(tag);
    }

    public void RemoveTag(ItemTag tag) {
        tags.Remove(tag);
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
        if(IsEmpty()) {
            CopyFrom(item);
            SetAmount(0);
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

    /// <summary>
    /// 이 아이템을 빈 상태로 초기화합니다
    /// </summary>
    public void Clear() {
        SetType(ItemTypeEnum.Empty);
        SetAmount(0);
        tags = new List<ItemTag>();
    }

    /// <summary>
    /// 해당 아이템의 정보를 복사해 이 아이템에 저장합니다
    /// </summary>
    /// <param name="other">대상 아이템</param>
    public void CopyFrom(ItemStack other) {
        SetType(other.type);
        SetAmount(other.amount);
        tags.Clear();
        tags.AddRange(other.tags);
    }

    public bool IsEmpty() {
        return type.enumType == ItemTypeEnum.Empty || amount == 0;
    }

    public ItemStack Clone() {
        return of(type, amount, tags);
    }

    /// <summary>
    /// 해당 아이템과 겹칠 수 있는지 여부
    /// </summary>
    /// <param name="item">대상 아이템</param>
    /// <returns>결과</returns>
    public bool Stackable(ItemStack item) {
        return IsEmpty() || StackableRestrict(item);
    }

    /// <summary>
    /// 해당 아이템과 겹칠 수 있는지 여부
    /// 단, 이 아이템이 Empty인 경우는 제외
    /// </summary>
    /// <param name="item">대상 아이템</param>
    /// <returns>결과</returns>
    public bool StackableRestrict(ItemStack item) {
        return item.type == type && Util.DeepEquals(item.tags, tags);
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

    public override string ToString() {
        string tags = "";
        foreach(ItemTag tag in this.tags) {
            tags += tag.ToString() + ", ";
        }
        if(tags.Length > 0) {
            tags = "{Tags: " + tags.Substring(0, tags.Length - 2) + "}";
        }
        return type.ToString() + " X" + amount + tags;
    }
}
