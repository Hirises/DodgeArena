using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class GameData : MonoBehaviour {
    public static GameData instance { private set; get; }

    [SerializeField]
    [BoxGroup("World")]
    public List<WorldType> allWorlds;

    [SerializeField]
    [BoxGroup("Entity")]
    public List<EntityType> allEntities;

    [SerializeField]
    [BoxGroup("Item")]
    public List<ItemType> allItems;

    [SerializeField]
    [BoxGroup("Biome")]
    public List<Biome> allBiomes;

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

        foreach(WorldType world in allWorlds) {
            WorldType.worldTypeMap.Add(world.enumType, world);
        }
        allWorlds.Clear();
        allWorlds = null;

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

        foreach(Biome biome in allBiomes) {
            Biome.biomeTypeMap.Add(biome.enumType, biome);
        }
        allBiomes.Clear();
        allBiomes = null;
    }
}