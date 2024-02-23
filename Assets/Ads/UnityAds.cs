using System;
using UnityEngine;

[Serializable]
public class UnityAds
{
    [Header("Android GameId")]
    public string androidGameId;
    public string android_interstlId;
    public string android_rewardId;

    [Header("IOS GameId")]
    public string iosGameId;
    public string ios_interstlId;
    public string ios_rewardId;

    public string get_interstitial()
    {
#if UNITY_ANDROID
        return android_interstlId;
#elif UNITY_IOS
        return ios_interstlId;

#endif

    }
    public string get_rewarded()
    {
#if UNITY_ANDROID
        return android_rewardId;
#elif UNITY_IOS
        return ios_rewardId;

#endif

    }
    public string get_game_id()
    {
#if UNITY_ANDROID
        return androidGameId;
#elif UNITY_IOS
        return iosGameId;

#endif

    }
}
