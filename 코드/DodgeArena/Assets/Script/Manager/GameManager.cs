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
    [BoxGroup("Generator")]
    public ChunkDataGenerator[] chunkDataGenerators;
    [SerializeField]
    [BoxGroup("Generator")]
    public EntityGenerator[] entityGenerators;

    [SerializeField]
    [BoxGroup("World")]
    public GameObject worldRoot;
    [SerializeField]
    [BoxGroup("World")]
    public World worldPrefab;

    [SerializeField]
    [BoxGroup("Biome")]
    [OnValueChanged(nameof(setMaxAffectedBiomeAmount))]
    public int half_MinBiomeSize_Chunk;
    [SerializeField]
    [BoxGroup("Biome")]
    [OnValueChanged(nameof(setMaxAffectedBiomeAmount))]
    public int half_MaxBiomeSize_Chunk;
    [ReadOnly]
    [SerializeField]
    [BoxGroup("Biome")]
    public int maxAffectedBiomeAmount;
    public void setMaxAffectedBiomeAmount() {
        maxAffectedBiomeAmount = Mathf.FloorToInt(Mathf.Pow(( half_MaxBiomeSize_Chunk / ( (half_MinBiomeSize_Chunk - 1) * 2 + 1 ) ) + 1, 2));
    }

    [SerializeField]
    [BoxGroup("Chunk")]
    public Chunk chunkPrefab;
    [SerializeField]
    [BoxGroup("Chunk")]
    public float half_ChunkUpdateRange;
    [SerializeField]
    [BoxGroup("Chunk")]
    public float half_ChunkSaveRange;
    [SerializeField]
    [BoxGroup("Chunk")]
    public float half_ChunkLoadRange;
    [SerializeField]
    [BoxGroup("Chunk")]
    public float half_ChunkWeidth;

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

    #region UnityLifecycle
    private void Awake() {
        if(instance == null) {
            instance = this;
        } else {
            Destroy(transform);
        }

        state = GameState.Stop;
        LoadWorld(WorldType.Main);
        WorldLocation startLocation = new WorldLocation(GetWorld(WorldType.Main), new Vector2(0, 0));
        player.Initiated(startLocation);
        player.OnSpawn();
        state = GameState.Run;
    }

    private void Update() {
        if(state == GameState.Run) {
            UpdateChunkState();
        }
        CheckPlayerInput();
        Test();
    }

    public void CheckPlayerInput() {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            if(state == GameState.Run) {
                state = GameState.Stop;
            } else {
                state = GameState.Run;
            }
        }
    }

    public void Test() {
        if(Input.GetKeyDown(KeyCode.Space)) {
            player.Teleport(new WorldLocation(LoadWorld(WorldType.Sub), new Vector2(0, 0)));
        }

        string biomeInfo = "";
        foreach(Biome biome in player.chunk.biomeInfo.affectedBiomes.Keys) {
            biomeInfo += biome.type.ToString() + " " + player.chunk.biomeInfo.affectedBiomes[biome] + "\n";
        }

        debugText.text = player.hp.ToString() + "\n" + biomeInfo;
    }

    public void UpdateChunkState() {
        World world = player.location.world;
        int half_LoadRange = (int) Mathf.Floor(half_ChunkUpdateRange / half_ChunkWeidth);
        Vector2 offset = player.location.chunkLocation.vector;
        for(int x = -half_LoadRange; x <= half_LoadRange; x++) {
            for(int y = -half_LoadRange; y <= half_LoadRange; y++) {
                ChunkLocation location = new ChunkLocation(world, new Vector2(x + offset.x, y + offset.y));
                world.UpdateChunkState(location);
            }
        }
    } 
    #endregion

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

    /// <summary>
    /// 해당 청크에 대해 사용 가능한 <see cref="ChunkDataGenerator"/>들의 목록을 반환합니다
    /// </summary>
    /// <param name="chunk">확인할 청크</param>
    /// <returns>가능한 목록</returns>
    public List<ChunkDataGenerator> GetPossibleChunkDataGenerators(Chunk chunk) {
        List<ChunkDataGenerator> output = new List<ChunkDataGenerator>();
        foreach(ChunkDataGenerator generator in chunkDataGenerators) {
            if(generator.CheckConditions(chunk)) {
                output.Add(generator);
            }
        }
        return output;
    }

    /// <summary>
    /// 해당 청크에 대한 랜덤한 <see cref="ChunkDataGenerator"/>를 반환합니다
    /// </summary>
    /// <param name="chunk">확인할 청크</param>
    /// <returns>반환된 생성자</returns>
    public ChunkDataGenerator GetChunkDataGenerator(Chunk chunk) {
        return Util.GetByWeigth(GetPossibleChunkDataGenerators(chunk), val => val.weight);
    }

    public void GameEnd() {
        state = GameState.Stop;
        foreach(WorldType type in worlds.Keys) {
            UnloadWorld(type);
        }
        SceneManager.LoadScene(Scene.MainScene.name);
    }
}
