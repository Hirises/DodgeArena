using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using NaughtyAttributes;

/// <summary>
/// 엔티티의 타입
/// 엔티티에 대한 기초 정보를 담고 있다.
/// </summary>
[CreateAssetMenu(fileName = "EntityType", menuName = "Data/EntityType")]
public class EntityType : ScriptableObject
{
    public static Dictionary<EntityTypeEnum, EntityType> entityTypeMap = new Dictionary<EntityTypeEnum, EntityType>();
    public EntityType this[EntityTypeEnum t]
    {
        get => entityTypeMap[t];
    }

    [SerializeField]
    private EntityTypeEnum _type;
    public EntityTypeEnum enumType {
        get => _type;
    }
    [SerializeField]
    public Entity prefab;
    [SerializeField]
    public Sprite sprite;
    [SerializeField]
    public new string name;
    [SerializeField]
    [ResizableTextArea]
    public string information;

    public EntityType(EntityTypeEnum type)
    {
        this._type = type;

        entityTypeMap.Add(type, this);
    }

    public static implicit operator EntityTypeEnum(EntityType self) {
        return self.enumType;
    }

    public static implicit operator EntityType(EntityTypeEnum self) {
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
            return t.enumType == enumType;
        }
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(enumType);
    }

    public override string ToString() {
        return enumType.ToString();
    }
}
