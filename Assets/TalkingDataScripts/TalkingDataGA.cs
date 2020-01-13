using UnityEngine;
using System.Collections.Generic;
#if UNITY_ANDROID
using System;
#endif
#if UNITY_IPHONE
using System.Runtime.InteropServices;
using System.Collections;
#endif

public static class TalkingDataGA
{
#if UNITY_ANDROID
    private static AndroidJavaClass agent;
    private static AndroidJavaClass unityClass;
#endif
#if UNITY_IPHONE
    [DllImport("__Internal")]
    private static extern string tdgaGetDeviceId();

    [DllImport("__Internal")]
    private static extern void tdgaSetVerboseLogDisabled();

    [DllImport("__Internal")]
    private static extern void tdgaOnStart(string appId, string channelId);

#if TDGA_CUSTOM
    [DllImport("__Internal")]
    private static extern void tdgaOnEvent(string eventId, string parameters);
#endif

	[DllImport("__Internal")]
	private static extern void tdgaSetLocation(double latitude, double longitude);

#if TDGA_PUSH
    [DllImport("__Internal")]
    private static extern void tdgaSetDeviceToken(byte[] deviceToke, int length);

    [DllImport("__Internal")]
    private static extern void tdgaHandlePushMessage(string message);

    private static bool hasTokenBeenObtained;
#endif
#endif

#if UNITY_ANDROID
    private static AndroidJavaClass GetAgent()
    {
        if (agent == null)
        {
            agent = new AndroidJavaClass("com.tendcloud.tenddata.TalkingDataGA");
        }
        return agent;
    }
#endif

    public static string GetDeviceId()
    {
        string deviceId = null;
        if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
        {
#if UNITY_ANDROID
            AndroidJavaObject activity = unityClass.GetStatic<AndroidJavaObject>("currentActivity");
            deviceId = GetAgent().CallStatic<string>("getDeviceId", activity);
#endif
#if UNITY_IPHONE
            deviceId = tdgaGetDeviceId();
#endif
        }
        return deviceId;
    }

    public static void SetVerboseLogDisabled()
    {
        if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
        {
#if UNITY_ANDROID
            GetAgent().CallStatic("setVerboseLogDisabled");
#endif
#if UNITY_IPHONE
            tdgaSetVerboseLogDisabled();
#endif
        }
    }

    public static void OnStart(string appId, string channelId)
    {
        if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
        {
#if UNITY_ANDROID
            using (AndroidJavaClass dz = new AndroidJavaClass("com.tendcloud.tenddata.game.dz"))
            {
                dz.SetStatic("a", 2);
            }
            unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject activity = unityClass.GetStatic<AndroidJavaObject>("currentActivity");
            GetAgent().CallStatic("init", activity, appId, channelId);
            GetAgent().CallStatic("onResume", activity);
#endif
#if UNITY_IPHONE
            tdgaOnStart(appId, channelId);
#endif
        }
    }

#if TDGA_CUSTOM
    public static void OnEvent(string actionId, Dictionary<string, object> parameters)
    {
        if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
        {
#if UNITY_ANDROID
            if (parameters != null && parameters.Count > 0)
            {
                int count = parameters.Count;
                AndroidJavaObject map = new AndroidJavaObject("java.util.HashMap", count);
                IntPtr method_Put = AndroidJNIHelper.GetMethodID(map.GetRawClass(), "put", "(Ljava/lang/Object;Ljava/lang/Object;)Ljava/lang/Object;");
                object[] args = new object[2];
                foreach (KeyValuePair<string, object> kvp in parameters)
                {
                    args[0] = new AndroidJavaObject("java.lang.String", kvp.Key);
                    args[1] = typeof(string).IsInstanceOfType(kvp.Value)
                        ? new AndroidJavaObject("java.lang.String", kvp.Value)
                        : new AndroidJavaObject("java.lang.Double", "" + kvp.Value);
                    AndroidJNI.CallObjectMethod(map.GetRawObject(), method_Put, AndroidJNIHelper.CreateJNIArgArray(args));
                }

                if (agent != null)
                {
                    agent.CallStatic("onEvent", actionId, map);
                }
                map.Dispose();
            }
            else
            {
                if (agent != null)
                {
                    agent.CallStatic("onEvent", actionId, null);
                }
            }
#endif
#if UNITY_IPHONE
			if (parameters != null && parameters.Count > 0)
			{
				string parameterStr = "{";
				foreach (KeyValuePair<string, object> kvp in parameters)
				{
					if (kvp.Value is string)
					{
						parameterStr += "\"" + kvp.Key + "\":\"" + kvp.Value + "\",";
					}
					else
					{
						try
						{
							double tmp = System.Convert.ToDouble(kvp.Value);
							parameterStr += "\"" + kvp.Key + "\":" + tmp + ",";
						}
						catch (System.Exception)
						{
						}
					}
				}
				parameterStr = parameterStr.TrimEnd(',');
				parameterStr += "}";
				tdgaOnEvent(actionId, parameterStr);
			}
			else
			{
				tdgaOnEvent(actionId, null);
			}
#endif
        }
    }
#endif

    public static void OnEnd()
    {
#if UNITY_ANDROID
        if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
        {
            if (agent != null)
            {
                AndroidJavaObject activity = unityClass.GetStatic<AndroidJavaObject>("currentActivity");
                agent.CallStatic("onPause", activity);
                agent = null;
                unityClass = null;
            }
        }
#endif
    }

    public static void OnKill()
    {
#if UNITY_ANDROID
        if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
        {
            if (agent != null)
            {
                AndroidJavaObject activity = unityClass.GetStatic<AndroidJavaObject>("currentActivity");
                agent.CallStatic("onKill", activity);
                agent = null;
                unityClass = null;
            }
        }
#endif
    }

	public static void SetLocation(double latitude, double longitude)
	{
#if UNITY_IPHONE
		if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
		{
			tdgaSetLocation(latitude, longitude);
		}
#endif
	}

#if TDGA_PUSH
    public static void SetDeviceToken()
    {
#if UNITY_IPHONE
        if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
        {
            if (!hasTokenBeenObtained)
            {
                byte[] deviceToken = UnityEngine.iOS.NotificationServices.deviceToken;
                if (deviceToken != null)
                {
                    tdgaSetDeviceToken(deviceToken, deviceToken.Length);
                    hasTokenBeenObtained = true;
                }
            }
        }
#endif
    }

    public static void HandlePushMessage()
    {
#if UNITY_IPHONE
        if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
        {
            UnityEngine.iOS.RemoteNotification[] notifications = UnityEngine.iOS.NotificationServices.remoteNotifications;
            if (notifications != null)
            {
                UnityEngine.iOS.NotificationServices.ClearRemoteNotifications();
                foreach (UnityEngine.iOS.RemoteNotification rn in notifications)
                {
                    foreach (DictionaryEntry de in rn.userInfo)
                    {
                        if (de.Key.ToString().Equals("sign"))
                        {
                            string sign = de.Value.ToString();
                            tdgaHandlePushMessage(sign);
                        }
                    }
                }
            }
        }
#endif
    }
#endif
}
