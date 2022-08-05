using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ItemType : MonoBehaviour
{
    private static Dictionary<Type, ItemType> itemTypeMap = new Dictionary<Type, ItemType>();
    public ItemType this[Type t] {
        get => itemTypeMap[t];
    }

    public enum Type {
        WildBoarsHair,
        WildBoarsCanine
    }

    public static readonly ItemType WildBoarsHair = new ItemType(Type.WildBoarsHair);
    public static readonly ItemType WildBoarsCanine = new ItemType(Type.WildBoarsCanine);

    public readonly Type type;

    public ItemType(Type type) {
        this.type = type;

        itemTypeMap.Add(type, this);
    }

    public override bool Equals(object obj) {
        if(( obj == null ) || !this.GetType().Equals(obj.GetType())) {
            return false;
        } else {
            ItemType t = (ItemType) obj;
            return t.type == type;
        }
    }

    public override int GetHashCode() {
        return HashCode.Combine(type);
    }
}
