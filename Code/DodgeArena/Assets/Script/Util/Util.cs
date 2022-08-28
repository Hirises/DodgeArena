using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Util {
    public delegate void Runnable();

    /// <summary>
    /// 해당 리스트를 Debug.Log로 출력합니다
    /// </summary>
    /// <typeparam name="T">리스트 타입</typeparam>
    /// <param name="list">출력할 리스트</param>
    public static void Logging<T>(List<T> list) {
        string log = "";
        foreach(T item in list) {
            log += item.ToString() + ", ";
        }
        if(log.Length > 0) {
            log = log.Substring(0, log.Length - 2);
        }
        Debug.Log(log);
    }

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
        Vector2 dir = ToVector(selfPos, targetPos);
        dir.Normalize();
        float angle = Vector2.SignedAngle(zeroRotation, dir);
        return new Vector3(0, 0, angle);
    }

    /// <summary>
    /// 입력된 대상을 바라보는 벡터를 반환합니다 <br/>
    /// 2D 기준으로 계산합니다
    /// </summary>
    /// <param name="selfPos">자신의 위치</param>
    /// <param name="targetPos">바라볼 대상의 위치</param>
    /// <returns>대상을 바라보는 백터</returns>
    public static Vector2 ToVector(Vector3 selfPos, Vector3 targetPos)
    {
        return new Vector2(targetPos.x - selfPos.x, targetPos.y - selfPos.y);
    }

    /// <summary>
    /// 랜덤하게 점들을 흩뿌립니다 <br></br>
    /// 입력된 중심점을 기준으로하는 정사각형 모양의 공간을 가정합니다 <br></br>
    /// 몇몇 경우에 완벽하게 흩뿌려지지 않을 수 있습니다
    /// </summary>
    /// <param name="count">흩뿌릴 점의 개수</param>
    /// <param name="center">중심점</param>
    /// <param name="minDistance">점들 사이의 최소거리</param>
    /// <param name="width">점들이 뿌려질 공간의 반지름</param>
    /// <returns>흩뿌려진 점의 위치</returns>
    public static List<Vector2> SpreadLocation(int count, Vector2 center, float minDistance, float width) {
        return SpreadLocation(count, center, minDistance, width, new List<Vector2>());
    }

    /// <summary>
    /// 랜덤하게 점들을 흩뿌립니다 <br></br>
    /// 입력된 중심점을 기준으로하는 정사각형 모양의 공간을 가정합니다 <br></br>
    /// 몇몇 경우에 완벽하게 흩뿌려지지 않을 수 있습니다
    /// </summary>
    /// <param name="preset">흩뿌릴 점</param>
    /// <param name="center">중심점</param>
    /// <param name="minDistance">점들 사이의 최소거리</param>
    /// <param name="width">점들이 뿌려질 공간의 반지름</param>
    /// <returns>흩뿌려진 점의 위치</returns>
    public static List<Vector2> SpreadLocation(List<Vector2> preset, int count, Vector2 center, float minDistance, float width) {
        return SpreadLocation(preset, center, minDistance, width, new List<Vector2>());
    }

    /// <summary>
    /// 랜덤하게 점들을 흩뿌립니다 <br></br>
    /// 입력된 중심점을 기준으로하는 정사각형 모양의 공간을 가정합니다 <br></br>
    /// 몇몇 경우에 완벽하게 흩뿌려지지 않을 수 있습니다
    /// </summary>
    /// <param name="count">흩뿌릴 점의 개수</param>
    /// <param name="center">중심점</param>
    /// <param name="minDistance">점들 사이의 최소거리</param>
    /// <param name="width">점들이 뿌려질 공간의 반지름</param>
    /// <param name="obstacles">이미 고정된 점들(움직이지는 않으나 고려대상임)</param>
    /// <returns>흩뿌려진 점의 위치</returns>
    public static List<Vector2> SpreadLocation(int count, Vector2 center, float minDistance, float width, List<Vector2> obstacles) {
        //기본 위치 설정 (랜덤)
        List<Vector2> positions = new List<Vector2>();
        for(int index = 0; index < count; index++) {
            positions.Add(Randomize(center, width));
        }

        return SpreadLocation(positions, center, minDistance, width, obstacles);
    }

    /// <summary>
    /// 랜덤하게 점들을 흩뿌립니다 <br></br>
    /// 입력된 중심점을 기준으로하는 정사각형 모양의 공간을 가정합니다 <br></br>
    /// 몇몇 경우에 완벽하게 흩뿌려지지 않을 수 있습니다
    /// </summary>
    /// <param name="preset">흩뿌릴 점</param>
    /// <param name="center">중심점</param>
    /// <param name="minDistance">점들 사이의 최소거리</param>
    /// <param name="width">점들이 뿌려질 공간의 반지름</param>
    /// <param name="obstacles">이미 고정된 점들(움직이지는 않으나 고려대상임)</param>
    /// <returns>흩뿌려진 점의 위치</returns>
    public static List<Vector2> SpreadLocation(List<Vector2> preset, Vector2 center, float minDistance, float width, List<Vector2> obstacles) {

        const int MAX_TRY_COUNT = 1000; //최대 시도횟수

        for(int tryCount = 0; tryCount < MAX_TRY_COUNT; tryCount++) {
            bool success = true;    //성공여부 플레그 초기화

            //각 플레이어에 대해 검증
            for(int index = 0; index < preset.Count; index++) {
                //초기화
                Vector2 position = preset[index];
                int occupiedCount = 0;
                Vector2 avoidForce = new Vector2();

                for(int checkIndex = 0; checkIndex < preset.Count + obstacles.Count; checkIndex++) {
                    if(checkIndex == index) {
                        //검증 대상과 동일하다면 스킵
                        continue;
                    }

                    //둘 사이의 거리 구하기
                    Vector2 checkPosition;
                    if(checkIndex < preset.Count) {
                        checkPosition = preset[checkIndex];
                    } else {
                        checkPosition = obstacles[checkIndex - preset.Count];
                    }
                    float distance = Vector2.Distance(position, checkPosition);

                    //만약 둘 사이의 거리가 최소거리보다 작다면
                    if(distance < minDistance) {
                        //회피 방향 적용
                        occupiedCount += 1;
                        avoidForce += checkPosition - position;
                    }
                }

                //겹치는 점이 1개 이상 있었다면
                if(occupiedCount > 0) {
                    avoidForce /= occupiedCount;
                    float length = avoidForce.magnitude;

                    if(length > 0f) {   //길이가 있다면
                        //반대 방향으로 이동
                        position -= avoidForce.normalized;
                    } else {    //길이가 없다면
                        //다시 랜덤하게 설정
                        position = Randomize(center, width);
                    }

                    //실패 판정
                    success = false;
                }

                //해당 점이 공간을 벗어났다면
                if(!IsIn(position, center, width)) {
                    //다시 랜덤하게 설정
                    position = Randomize(center, width);

                    //실패 판정
                    success = false;
                }

                preset[index] = position;
            }

            //성공시 탈출
            if(success) {
                break;
            }
        }

        return preset;
    }

    /// <summary>
    /// 입력된 벡터를 랜덤한 방향으로 이동시킵니다 <br></br>
    /// 입력된 벡터로부터 정사각형 공간을 가정합니다
    /// </summary>
    /// <param name="input">이동시킬 벡터</param>
    /// <param name="half">최대 이동할 거리 (반지름)</param>
    /// <returns>이동된 벡터</returns>
    public static Vector2 Randomize(Vector2 input, float half) {
        if(half == 0) {
            return input;
        }

        return input + new Vector2(Random.instance.NextFloat() * half * 2 - half,
           Random.instance.NextFloat() * half * 2 - half);
    }

    /// <summary>
    /// 입력된 벡터가 해당 공간 안에 있는지 판단합니다 <br></br>
    /// 기준점으로부터 정사각형 공간을 가정합니다
    /// </summary>
    /// <param name="input">확인할 벡터</param>
    /// <param name="center">기준점</param>
    /// <param name="half">반지름</param>
    /// <returns></returns>
    public static bool IsIn(Vector2 input, Vector2 center, float half) {
        return input.x >= center.x - half && input.x >= center.x - half
            && input.x >= center.x - half && input.x >= center.x - half;
    }

    /// <summary>
    /// 가중치에 따라 리스트에서 랜덤한 요소를 선택합니다
    /// </summary>
    /// <typeparam name="T">선택할 요소의 타입</typeparam>
    /// <param name="objects">선택할 요소들</param>
    /// <param name="weightFuction">요소에서 가중치를 반환하는 함수</param>
    /// <returns>선택된 요소</returns>
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

    /// <summary>
    /// 가중치에 따라 리스트에서 랜덤한 요소를 선택합니다
    /// </summary>
    /// <typeparam name="T">선택할 요소의 타입</typeparam>
    /// <param name="objects">선택할 요소들</param>
    /// <returns>선택된 요소</returns>
    public static T GetByWeigth<T>(List<T> objects) where T : IHasWeight {
        List<int> weights = new List<int>();
        int weight;
        int maxWeight = 0;
        for(int index = 0; index < objects.Count; index++) {
            weight = objects[index].GetWeight();
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

    /// <summary>
    /// 청크를 대상으로 하는 거리 연산입니다
    /// x, y 거리중 더 짧은 쪽을 반환합니다
    /// </summary>
    /// <param name="vec1">지점 1</param>
    /// <param name="vec2">지점 2</param>
    /// <returns>거리</returns>
    public static int DistanceSquare(Vector2 vec1, Vector2 vec2) {
        return Mathf.Max(0, Mathf.Max(Mathf.Abs(Mathf.RoundToInt(vec1.x) - Mathf.RoundToInt(vec2.x)), Mathf.Abs(Mathf.RoundToInt(vec1.y) - Mathf.RoundToInt(vec2.y))) - 1);
    }

    /// <summary>
    /// 입력된 함수를 해당 시간 이후에 실행해줍니다
    /// </summary>
    /// <param name="time">지연 시간</param>
    /// <param name="run">대상 함수</param>
    public static void RunLater(YieldInstruction time, Runnable run) {
        GameManager.instance.StartCoroutine(Runner(time, run).GetEnumerator());
    }

    private static IEnumerable Runner(YieldInstruction time, Runnable run) {
        yield return time;
        run();
    }

    /// <summary>
    /// 두 리스트의 내용이 동일한지 확인합니다
    /// </summary>
    /// <param name="list1">확인할 리스트1</param>
    /// <param name="list2">확인할 리스트2</param>
    /// <returns>결과</returns>
    public static bool DeepEquals<A, B>(List<A> list1, List<B> list2) {
        if(list1 == null ^ list2 == null) {
            return false;
        }
        if(list1 == null) {
            return true;
        }
        if(list1.Capacity != list2.Capacity) {
            return false;
        }
        for(int index = 0; index < list1.Capacity; index++) {
            if(!list1[index].Equals(list2[index])) {
                return false;
            }
        }
        return true;
    }
}
