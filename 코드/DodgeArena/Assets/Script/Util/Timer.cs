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
    public float target;
    public enum Count {
        Up,
        Down
    }
    public Count count = Count.Up;
    public float rate {
        get {
            return time / target;
        }
    }
    private IEnumerator instance;

    public Timer()
    {
        Reset();
    }

    public Timer(float time) {
        Reset(time);
    }

    public void Reset()
    {
        Reset(0);
    }

    public void Reset(float time) {
        this._time = time;
    }

    public void Start(Action<float> callback, Util.Runnable finish) {
        instance = Run(callback, finish).GetEnumerator();
        GameManager.instance.StartCoroutine(instance);
    }

    public void Stop() {
        if(instance != null) {
            GameManager.instance.StopCoroutine(instance);
            instance = null;
        }
    }

    private IEnumerable Run(Action<float> callback, Util.Runnable finish) {
        while(!Check()) {
            yield return null;
            Tick();
            if(callback != null) {
                callback(time);
            }
        }
        if(finish != null) {
            finish();
        }
    }

    public void Tick() {
        if(count == Count.Up) {
            this._time += Time.deltaTime;
        } else {
            this._time -= Time.deltaTime;
        }
    }

    public bool Check() {
        if(count == Count.Up) {
            return this._time >= target;
        } else {
            return this._time <= target;
        }
    }
}
