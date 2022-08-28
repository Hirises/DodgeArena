using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "BiomeInfo", menuName = "Data/BiomeInfo")]
public class Biome : ScriptableObject
{
    public static Dictionary<BiomeTypeEnum, Biome> biomeTypeMap = new Dictionary<BiomeTypeEnum, Biome>();
    public Biome this[BiomeTypeEnum t] {
        get => biomeTypeMap[t];
    }

    public BiomeTypeEnum enumType;

    public static implicit operator BiomeTypeEnum(Biome self) {
        return self.enumType;
    }

    public static implicit operator Biome(BiomeTypeEnum self) {
        return biomeTypeMap[self];
    }

    public override bool Equals(object obj) {
        if(( obj == null ) || !this.GetType().Equals(obj.GetType())) {
            return false;
        } else {
            Biome t = (Biome) obj;
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
