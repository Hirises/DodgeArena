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
        DependedUp,
        DependedDown,
        IndependedUp,
        IndependedDown
    }
    public Count type = Count.DependedUp;
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
        switch(type) {
            case Count.DependedUp:
                this._time += Time.deltaTime;
                break;
            case Count.DependedDown:
                this._time -= Time.deltaTime;
                break;
            case Count.IndependedUp:
                this._time += Time.unscaledDeltaTime;
                break;
            case Count.IndependedDown:
                this._time -= Time.unscaledDeltaTime;
                break;
        }
    }

    public bool Check() {
        switch(type) {
            case Count.DependedUp:
                return this._time >= target;
            case Count.DependedDown:
                return this._time <= target;
            case Count.IndependedUp:
                return this._time >= target;
            case Count.IndependedDown:
                return this._time <= target;
        }
        return true;
    }
}
