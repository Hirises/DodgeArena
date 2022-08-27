using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class GameData : MonoBehaviour {
    public static GameData instance { private set; get; }

    [SerializeField]
    [BoxGroup("Entity")]
    public List<EntityType> allEntities;

    [SerializeField]
    [BoxGroup("Item")]
    public List<ItemType> allItems;

    [SerializeField]
    [BoxGroup("Generator")]
    public List<BiomeGenerator> biomeGenerators;
    [SerializeField]
    [BoxGroup("Generator")]
    public List<ChunkDataGenerator> chunkDataGenerators;
    [SerializeField]
    [BoxGroup("Generator")]
    public List<EntityGenerator> entityGenerators;

    private void Awake() {
        if(instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        foreach(ItemType item in allItems) {
            ItemType.itemTypeMap.Add(item.enumType, item);
        }
        allItems.Clear();
        allItems = null;

        foreach(EntityType entity in allEntities) {
            EntityType.entityTypeMap.Add(entity.enumType, entity);
        }
        allEntities.Clear();
        allEntities = null;
    }
}