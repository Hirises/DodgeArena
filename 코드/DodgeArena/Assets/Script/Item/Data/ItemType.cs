using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using RotaryHeart.Lib.SerializableDictionary;

[CreateAssetMenu(fileName = "ItemType", menuName = "Item/ItemType")]
public class ItemType : ScriptableObject
{
    public static Dictionary<ItemTypeEnum, ItemType> itemTypeMap = new Dictionary<ItemTypeEnum, ItemType>();
    public ItemType this[ItemTypeEnum t] {
        get {
            if(itemTypeMap.ContainsKey(t)) {
                return itemTypeMap[t];
            }
            return Empty();
        }
    }

    public ItemTypeEnum enumType;
    public int maxStackSize;
    [SerializeField]
    private SerializableDictionaryBase<string, Sprite> sprites = new SerializableDictionaryBase<string, Sprite>();
    [SerializeField]
    private SerializableDictionaryBase<string, string> data = new SerializableDictionaryBase<string, string>();

    public Sprite GetSprite(string tag) {
        return sprites[tag];
    }

    public string GetData(string tag) {
        return data[tag];
    }

    private static ItemType Empty() {
        ItemType instance = CreateInstance<ItemType>();
        instance.enumType = ItemTypeEnum.Empty;
        instance.maxStackSize = 0;
        return instance;
    }

    public static implicit operator ItemTypeEnum(ItemType self) {
        return self.enumType;
    }

    public static implicit operator ItemType(ItemTypeEnum self) {
        if(itemTypeMap.ContainsKey(self)) {
            return itemTypeMap[self];
        }
        return Empty();
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
