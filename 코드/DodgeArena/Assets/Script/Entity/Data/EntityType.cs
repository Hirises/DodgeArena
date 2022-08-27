using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "EntityData", menuName = "Entity/EntityData")]
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
    private List<Sprite> sprites;
    [SerializeField]
    private List<string> lable;

    public Sprite GetSprite(string tag) {
        return sprites[lable.LastIndexOf(tag)];
    }

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
