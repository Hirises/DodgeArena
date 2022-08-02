using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Timer
{
    private float _time;
    public float time
    {
        get => _time;
    }

    public Timer()
    {
        Reset();
    }

    public void Reset()
    {
        this._time = 0;
    }

    public IEnumerable Start()
    {
        yield return null;
        Tick();
    }

    public void Tick()
    {
        this._time += Time.deltaTime;
    }

    public bool Check(float value)
    {
        return time >= value;
    }
}
