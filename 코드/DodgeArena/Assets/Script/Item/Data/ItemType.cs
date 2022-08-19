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
        IronOre,
        WildBoarsHair,
        WildBoarsCanine
    }

    public static readonly ItemType IronOre = new ItemType(Type.IronOre);
    public static readonly ItemType WildBoarsHair = new ItemType(Type.WildBoarsHair);
    public static readonly ItemType WildBoarsCanine = new ItemType(Type.WildBoarsCanine);

    private readonly Type type;

    public ItemType(Type type) {
        this.type = type;

        itemTypeMap.Add(type, this);
    }

    public static implicit operator ItemType.Type(ItemType self) {
        return self.type;
    }

    public static implicit operator ItemType(ItemType.Type self) {
        return itemTypeMap[self];
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

    public override string ToString() {
        return type.ToString();
    }
}
