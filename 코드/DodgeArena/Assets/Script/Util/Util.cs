using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        Vector2 dir = new Vector2(targetPos.x - selfPos.x, targetPos.y - selfPos.y);
        dir.Normalize();
        float angle = Vector2.SignedAngle(zeroRotation, dir);
        return new Vector3(0, 0, angle);
    }
}
