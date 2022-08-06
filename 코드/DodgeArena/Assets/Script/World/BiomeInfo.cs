using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

/// <summary>
/// ûũ�� ���̿� ����
/// </summary>
public class BiomeInfo {

    /// <summary>
    /// �� ûũ�� ��ġ <br></br>
    /// ����! ��� �Ϸ� ���� <code>location.chunk</code>�� ȣ���ϸ� ���ѷ����� ������
    /// </summary>
    [System.NonSerialized]
    public readonly ChunkLocation location;

    /// <summary>
    /// �� ûũ�� ��� ���̿� ������ Ȯ�� �Ǿ����� ���� <br></br>
    /// '�� ���̿�'�� '��� ���̿�' ���� ���θ� ������ <br></br>
    /// ���� �� ���� true�̴��� <see cref="calculated"/>�� false�� �� �ִ�
    /// </summary>
    public bool defined { get; private set; }
    /// <summary>
    /// �� ûũ�� ���̿� ������ ��� �Ǿ����� ���� <br></br>
    /// �ֺ� ��� ûũ�� ��� ���̿� ������ ��� �Ǿ��� ������ ���̻� �������� �ʴ´� <br></br>
    /// ���� �� ���� true�� ��� <see cref="defined"/>�� �׻� true�̴�.
    /// </summary>
    public bool calculated { get; private set; }
    /// <summary>
    /// �� ûũ�� ��� ���̿����� ����
    /// </summary>
    public bool isSource { get; private set; }
    /// <summary>
    /// ��� ���̿� ���� <br></br>
    /// <see cref="isSource"/>�� false��� null�̴�.
    /// </summary>
    public Biome sourceBiome { get; private set; }

    /// <summary>
    /// �� ûũ�� ������ �ִ� ���̿ȵ� (�� ûũ�� ����� ���̿ȵ� ����)
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
    /// �� ûũ�� ���̿� ������ ����Ѵ�
    /// </summary>
    public void Calculate() {
        if(calculated) {
            return;
        }

        this.calculated = true;
        World world = this.location.world;

        //�ֺ��� ��� ���̿� ������ �����´�
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

        //�ֺ��� ������ ���̿��� ���� ���� (����)
        int nearBiomes = Random.instance.RandRange(0, Mathf.FloorToInt(Mathf.Pow(( GameManager.instance.half_MaxBiomeSize / GameManager.instance.half_MinBiomeSize ) + 1, 2)));
        if(sourceChunks.Count < nearBiomes) {
            //���ڸ��ٸ� ����
            int generation = nearBiomes - sourceChunks.Count;
            List<Vector2> points = Util.SpreadLocation(generation, offset, GameManager.instance.half_MinBiomeSize, GameManager.instance.half_MaxBiomeSize, sourceChunks);

            //��ȿ�� Ȯ��(�ּҰŸ� �� Ȯ������)
            foreach(Vector2 point in points) {
                bool valid = true;

                //�̹� Ȯ���� ûũ���ٸ� ��ȿ���� ����
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

                    //���� ���̿Ȱ��� �Ÿ��� �ּ� �Ÿ����� �� �۴ٸ� ��ȿ���� ����
                    float distance = Vector2.Distance(point, checkPoint);
                    if(distance < GameManager.instance.half_MinBiomeSize) {
                        valid = false;
                        break;
                    }
                }

                //��ȿ�� ��쿡�� ����
                if(valid) {
                    info.defined = true;
                    info.sourceBiome = Random.instance.RandRange(0, 1) == 0 ? Biome.Forest : Biome.Plain;     //TODO ���̿� ���� ����
                    info.isSource = true;
                    sourceChunks.Add(location.vector);
                }
            }
        }

        //�ֺ� ûũ�� ��� ���̿��� ���� Ȯ�� ó��
        for(int x = -half; x <= half; x++) {
            for(int y = -half; y <= half; y++) {
                BiomeInfo info = world.GetBiomeInfo(new ChunkLocation(world, new Vector2(x + offset.x, y + offset.y)));
                info.defined = true;
            }
        }

        //��� �Ϸ�
        foreach(Vector2 vec in sourceChunks) {
            BiomeInfo info = world.GetBiomeInfo(new ChunkLocation(world, vec));
            float distance = Vector2.Distance(location.vector, info.location.vector);
            if(!affectedBiomes.ContainsKey(info.sourceBiome)) {
                affectedBiomes.Add(info.sourceBiome, LerpBiomeEffect(distance));
            }
        }
    }

    /// <summary>
    /// �ش� �Ÿ��� �ִ� ûũ�� ������� ��ȯ�մϴ� <br></br>
    /// ���� �������� �۵��մϴ� (2022-08-07 ����)
    /// </summary>
    /// <param name="distance">�Ÿ�</param>
    /// <returns>�����</returns>
    public static float LerpBiomeEffect(float distance) {
        if(distance <= GameManager.instance.half_MinBiomeSize) {
            return 1;
        }else if(distance >= GameManager.instance.half_MinBiomeSize) {
            return 0;
        }

        //TODO ���� �Ÿ� ���ؼ� ������ �հ�� 1�� �ǵ���
        return ( distance - GameManager.instance.half_MinBiomeSize ) / (GameManager.instance.half_MaxBiomeSize - GameManager.instance.half_MinBiomeSize);
    }

    /// <summary>
    /// �� ûũ�� ���� �ش� ���̿��� ������� ��ȯ�մϴ� <br></br>
    /// ��ȯ���� [0-1]�� ���Դϴ�
    /// </summary>
    /// <param name="biome">����� ���̿�</param>
    /// <returns>�ش� ���̿��� �����</returns>
    public float getEffect(Biome biome) {
        if(!affectedBiomes.ContainsKey(biome)) {
            return 0;
        }

        return affectedBiomes[biome];
    }
}
