using System;
using UnityEngine;

[Serializable]
public class AdIds : MonoBehaviour
{
    public Admob admob;
    public UnityAds unity;
    public enum AdPosition { Top, Bottom };
    public AdPosition bannerOn;
    public static AdIds instance;
    public bool isTesting = false;
    private void Awake()
    {
        AdIds.instance = this;
        if (isTesting)
        {

#if UNITY_IOS
            admob.iosBanner = "ca-app-pub-3940256099942544/6300978111";
            admob.iosInterstitial = "ca-app-pub-3940256099942544/1033173712";
            admob.iosRewarded = "ca-app-pub-3940256099942544/5224354917";
#elif UNITY_ANDROID
            admob.androidBanner = "ca-app-pub-3940256099942544/6300978111";
            admob.androidInterstitial = "ca-app-pub-3940256099942544/1033173712";
            admob.androidRewarded = "ca-app-pub-3940256099942544/5224354917";
#endif
        }
    }
}
