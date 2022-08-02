using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util
{
    /// <summary>
    /// 3D기준 백터를 2D 기준으로 변경합니다. <br/>
    /// 백터의 x, y 성분을 이용합니다
    /// </summary>
    /// <param name="vector">변경할 백터</param>
    /// <returns>변경된 백터</returns>
    public static Vector2 FlattenLocation(Vector3 vector)
    {
        return new Vector2(vector.x, vector.y);
    }

    /// <summary>
    /// 입력된 대상을 바라보는 회전값을 오일러 각도로 반환합니다. <br/>
    /// 2D 기준으로 계산하며, Z회전이 0일 때 오른쪽을 바라보고 있다고 가정합니다.
    /// </summary>
    /// <param name="selfPos">자신의 위치</param>
    /// <param name="targetPos">바라볼 대상의 위치</param>
    /// <returns>대상을 바라보는 회전값</returns>
    public static Vector3 LootAtRotation(Vector3 selfPos, Vector3 targetPos)
    {
        return LootAtRotation(selfPos, targetPos, Vector2.right);
    }

    /// <summary>
    /// 입력된 대상을 바라보는 회전값을 오일러 각도로 반환합니다. <br/>
    /// 2D 기준으로 계산합니다.
    /// </summary>
    /// <param name="selfPos">자신의 위치</param>
    /// <param name="targetPos">바라볼 대상의 위치</param>
    /// <param name="zeroRotation">Z회전이 0일때 바라보는 방향</param>
    /// <returns>대상을 바라보는 회전값</returns>
    public static Vector3 LootAtRotation(Vector3 selfPos, Vector3 targetPos, Vector2 zeroRotation)
    {
        Vector2 dir = new Vector2(targetPos.x - selfPos.x, targetPos.y - selfPos.y);
        dir.Normalize();
        float angle = Vector2.SignedAngle(zeroRotation, dir);
        return new Vector3(0, 0, angle);
    }
}
