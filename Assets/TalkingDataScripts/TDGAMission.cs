using UnityEngine;
#if UNITY_IPHONE
using System.Runtime.InteropServices;
#endif

public static class TDGAMission
{
#if UNITY_ANDROID
    private static AndroidJavaClass agent;
#endif
#if UNITY_IPHONE
    [DllImport("__Internal")]
    private static extern void tdgaOnBegin(string missionId);

    [DllImport("__Internal")]
    private static extern void tdgaOnCompleted(string missionId);

    [DllImport("__Internal")]
    private static extern void tdgaOnFailed(string missionId, string failedCause);
#endif

#if UNITY_ANDROID
    private static AndroidJavaClass GetAgent()
    {
        if (agent == null)
        {
            agent = new AndroidJavaClass("com.tendcloud.tenddata.TDGAMission");
        }
        return agent;
    }
#endif

    public static void OnBegin(string missionId)
    {
        if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
        {
#if UNITY_ANDROID
            GetAgent().CallStatic("onBegin", missionId);
#endif
#if UNITY_IPHONE
            tdgaOnBegin(missionId);
#endif
        }
    }

    public static void OnCompleted(string missionId)
    {
        if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
        {
#if UNITY_ANDROID
            GetAgent().CallStatic("onCompleted", missionId);
#endif
#if UNITY_IPHONE
            tdgaOnCompleted(missionId);
#endif
        }
    }

    public static void OnFailed(string missionId, string failedCause)
    {
        if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
        {
#if UNITY_ANDROID
            GetAgent().CallStatic("onFailed", missionId, failedCause);
#endif
#if UNITY_IPHONE
            tdgaOnFailed(missionId, failedCause);
#endif
        }
    }
}
