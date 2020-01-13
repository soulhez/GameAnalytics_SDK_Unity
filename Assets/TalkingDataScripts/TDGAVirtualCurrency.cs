using UnityEngine;
#if UNITY_IPHONE
using System.Runtime.InteropServices;
#endif

public static class TDGAVirtualCurrency
{
#if UNITY_ANDROID
    private static AndroidJavaClass agent;
#endif
#if UNITY_IPHONE
    [DllImport("__Internal")]
    private static extern void tdgaOnChargeRequst(string orderId, string iapId, double currencyAmount, string currencyType, double virtualCurrencyAmount, string paymentType);

    [DllImport("__Internal")]
    private static extern void tdgaOnChargSuccess(string orderId);

    [DllImport("__Internal")]
    private static extern void tdgaOnReward(double virtualCurrencyAmount, string reason);
#endif

#if UNITY_ANDROID
    private static AndroidJavaClass GetAgent()
    {
        if (agent == null)
        {
            agent = new AndroidJavaClass("com.tendcloud.tenddata.TDGAVirtualCurrency");
        }
        return agent;
    }
#endif

    public static void OnChargeRequest(string orderId, string iapId, double currencyAmount, string currencyType, double virtualCurrencyAmount, string paymentType)
    {
        if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
        {
#if UNITY_ANDROID
            GetAgent().CallStatic("onChargeRequest", orderId, iapId, currencyAmount, currencyType, virtualCurrencyAmount, paymentType);
#endif
#if UNITY_IPHONE
            tdgaOnChargeRequst(orderId, iapId, currencyAmount, currencyType, virtualCurrencyAmount, paymentType);
#endif
        }
    }

    public static void OnChargeSuccess(string orderId)
    {
        if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
        {
#if UNITY_ANDROID
            GetAgent().CallStatic("onChargeSuccess", orderId);
#endif
#if UNITY_IPHONE
            tdgaOnChargSuccess(orderId);
#endif
        }
    }

    public static void OnReward(double virtualCurrencyAmount, string reason)
    {
        if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
        {
#if UNITY_ANDROID
            GetAgent().CallStatic("onReward", virtualCurrencyAmount, reason);
#endif
#if UNITY_IPHONE
            tdgaOnReward(virtualCurrencyAmount, reason);
#endif
        }
    }
}
