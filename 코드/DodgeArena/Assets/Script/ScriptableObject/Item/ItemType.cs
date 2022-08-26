using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "ItemType", menuName = "Item")]
public class ItemType : ScriptableObject
{
    public static Dictionary<ItemTypeEnum, ItemType> itemTypeMap = new Dictionary<ItemTypeEnum, ItemType>();
    public ItemType this[ItemTypeEnum t] {
        get => itemTypeMap[t];
    }

    public ItemTypeEnum enumType;
    public int maxStackSize;

    public static implicit operator ItemTypeEnum(ItemType self) {
        return self.enumType;
    }

    public static implicit operator ItemType(ItemTypeEnum self) {
        return itemTypeMap[self];
    }

    public override bool Equals(object obj) {
        if(( obj == null ) || !this.GetType().Equals(obj.GetType())) {
            return false;
        } else {
            ItemType t = (ItemType) obj;
            return t.enumType == enumType;
        }
    }

    public override int GetHashCode() {
        return HashCode.Combine(enumType);
    }

    public override string ToString() {
        return enumType.ToString();
    }
}
