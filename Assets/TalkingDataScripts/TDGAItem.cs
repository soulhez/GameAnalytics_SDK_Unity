using UnityEngine;
#if UNITY_IPHONE
using System.Runtime.InteropServices;
#endif

public static class TDGAItem
{
#if UNITY_ANDROID
    private static AndroidJavaClass agent;
#endif
#if UNITY_IPHONE
    [DllImport("__Internal")]
    private static extern void tdgaOnPurchase(string item, int itemNumber, double priceInVirtualCurrency);

    [DllImport("__Internal")]
    private static extern void tdgaOnUse(string item, int itemNumber);
#endif

#if UNITY_ANDROID
    private static AndroidJavaClass GetAgent()
    {
        if (agent == null)
        {
            agent = new AndroidJavaClass("com.tendcloud.tenddata.TDGAItem");
        }
        return agent;
    }
#endif

    public static void OnPurchase(string item, int itemNumber, double priceInVirtualCurrency)
    {
        if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
        {
#if UNITY_ANDROID
            GetAgent().CallStatic("onPurchase", item, itemNumber, priceInVirtualCurrency);
#endif
#if UNITY_IPHONE
            tdgaOnPurchase(item, itemNumber, priceInVirtualCurrency);
#endif
        }
    }

    public static void OnUse(string item, int itemNumber)
    {
        if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
        {
#if UNITY_ANDROID
            GetAgent().CallStatic("onUse", item, itemNumber);
#endif
#if UNITY_IPHONE
            tdgaOnUse(item, itemNumber);
#endif
        }
    }
}
