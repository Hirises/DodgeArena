using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class GameData : MonoBehaviour {
    public static GameData instance { private set; get; }
    public delegate void Runnable();

    [SerializeField]
    [BoxGroup("Item")]
    public List<ItemType> allItems;

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
    }
}