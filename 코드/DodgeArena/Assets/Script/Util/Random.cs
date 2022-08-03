using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Random 관련 유틸 메소드들을 제공 <br/>
/// 유니티 기본 제공 Random 클래스가 부실해서 새로 제작함
/// </summary>
public class Random
{
    public static readonly Random instance = new Random();
    private System.Random random;

    public Random()
    {
        this.random = new System.Random();
    }

    public Random(int seed)
    {
        this.random = new System.Random(seed);
    }

    public void Seed(int seed)
    {
        this.random = new System.Random(seed);
    }

    /// <summary>
    /// [0-1) 사이의 float 값을 반환합니다
    /// </summary>
    /// <returns>랜덤값</returns>
    public float NextFloat()
    {
        return Convert.ToSingle(random.NextDouble());
    }

    /// <summary>
    /// [0-1) 사이의 double 값을 반환합니다
    /// </summary>
    /// <returns>랜덤값</returns>
    public double NextDouble()
    {
        return random.NextDouble();
    }

    /// <summary>
    /// 랜덤한 true, false 값을 반환합니다
    /// </summary>
    /// <returns>랜덤값</returns>
    public bool NextBool()
    {
        return RandomRange(0, 1) == 0;
    }

    /// <summary>
    /// 랜덤한 부호값을 반환합니다 <br/>
    /// </summary>
    /// <returns>1 또는 -1</returns>
    public int NextSign()
    {
        return NextBool() ? -1 : 1;
    }

    /// <summary>
    /// [min, max] 사이의 int 값을 반환합니다 <br/>
    /// 자동으로 min값과 max값의 대소를 설정합니다
    /// </summary>
    /// <param name="min">최소값</param>
    /// <param name="max">최대값</param>
    /// <returns>랜덤값</returns>
    public int RandomRange(int min, int max)
    {
        if(min > max)
        {
            (min, max) = (max, min);
        }else if(min == max)
        {
            return min;
        }

        int range = max - min + 1;
        float ran = NextFloat() * range;
        return Mathf.FloorToInt(ran) + min;
    }

    /// <summary>
    /// [from, to) 사이의 int 값을 반환합니다
    /// </summary>
    /// <param name="from">시작점</param>
    /// <param name="to">끝점</param>
    /// <returns>랜덤값</returns>
    public int RandInt(int from, int to)
    {
        int range = to - from;
        float ran = NextFloat() * range;
        return Mathf.FloorToInt(ran) + from;
    }

    /// <summary>
    /// [from, to) 사이의 float 값을 반환합니다
    /// </summary>
    /// <param name="from">시작점</param>
    /// <param name="to">끝점</param>
    /// <returns>랜덤값</returns>
    public float RandFloat(float from, float to)
    {
        float range = to - from;
        float ran = NextFloat() * range;
        return ran + from;
    }

    /// <summary>
    /// [from, to) 사이의 double 값을 반환합니다
    /// </summary>
    /// <param name="from">시작점</param>
    /// <param name="to">끝점</param>
    /// <returns>랜덤값</returns>
    public double RandDouble(double from, double to)
    {
        double range = to - from;
        double ran = NextDouble() * range;
        return ran + from;
    }

    /// <summary>
    /// [0, 1]사이의 확률값을 받아서 검증합니다
    /// </summary>
    /// <param name="rate">확률</param>
    /// <returns>성공 여부</returns>
    public bool CheckRate(double rate)
    {
        return NextDouble() < rate;
    }
}