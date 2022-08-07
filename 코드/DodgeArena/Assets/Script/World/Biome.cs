using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Biome
{
    private static Dictionary<Type, Biome> biomeTypeMap = new Dictionary<Type, Biome>();
    public Biome this[Type t] {
        get => biomeTypeMap[t];
    }

    public enum Type {
        Forest,
        Plain
    }

    public static readonly Biome Forest = new Biome(Type.Forest);
    public static readonly Biome Plain = new Biome(Type.Plain);

    public readonly Type type;

    public Biome(Type type) {
        this.type = type;

        biomeTypeMap.Add(type, this);
    }

    public static implicit operator Biome.Type(Biome self) {
        return self.type;
    }

    public static implicit operator Biome(Biome.Type self) {
        return biomeTypeMap[self];
    }

    public override bool Equals(object obj) {
        if(( obj == null ) || !this.GetType().Equals(obj.GetType())) {
            return false;
        } else {
            Biome t = (Biome) obj;
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
