using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Util
{
    /// <summary>
    /// 3D���� ���͸� 2D �������� �����մϴ�. <br/>
    /// ������ x, y ������ �̿��մϴ�
    /// </summary>
    /// <param name="vector">������ ����</param>
    /// <returns>����� ����</returns>
    public static Vector2 FlattenLocation(Vector3 vector)
    {
        return new Vector2(vector.x, vector.y);
    }

    /// <summary>
    /// �Էµ� ����� �ٶ󺸴� ȸ������ ���Ϸ� ������ ��ȯ�մϴ�. <br/>
    /// 2D �������� ����ϸ�, Zȸ���� 0�� �� �������� �ٶ󺸰� �ִٰ� �����մϴ�.
    /// </summary>
    /// <param name="selfPos">�ڽ��� ��ġ</param>
    /// <param name="targetPos">�ٶ� ����� ��ġ</param>
    /// <returns>����� �ٶ󺸴� ȸ����</returns>
    public static Vector3 LootAtRotation(Vector3 selfPos, Vector3 targetPos)
    {
        return LootAtRotation(selfPos, targetPos, Vector2.right);
    }

    /// <summary>
    /// �Էµ� ����� �ٶ󺸴� ȸ������ ���Ϸ� ������ ��ȯ�մϴ�. <br/>
    /// 2D �������� ����մϴ�.
    /// </summary>
    /// <param name="selfPos">�ڽ��� ��ġ</param>
    /// <param name="targetPos">�ٶ� ����� ��ġ</param>
    /// <param name="zeroRotation">Zȸ���� 0�϶� �ٶ󺸴� ����</param>
    /// <returns>����� �ٶ󺸴� ȸ����</returns>
    public static Vector3 LootAtRotation(Vector3 selfPos, Vector3 targetPos, Vector2 zeroRotation)
    {
        Vector2 dir = ToVector(selfPos, targetPos);
        dir.Normalize();
        float angle = Vector2.SignedAngle(zeroRotation, dir);
        return new Vector3(0, 0, angle);
    }

    /// <summary>
    /// �Էµ� ����� �ٶ󺸴� ���͸� ��ȯ�մϴ� <br/>
    /// 2D �������� ����մϴ�
    /// </summary>
    /// <param name="selfPos">�ڽ��� ��ġ</param>
    /// <param name="targetPos">�ٶ� ����� ��ġ</param>
    /// <returns>����� �ٶ󺸴� ����</returns>
    public static Vector2 ToVector(Vector3 selfPos, Vector3 targetPos)
    {
        return new Vector2(targetPos.x - selfPos.x, targetPos.y - selfPos.y);
    }

    /// <summary>
    /// �����ϰ� ������ ��Ѹ��ϴ� <br></br>
    /// �Էµ� �߽����� ���������ϴ� ���簢�� ����� ������ �����մϴ� <br></br>
    /// ��� ��쿡 �Ϻ��ϰ� ��ѷ����� ���� �� �ֽ��ϴ�
    /// </summary>
    /// <param name="count">��Ѹ� ���� ����</param>
    /// <param name="center">�߽���</param>
    /// <param name="minDistance">���� ������ �ּҰŸ�</param>
    /// <param name="width">������ �ѷ��� ������ ������</param>
    /// <returns>��ѷ��� ���� ��ġ</returns>
    public static Vector2[] SpreadLocation(int count, Vector2 center, float minDistance, float width) {
        //�⺻ ��ġ ���� (����)
        Vector2[] positions = new Vector2[count];
        for(int index = 0; index < positions.Length; index++) {
            positions[index] = Randomize(center, width);
        }

        const int MAX_TRY_COUNT = 1000; //�ִ� �õ�Ƚ��

        for(int tryCount = 0; tryCount < MAX_TRY_COUNT; tryCount++) {
            bool success = true;    //�������� �÷��� �ʱ�ȭ

            //�� �÷��̾ ���� ����
            for(int index = 0; index < positions.Length; index++) {
                //�ʱ�ȭ
                Vector2 position = positions[index];
                int occupiedCount = 0;
                Vector2 avoidForce = new Vector2();

                for(int checkIndex = 0; checkIndex < positions.Length; checkIndex++) {
                    if(checkIndex == index) {
                        //���� ���� �����ϴٸ� ��ŵ
                        continue;
                    }

                    //�� ������ �Ÿ� ���ϱ�
                    Vector2 checkPosition = positions[checkIndex];
                    float distance = Vector2.Distance(position, checkPosition);

                    //���� �� ������ �Ÿ��� �ּҰŸ����� �۴ٸ�
                    if(distance < minDistance) {
                        //ȸ�� ���� ����
                        occupiedCount += 1;
                        avoidForce += checkPosition - position;
                    }
                }

                //��ġ�� ���� 1�� �̻� �־��ٸ�
                if(occupiedCount > 0) {
                    avoidForce /= occupiedCount;
                    float length = avoidForce.magnitude;

                    if(length > 0f) {   //���̰� �ִٸ�
                        //�ݴ� �������� �̵�
                        position -= avoidForce.normalized;
                    } else {    //���̰� ���ٸ�
                        //�ٽ� �����ϰ� ����
                        position = Randomize(center, width);
                    }

                    //���� ����
                    success = false;
                }

                //�ش� ���� ������ ����ٸ�
                if(!IsIn(position, center, width)) {
                    //�ٽ� �����ϰ� ����
                    position = Randomize(center, width);

                    //���� ����
                    success = false;
                }

                positions[index] = position;
            }

            //������ Ż��
            if(success) {
                break;
            }
        }

        return positions;
    }

    /// <summary>
    /// �Էµ� ���͸� ������ �������� �̵���ŵ�ϴ� <br></br>
    /// �Էµ� ���ͷκ��� ���簢�� ������ �����մϴ�
    /// </summary>
    /// <param name="input">�̵���ų ����</param>
    /// <param name="half">�ִ� �̵��� �Ÿ� (������)</param>
    /// <returns>�̵��� ����</returns>
    public static Vector2 Randomize(Vector2 input, float half) {
        if(half == 0) {
            return input;
        }

        return input + new Vector2(Random.instance.NextFloat() * half * 2 - half,
           Random.instance.NextFloat() * half * 2 - half);
    }

    /// <summary>
    /// �Էµ� ���Ͱ� �ش� ���� �ȿ� �ִ��� �Ǵ��մϴ� <br></br>
    /// ���������κ��� ���簢�� ������ �����մϴ�
    /// </summary>
    /// <param name="input">Ȯ���� ����</param>
    /// <param name="center">������</param>
    /// <param name="half">������</param>
    /// <returns></returns>
    public static bool IsIn(Vector2 input, Vector2 center, float half) {
        return input.x >= center.x - half && input.x >= center.x - half
            && input.x >= center.x - half && input.x >= center.x - half;
    }

    /// <summary>
    /// ����ġ�� ���� ����Ʈ���� ������ ��Ҹ� �����մϴ�
    /// </summary>
    /// <typeparam name="T">������ ����� Ÿ��</typeparam>
    /// <param name="objects">������ ��ҵ�</param>
    /// <param name="weightFuction">��ҿ��� ����ġ�� ��ȯ�ϴ� �Լ�</param>
    /// <returns>���õ� ���</returns>
    public static T GetByWeigth<T>(List<T> objects, Func<T, int> weightFuction) {
        List<int> weights = new List<int>();
        int weight;
        int maxWeight = 0;
        for(int index = 0; index < objects.Count; index++) {
            weight = weightFuction(objects[index]);
            weights.Add(weight);
            maxWeight += weight;
        }
        int ran = Random.instance.RandInt(0, maxWeight);
        for(int index = weights.Count - 1; index >= 0; index--) {
            if(ran < weights[index]) {
                return objects[index];
            }
        }
        return objects[0];
    }
}
