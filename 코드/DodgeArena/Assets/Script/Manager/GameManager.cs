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
    [BoxGroup("Chunk")]
    public GameObject objectsRoot;
    [SerializeField]
    [BoxGroup("Chunk")]
    public Chunk chunkObject;
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
    private Dictionary<ChunkLocation, Chunk> chunks = new Dictionary<ChunkLocation, Chunk>();

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
        player.location = new WorldLocation(new Vector2(0, 0));
        player.OnSpawn();
        UpdateChunk();
    }

    private void Update()
    {
        if(state == GameState.Run) {
            UpdateChunk();
        }
        Test();
    }

    public void Test() {
        debugText.text = player.hp.ToString();
    }

    #region Chunk
    public void UpdateChunk() {
        int loadRange = (int) Mathf.Floor(chunkUpdateRange / chunkWeidth);
        Vector2 offset = new WorldLocation(player.transform.position).chunkLocation.vector;
        for(int x = -loadRange; x <= loadRange; x++) {
            for(int y = -loadRange; y <= loadRange; y++) {
                ChunkLocation location = new ChunkLocation(new Vector2(x + offset.x, y + offset.y));
                CheckChunk(location);
            }
        }
    }

    public void CheckChunk(ChunkLocation location) {
        if(location.CheckLoad()) {
            if(!chunks.ContainsKey(location)) {
                SpawnChunk(location);
            }
            LoadChunk(location);
        } else if(location.CheckKeep()) {
            if(!chunks.ContainsKey(location)) {
                SpawnChunk(location);
                return;
            }
            UnloadChunk(location);
        } else {
            RemoveChunk(location);
        }
    }

    public Chunk GetChunk(ChunkLocation location) {
        if(!chunks.ContainsKey(location)) {
            return SpawnChunk(location);
        }
        Chunk chunk = chunks[location];
        return chunk;
    }

    public Chunk SpawnChunk(ChunkLocation location) {
        if(chunks.ContainsKey(location)) {
            return GetChunk(location);
        }

        Chunk chunk = Instantiate(chunkObject, location.center.vector, Quaternion.identity, objectsRoot.transform);
        chunks.Add(location, chunk);
        chunk.ResetProperties(location);
        return chunk;
    }

    public Chunk LoadChunk(ChunkLocation location) {
        if(chunks.ContainsKey(location) && GetChunk(location).loaded) {
            return GetChunk(location);
        }
        Chunk chunk = GetChunk(location);
        chunk.Load();
        return chunk;
    }

    public void UnloadChunk(ChunkLocation location) {
        if(!chunks.ContainsKey(location) || !GetChunk(location).loaded) {
            return;
        }

        GetChunk(location).Unload();
    }

    public void RemoveChunk(ChunkLocation location) {
        if(!chunks.ContainsKey(location)) {
            return;
        }

        RemoveChunk(GetChunk(location));
    }

    public void RemoveChunk(Chunk chunk) {
        chunk.Unload();
        List<Entity> copy = new List<Entity>();
        copy.AddRange(chunk.entities);
        foreach(Entity entity in copy) {
            entity.Remove();
        }
        chunks.Remove(chunk.location);
        Destroy(chunk.gameObject);
    } 
    #endregion

    public void GameEnd() {
        state = GameState.Stop;
        ChunkLocation[] locations = new ChunkLocation[chunks.Keys.Count];
        chunks.Keys.CopyTo(locations, 0);
        foreach(ChunkLocation location in locations) {
            RemoveChunk(location);
        }
        SceneManager.LoadScene("TempMenuScene");
    }

    /// <summary>
    /// 입력된 개체를 월드에 생성합니다
    /// </summary>
    /// <typeparam name="T">생성할 개체의 타입</typeparam>
    /// <param name="target">생성할 개체</param>
    /// <param name="location">생성할 위치</param>
    /// <returns>생성된 개체</returns>
    public T Spawn<T>(T target, WorldLocation location) where T : Entity {
        Chunk chunk = location.chunk;
        T instance = Instantiate(target, location.vector, Quaternion.identity, chunk.gameObject.transform);
        instance.Initiated(location, chunk);
        instance.OnSpawn();
        if(chunk.loaded) {
            instance.OnLoad();
        } else {
            instance.OnUnload();
        }
        return instance;
    }
}
