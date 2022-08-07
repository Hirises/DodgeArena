using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

/// <summary>
/// 청크의 바이옴 정보
/// </summary>
public class BiomeInfo {

    /// <summary>
    /// 이 청크의 위치 <br></br>
    /// 주의! 계산 완료 전에 <code>location.chunk</code>를 호출하면 무한루프에 빠진다
    /// </summary>
    public readonly ChunkLocation location;

    /// <summary>
    /// 이 청크의 기원 바이옴 정보가 확정 되었는지 여부 <br></br>
    /// '이 바이옴'의 '기원 바이옴' 설정 여부만 따진다 <br></br>
    /// 따라서 이 값이 true이더라도 <see cref="calculated"/>는 false일 수 있다
    /// </summary>
    public bool defined { get; private set; }
    /// <summary>
    /// 이 청크의 바이옴 정보가 계산 되었는지 여부 <br></br>
    /// 주변 모든 청크의 기원 바이옴 정보가 계산 되었기 때문에 더이상 수정되지 않는다 <br></br>
    /// 따라서 이 값이 true인 경우 <see cref="defined"/>도 항상 true이다.
    /// </summary>
    public bool calculated { get; private set; }
    /// <summary>
    /// 이 청크가 기원 바이옴인지 여부
    /// </summary>
    public bool isSource { get; private set; }
    /// <summary>
    /// 기원 바이옴 정보 <br></br>
    /// <see cref="isSource"/>가 false라면 null이다.
    /// </summary>
    public Biome sourceBiome { get; private set; }

    /// <summary>
    /// 이 청크에 영향을 주는 바이옴들 (이 청크가 기원인 바이옴도 포함)
    /// </summary>
    public readonly Dictionary<Biome, float> affectedBiomes;

    public BiomeInfo(ChunkLocation location) {
        this.defined = false;
        this.isSource = false;
        this.sourceBiome = null;
        this.location = location;
        this.affectedBiomes = new Dictionary<Biome, float>();
    }

    /// <summary>
    /// 이 청크의 바이옴 정보를 계산한다
    /// </summary>
    public void Calculate() {
        if(calculated) {
            return;
        }

        this.calculated = true;
        World world = this.location.world;

        //주변의 기원 바이옴 정보를 가져온다
        List<Vector2> sourceBiomeChunks = new List<Vector2>();
        List<BiomeInfo> undefinedChunks = new List<BiomeInfo>();
        Vector2 offset = this.location.vector;
        int half = GameManager.instance.half_MaxBiomeSize_Chunk - 1;
        for(int x = -half; x <= half; x++) {
            for(int y = -half; y <= half; y++) {
                BiomeInfo info = world.GetBiomeInfo(new ChunkLocation(world, new Vector2(x + offset.x, y + offset.y)));
                if(info.isSource) {
                    sourceBiomeChunks.Add(info.location.vector);
                }
                if(!info.defined) {
                    undefinedChunks.Add(info);
                }
            }
        }

        //주변에 스폰할 바이옴의 개수 설정 (랜덤)
        int nearBiomes = Random.instance.RandRange(1, Random.instance.RandRange(1, GameManager.instance.maxAffectedBiomeAmount));
        if(sourceBiomeChunks.Count < nearBiomes) {
            //모자르다면 스폰
            int generation = nearBiomes - sourceBiomeChunks.Count;
            List<Vector2> points = Util.SpreadLocationForChunk(generation, offset, (GameManager.instance.half_MinBiomeSize_Chunk - 1 ) * 2, GameManager.instance.half_MaxBiomeSize_Chunk - 1, sourceBiomeChunks);

            //유효성 확인(최소거리 및 확정여부)
            for(int index = 0; index < points.Count; index++) {
                Vector2 point = new ChunkLocation(world, points[index]).vector;
                bool valid = true;

                //이미 확정된 청크였다면 유효하지 않음
                ChunkLocation location = new ChunkLocation(world, point);
                BiomeInfo info = world.GetBiomeInfo(location);
                if(info.defined) {
                    break;
                }

                for(int checkIndex = index + 1; checkIndex < points.Count; checkIndex++) {
                    Vector2 checkPoint = points[checkIndex];

                    //인접 바이옴과의 거리가 최소 거리보다 더 작다면 유효하지 않음
                    float distance = Util.DistanceSquare(point, checkPoint);
                    if(distance < ( GameManager.instance.half_MinBiomeSize_Chunk - 1 ) * 2) {
                        valid = false;
                        break;
                    }
                }

                //유효한 경우에만 적용
                if(valid) {
                    info.defined = true;
                    info.sourceBiome = Random.instance.RandRange(0, 1) == 0 ? Biome.Forest : Biome.Plain;     //TODO 바이옴 랜덤 생성
                    info.isSource = true;
                    sourceBiomeChunks.Add(info.location.vector);
                }
            }
        }

        int forceSpawnIndex = -1;
        //기원 바이옴이 하나도 없다면
        if(sourceBiomeChunks.Count <= 0) {
            //강제로 한개 생성
            forceSpawnIndex = Random.instance.RandInt(0, undefinedChunks.Count);
        }

        //주변 청크의 기원 바이옴을 전부 확정 처리
        for(int index = 0; index < undefinedChunks.Count; index++) {
            BiomeInfo info = undefinedChunks[index];
            info.defined = true;

            if(forceSpawnIndex == index) {
                info.sourceBiome = Random.instance.RandRange(0, 1) == 0 ? Biome.Forest : Biome.Plain;     //TODO 바이옴 랜덤 생성
                info.isSource = true;
                sourceBiomeChunks.Add(info.location.vector);
            }
        }

        //계산 결과 반영
        float total = 0;
        foreach(Vector2 vec in sourceBiomeChunks) {
            //이 청크에 대한 바이옴 영향 계산
            BiomeInfo info = world.GetBiomeInfo(new ChunkLocation(world, vec));
            float distance = 1.0f / (Util.DistanceSquare(location.vector, vec) - ( GameManager.instance.half_MinBiomeSize_Chunk - 1 ) + 1);
            if(distance < GameManager.instance.half_MinBiomeSize_Chunk) { //바이옴의 최소 거리 안쪽이면
                //해당 바이옴만 적용됨
                affectedBiomes.Clear();
                affectedBiomes.Add(info.sourceBiome, 1);
                total = 1;
                break;
            }
            if(!affectedBiomes.ContainsKey(info.sourceBiome)) {
                affectedBiomes.Add(info.sourceBiome, distance);
            } else {
                affectedBiomes.Add(info.sourceBiome, affectedBiomes[info.sourceBiome] + distance);
            }
            total += distance;
        }
        List<Biome> copy = new List<Biome>();
        copy.AddRange(affectedBiomes.Keys);
        foreach(Biome biome in copy) {
            affectedBiomes[biome] = affectedBiomes[biome] / total;
        }
    }
}
