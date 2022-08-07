using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WorldType
{
    private static Dictionary<Type, WorldType> worldTypeMap = new Dictionary<Type, WorldType>();
    public WorldType this[Type t] {
        get => worldTypeMap[t];
    }

    public enum Type {
        Main,
        Sub
    }

    public static readonly WorldType Main = new WorldType(Type.Main);
    public static readonly WorldType Sub = new WorldType(Type.Sub);

    private readonly Type type;

    public WorldType(Type type) {
        this.type = type;

        worldTypeMap.Add(type, this);
    }

    public static implicit operator WorldType.Type(WorldType self) {
        return self.type;
    }

    public static implicit operator WorldType(WorldType.Type self) {
        return worldTypeMap[self];
    }

    public override bool Equals(object obj) {
        if(( obj == null ) || !this.GetType().Equals(obj.GetType())) {
            return false;
        } else {
            WorldType t = (WorldType) obj;
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
