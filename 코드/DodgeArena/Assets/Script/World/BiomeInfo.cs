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
    [System.NonSerialized]
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
    [System.NonSerialized]
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
        List<Vector2> sourceChunks = new List<Vector2>();
        Vector2 offset = this.location.vector;
        int half = GameManager.instance.half_MaxBiomeSize;
        for(int x = -half; x <= half; x++) {
            for(int y = -half; y <= half; y++) {
                BiomeInfo info = world.GetBiomeInfo(new ChunkLocation(world, new Vector2(x + offset.x, y + offset.y)));
                if(info.isSource) {
                    sourceChunks.Add(info.location.vector);
                }
            }
        }

        //주변에 스폰할 바이옴의 개수 설정 (랜덤)
        int nearBiomes = Random.instance.RandRange(0, Mathf.FloorToInt(Mathf.Pow(( GameManager.instance.half_MaxBiomeSize / GameManager.instance.half_MinBiomeSize ) + 1, 2)));
        if(sourceChunks.Count < nearBiomes) {
            //모자르다면 스폰
            int generation = nearBiomes - sourceChunks.Count;
            List<Vector2> points = Util.SpreadLocation(generation, offset, GameManager.instance.half_MinBiomeSize, GameManager.instance.half_MaxBiomeSize, sourceChunks);

            //유효성 확인(최소거리 및 확정여부)
            foreach(Vector2 point in points) {
                bool valid = true;

                //이미 확정된 청크였다면 유효하지 않음
                ChunkLocation location = new ChunkLocation(world, point);
                BiomeInfo info = world.GetBiomeInfo(location);
                if(info.defined) {
                    valid = false;
                    break;
                }

                foreach(Vector2 checkPoint in points) {
                    if(point.Equals(checkPoint)) {
                        continue;
                    }

                    //인접 바이옴과의 거리가 최소 거리보다 더 작다면 유효하지 않음
                    float distance = Vector2.Distance(point, checkPoint);
                    if(distance < GameManager.instance.half_MinBiomeSize) {
                        valid = false;
                        break;
                    }
                }

                //유효한 경우에만 적용
                if(valid) {
                    info.defined = true;
                    info.sourceBiome = Random.instance.RandRange(0, 1) == 0 ? Biome.Forest : Biome.Plain;     //TODO 바이옴 랜덤 생성
                    info.isSource = true;
                    sourceChunks.Add(location.vector);
                }
            }
        }

        //주변 청크의 기원 바이옴을 전부 확정 처리
        for(int x = -half; x <= half; x++) {
            for(int y = -half; y <= half; y++) {
                BiomeInfo info = world.GetBiomeInfo(new ChunkLocation(world, new Vector2(x + offset.x, y + offset.y)));
                info.defined = true;
            }
        }

        //계산 완료
        foreach(Vector2 vec in sourceChunks) {
            BiomeInfo info = world.GetBiomeInfo(new ChunkLocation(world, vec));
            float distance = Vector2.Distance(location.vector, info.location.vector);
            if(!affectedBiomes.ContainsKey(info.sourceBiome)) {
                affectedBiomes.Add(info.sourceBiome, LerpBiomeEffect(distance));
            }
        }
    }

    /// <summary>
    /// 해당 거리에 있는 청크의 영향력을 반환합니다 <br></br>
    /// 선형 보간으로 작동합니다 (2022-08-07 기준)
    /// </summary>
    /// <param name="distance">거리</param>
    /// <returns>영향력</returns>
    public static float LerpBiomeEffect(float distance) {
        if(distance <= GameManager.instance.half_MinBiomeSize) {
            return 1;
        }else if(distance >= GameManager.instance.half_MinBiomeSize) {
            return 0;
        }

        //TODO 각자 거리 비교해서 언제나 합계는 1이 되도록
        return ( distance - GameManager.instance.half_MinBiomeSize ) / (GameManager.instance.half_MaxBiomeSize - GameManager.instance.half_MinBiomeSize);
    }

    /// <summary>
    /// 이 청크에 대한 해당 바이옴의 영향력을 반환합니다 <br></br>
    /// 반환값은 [0-1]의 값입니다
    /// </summary>
    /// <param name="biome">고려할 바이옴</param>
    /// <returns>해당 바이옴의 영향력</returns>
    public float getEffect(Biome biome) {
        if(!affectedBiomes.ContainsKey(biome)) {
            return 0;
        }

        return affectedBiomes[biome];
    }
}
