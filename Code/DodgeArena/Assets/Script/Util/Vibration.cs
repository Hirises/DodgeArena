using System.Collections;
using UnityEngine;

/// <summary>
/// 안드로이드 진동
/// </summary>
public static class Vibration {

    #if UNITY_ANDROID && !UNITY_EDITOR
        public static AndroidJavaClass AndroidPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        public static AndroidJavaObject AndroidcurrentActivity = AndroidPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        public static AndroidJavaObject AndroidVibrator = AndroidcurrentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
    #endif

    /// <summary>
    /// 1초동안 길게 진동
    /// </summary>
    public static void VibrateLong() {
        #if UNITY_ANDROID && !UNITY_EDITOR
            AndroidVibrator.Call("vibrate");
        #else
            //Handheld.Vibrate();
        #endif
    }

    /// <summary>
    /// 0.05초동안 짧게 진동
    /// </summary>
    public static void VibrateShort() {
        Vibrate(50);
    }

    /// <summary>
    /// 해당 밀리초 동안 진동
    /// </summary>
    /// <param name="milliseconds">밀리초</param>
    public static void Vibrate(long milliseconds) {
        #if UNITY_ANDROID && !UNITY_EDITOR
            AndroidVibrator.Call("vibrate", milliseconds);
        #else
            //Handheld.Vibrate();
        #endif
    }

    /// <summary>
    /// 해당 패턴을 반복
    /// [2n]초 쉬고, [2n+1]초 울리고 반복 (n은 0포함 자연수)
    /// </summary>
    /// <param name="pattern">울릴 패턴</param>
    /// <param name="repeat">반복횟수
    /// -1이면 반복하지 않음</param>
    public static void Vibrate(long[] pattern, int repeat) {
        #if UNITY_ANDROID && !UNITY_EDITOR
            AndroidVibrator.Call("vibrate", pattern, repeat);
        #else
            //Handheld.Vibrate();
        #endif
    }

    /// <summary>
    /// 현재 진동을 취소
    /// </summary>
    public static void Cancel() {
        #if UNITY_ANDROID && !UNITY_EDITOR
            AndroidVibrator.Call("cancel");
        #endif
    }

}