using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum GameState {
        Stop,
        Run
    }
    public static GameManager instance;

    [SerializeField]
    [BoxGroup("Reference")]
    public Player player;

    [SerializeField]
    [BoxGroup("Spawner")]
    public Spawner[] spawners;
    [SerializeField]
    [BoxGroup("Spawner")]
    public SpawnDataSetter spawnDataSetter;

    [SerializeField]
    [BoxGroup("World")]
    public GameObject worldRoot;
    [SerializeField]
    [BoxGroup("World")]
    public World worldPrefab;

    [SerializeField]
    [BoxGroup("Chunk")]
    public Chunk chunkPrefab;
    [SerializeField]
    [BoxGroup("Chunk")]
    public float chunkUpdateRange;
    [SerializeField]
    [BoxGroup("Chunk")]
    public float chunkSaveRange;
    [SerializeField]
    [BoxGroup("Chunk")]
    public float chunkLoadRange;
    [SerializeField]
    [BoxGroup("Chunk")]
    public float chunkWeidth;

    [SerializeField]
    [BoxGroup("Test")]
    public TextMeshProUGUI debugText;

    public Dictionary<WorldType, World> worlds = new Dictionary<WorldType, World>();

    private GameState _state;
    public GameState state {
        get => _state;
        private set {
            if(value == GameState.Run) {
                Time.timeScale = 1;
            }else if(value == GameState.Stop) {
                Time.timeScale = 0;
            }
            _state = value;
        }
    }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(transform);
        }

        state = GameState.Run;
        WorldLocation startLocation = new WorldLocation(LoadWorld(WorldType.Main), new Vector2(0, 0));
        player.Initiated(startLocation, startLocation.chunk);
        player.OnSpawn();
    }

    private void Update() {
        UpdateChunkState();
        Test();
    }

    public void Test() {
        debugText.text = player.hp.ToString();
    }

    public void UpdateChunkState() {
        World world = player.location.world;
        int loadRange = (int) Mathf.Floor(chunkUpdateRange / chunkWeidth);
        Vector2 offset = player.location.chunkLocation.vector;
        for(int x = -loadRange; x <= loadRange; x++) {
            for(int y = -loadRange; y <= loadRange; y++) {
                ChunkLocation location = new ChunkLocation(world, new Vector2(x + offset.x, y + offset.y));
                world.UpdateChunkState(location);
            }
        }
    }

    public void GameEnd() {
        state = GameState.Stop;
        foreach(WorldType type in worlds.Keys) {
            UnloadWorld(type);
        }
        SceneManager.LoadScene("TempMenuScene");
    }

    #region World
    public World GetWorld(WorldType type) {
        if(worlds.ContainsKey(type)) {
            return worlds[type];
        }
        return SpawnWorld(type);
    }

    public World SpawnWorld(WorldType type) {
        World world = Instantiate(worldPrefab, worldRoot.transform);
        worlds.Add(type, world);
        world.Initiate(type);
        return world;
    }

    public World LoadWorld(WorldType type) {
        World world = GetWorld(type);
        world.Load();
        return world;
    }

    public void UnloadWorld(WorldType type) {
        if(!worlds.ContainsKey(type)) {
            return;
        }

        GetWorld(type).Unload();
    } 
    #endregion
}
