using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Random ���� ��ƿ �޼ҵ���� ���� <br/>
/// ����Ƽ �⺻ ���� Random Ŭ������ �ν��ؼ� ���� ������
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
    /// [0-1) ������ float ���� ��ȯ�մϴ�
    /// </summary>
    /// <returns>������</returns>
    public float NextFloat()
    {
        return Convert.ToSingle(random.NextDouble());
    }

    /// <summary>
    /// [0-1) ������ double ���� ��ȯ�մϴ�
    /// </summary>
    /// <returns>������</returns>
    public double NextDouble()
    {
        return random.NextDouble();
    }

    /// <summary>
    /// ������ true, false ���� ��ȯ�մϴ�
    /// </summary>
    /// <returns>������</returns>
    public bool NextBool()
    {
        return RandomRange(0, 1) == 0;
    }

    /// <summary>
    /// ������ ��ȣ���� ��ȯ�մϴ� <br/>
    /// </summary>
    /// <returns>1 �Ǵ� -1</returns>
    public int NextSign()
    {
        return NextBool() ? -1 : 1;
    }

    /// <summary>
    /// [min, max] ������ int ���� ��ȯ�մϴ� <br/>
    /// �ڵ����� min���� max���� ��Ҹ� �����մϴ�
    /// </summary>
    /// <param name="min">�ּҰ�</param>
    /// <param name="max">�ִ밪</param>
    /// <returns>������</returns>
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
    /// [from, to) ������ int ���� ��ȯ�մϴ�
    /// </summary>
    /// <param name="from">������</param>
    /// <param name="to">����</param>
    /// <returns>������</returns>
    public int RandInt(int from, int to)
    {
        int range = to - from;
        float ran = NextFloat() * range;
        return Mathf.FloorToInt(ran) + from;
    }

    /// <summary>
    /// [from, to) ������ float ���� ��ȯ�մϴ�
    /// </summary>
    /// <param name="from">������</param>
    /// <param name="to">����</param>
    /// <returns>������</returns>
    public float RandFloat(float from, float to)
    {
        float range = to - from;
        float ran = NextFloat() * range;
        return ran + from;
    }

    /// <summary>
    /// [from, to) ������ double ���� ��ȯ�մϴ�
    /// </summary>
    /// <param name="from">������</param>
    /// <param name="to">����</param>
    /// <returns>������</returns>
    public double RandDouble(double from, double to)
    {
        double range = to - from;
        double ran = NextDouble() * range;
        return ran + from;
    }

    /// <summary>
    /// [0, 1]������ Ȯ������ �޾Ƽ� �����մϴ�
    /// </summary>
    /// <param name="rate">Ȯ��</param>
    /// <returns>���� ����</returns>
    public bool CheckRate(double rate)
    {
        return NextDouble() < rate;
    }
}