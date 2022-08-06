using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class World : MonoBehaviour {
    public WorldType type;

    public bool initiated { private set; get; }
    public bool loaded { private set; get; }
    private Dictionary<ChunkLocation, Chunk> chunks = new Dictionary<ChunkLocation, Chunk>();

    public void Initiate(WorldType type) {
        if(initiated) {
            return;
        }

        this.type = type;
        this.loaded = false;
        gameObject.SetActive(false);
        this.initiated = true;
#if UNITY_EDITOR
        gameObject.name = type.type.ToString();
#endif
    }

    public void Load() {
        if(loaded) {
            return;
        }

        gameObject.SetActive(true);
        loaded = true;
    }

    public void Unload() {
        if(!loaded) {
            return;
        }

        loaded = false;
        ChunkLocation[] locations = new ChunkLocation[chunks.Keys.Count];
        chunks.Keys.CopyTo(locations, 0);
        foreach(ChunkLocation location in locations) {
            RemoveChunk(location);
        }
        gameObject.SetActive(false);
    }

    #region Chunk

    public void UpdateChunkState(ChunkLocation location) {
        Vector2 pos = GameManager.instance.player.location.vector2;
        if(location.CheckLoad(pos)) {
            if(!chunks.ContainsKey(location)) {
                SpawnChunk(location);
            }
            LoadChunk(location);
        } else if(location.CheckKeep(pos)) {
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

        Chunk chunk = Instantiate(GameManager.instance.chunkPrefab, location.center.vector, Quaternion.identity, transform);
        chunks.Add(location, chunk);
        chunk.ResetProperties(location, Biome.Plain);
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
        instance.Initiated(location);
        instance.OnSpawn();
        if(chunk.loaded) {
            instance.OnLoad();
        } else {
            instance.OnUnload();
        }
        return instance;
    }

    public override bool Equals(object obj) {
        if(( obj == null ) || !this.GetType().Equals(obj.GetType())) {
            return false;
        } else {
            World p = (World) obj;
            return p.type.Equals(type);
        }
    }

    public override int GetHashCode() {
        return HashCode.Combine(type);
    }
}