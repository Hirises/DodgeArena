using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "WorldType", menuName = "Data/WorldType")]
public class WorldType : ScriptableObject
{
    public static Dictionary<WorldTypeEnum, WorldType> worldTypeMap = new Dictionary<WorldTypeEnum, WorldType>();
    public WorldType this[WorldTypeEnum t] {
        get => worldTypeMap[t];
    }

    public WorldTypeEnum enumType;

    public WorldType(WorldTypeEnum type) {
        this.enumType = type;

        worldTypeMap.Add(type, this);
    }

    public static implicit operator WorldTypeEnum(WorldType self) {
        return self.enumType;
    }

    public static implicit operator WorldType(WorldTypeEnum self) {
        return worldTypeMap[self];
    }

    public override bool Equals(object obj) {
        if(( obj == null ) || !this.GetType().Equals(obj.GetType())) {
            return false;
        } else {
            WorldType t = (WorldType) obj;
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
