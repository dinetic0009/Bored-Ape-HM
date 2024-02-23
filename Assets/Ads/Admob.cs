using System;
using UnityEngine;

[Serializable]
public class Admob
{
    [Header("Android")]
    public string androidBanner;
    public string androidInterstitial;
    public string androidRewarded;

    [Header("IOS")]
    public string iosBanner;
    public string iosInterstitial;
    public string iosRewarded;

}
