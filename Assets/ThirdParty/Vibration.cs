// Taken from aVolpe's Vibration for Unity3d with Android native Call, with fallback to Handlheld.Vibrate() 
// https://gist.github.com/aVolpe/707c8cf46b1bb8dfb363
//
// License is unknown, will need to replace with custom solution

using UnityEngine;
using System.Collections;

public static class Vibration
{

#if UNITY_ANDROID && !UNITY_EDITOR
    public static AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
    public static AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
    public static AndroidJavaObject vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
#else
    public static AndroidJavaClass unityPlayer;
    public static AndroidJavaObject currentActivity;
    public static AndroidJavaObject vibrator;
#endif

    public static void Vibrate()
    {
        if (isAndroid())
            vibrator.Call("vibrate");
        else
            Handheld.Vibrate();
    }


    public static void Vibrate(long milliseconds)
    {
        if (isAndroid())
            vibrator.Call("vibrate", milliseconds);
        else
        {
#if !UNITY_EDITOR
            Handheld.Vibrate();
#endif
        }
    }

    public static void Vibrate(long[] pattern, int repeat)
    {
        if (isAndroid())
        {
            vibrator.Call("vibrate", pattern, repeat);
        }
        else
        {
#if !UNITY_EDITOR
            Handheld.Vibrate();
#endif
        }
    }

    public static bool HasVibrator()
    {
        return vibrator.Call<bool>("hasVibrator");
    }

    public static void Cancel()
    {
        if (isAndroid())
            vibrator.Call("cancel");
    }

    private static bool isAndroid()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
	return true;
#else
        return false;
#endif
    }
}
