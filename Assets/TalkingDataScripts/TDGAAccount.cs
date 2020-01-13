using UnityEngine;
#if UNITY_IPHONE
using System.Runtime.InteropServices;
#endif

public enum Gender
{
    UNKNOW = 0,
    MALE = 1,
    FEMALE = 2
}

public enum AccountType
{
    ANONYMOUS = 0,
    REGISTERED = 1,
    SINA_WEIBO = 2,
    QQ = 3,
    QQ_WEIBO = 4,
    ND91 = 5,
    WEIXIN = 6,
    TYPE1 = 11,
    TYPE2 = 12,
    TYPE3 = 13,
    TYPE4 = 14,
    TYPE5 = 15,
    TYPE6 = 16,
    TYPE7 = 17,
    TYPE8 = 18,
    TYPE9 = 19,
    TYPE10 = 20
}

public class TDGAAccount
{
    private static TDGAAccount account;
#if UNITY_ANDROID
    private static AndroidJavaClass agent;
    private AndroidJavaObject mAccount;
#endif
#if UNITY_IPHONE
    [DllImport("__Internal")]
    private static extern void tdgaSetAccount(string accountId);

    [DllImport("__Internal")]
    private static extern void tdgaSetAccountName(string accountName);

    [DllImport("__Internal")]
    private static extern void tdgaSetAccountType(int accountType);

    [DllImport("__Internal")]
    private static extern void tdgaSetLevel(int level);

    [DllImport("__Internal")]
    private static extern void tdgaSetGender(int gender);

    [DllImport("__Internal")]
    private static extern void tdgaSetAge(int age);

    [DllImport("__Internal")]
    private static extern void tdgaSetGameServer(string gameServer);
#endif

    public static TDGAAccount SetAccount(string accountId)
    {
        if (account == null)
        {
            account = new TDGAAccount();
        }
        if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
        {
#if UNITY_ANDROID
            if (agent == null)
            {
                agent = new AndroidJavaClass("com.tendcloud.tenddata.TDGAAccount");
            }
            account.mAccount = agent.CallStatic<AndroidJavaObject>("setAccount", accountId);
#endif
#if UNITY_IPHONE
            tdgaSetAccount(accountId);
#endif
        }

        return account;
    }

    public void SetAccountName(string accountName)
    {
        if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
        {
#if UNITY_ANDROID
            if (mAccount != null)
            {
                mAccount.Call("setAccountName", accountName);
            }
#endif
#if UNITY_IPHONE
            tdgaSetAccountName(accountName);
#endif
        }
    }

    public void SetAccountType(AccountType type)
    {
        if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
        {
#if UNITY_ANDROID
            if (mAccount != null)
            {
                AndroidJavaClass enumClass = new AndroidJavaClass("com.tendcloud.tenddata.TDGAAccount$AccountType");
                AndroidJavaObject obj = enumClass.CallStatic<AndroidJavaObject>("valueOf", type.ToString());
                mAccount.Call("setAccountType", obj);
                enumClass.Dispose();
            }
#endif
#if UNITY_IPHONE
            tdgaSetAccountType((int)type);
#endif
        }
    }

    public void SetLevel(int level)
    {
        if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
        {
#if UNITY_ANDROID
            if (mAccount != null)
            {
                mAccount.Call("setLevel", level);
            }
#endif
#if UNITY_IPHONE
            tdgaSetLevel(level);
#endif
        }
    }

    public void SetAge(int age)
    {
        if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
        {
#if UNITY_ANDROID
            if (mAccount != null)
            {
                mAccount.Call("setAge", age);
            }
#endif
#if UNITY_IPHONE
            tdgaSetAge(age);
#endif
        }
    }

    public void SetGender(Gender type)
    {
        if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
        {
#if UNITY_ANDROID
            if (mAccount != null)
            {
                AndroidJavaClass enumClass = new AndroidJavaClass("com.tendcloud.tenddata.TDGAAccount$Gender");
                AndroidJavaObject obj = enumClass.CallStatic<AndroidJavaObject>("valueOf", type.ToString());
                mAccount.Call("setGender", obj);
                enumClass.Dispose();
            }
#endif
#if UNITY_IPHONE
            tdgaSetGender((int)type);
#endif
        }
    }

    public void SetGameServer(string gameServer)
    {
        if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
        {
#if UNITY_ANDROID
            if (mAccount != null)
            {
                mAccount.Call("setGameServer", gameServer);
            }
#endif
#if UNITY_IPHONE
            tdgaSetGameServer(gameServer);
#endif
        }
    }
}
