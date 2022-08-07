using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EntityType
{
    private static Dictionary<Type, EntityType> entityTypeMap = new Dictionary<Type, EntityType>();
    public EntityType this[Type t]
    {
        get => entityTypeMap[t];
    }

    public enum Type
    {
        Player,
        Grass,
        WildBoar
    }

    public static readonly EntityType Player = new EntityType(Type.Player);
    public static readonly EntityType Grass = new EntityType(Type.Grass);
    public static readonly EntityType WildBoar = new EntityType(Type.WildBoar);

    private readonly Type type;

    public EntityType(Type type)
    {
        this.type = type;

        entityTypeMap.Add(type, this);
    }

    public static implicit operator EntityType.Type(EntityType self) {
        return self.type;
    }

    public static implicit operator EntityType(EntityType.Type self) {
        return entityTypeMap[self];
    }

    public override bool Equals(object obj)
    {
        if ((obj == null) || !this.GetType().Equals(obj.GetType()))
        {
            return false;
        }
        else
        {
            EntityType t = (EntityType)obj;
            return t.type == type;
        }
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(type);
    }

    public override string ToString() {
        return type.ToString();
    }
}
