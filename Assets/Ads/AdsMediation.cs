using UnityEngine;
using System;
using UnityEngine.Advertisements;
using GoogleMobileAds.Api;
using GameAnalyticsSDK;
#if UNITY_IOS
using Unity.Advertisement.IosSupport;
#endif

public class AdsMediation : MonoBehaviour
{
    public static AdsMediation instance;
    /*========= Admob ========== */
    private BannerView admob_Banner;
    GoogleMobileAds.Api.InterstitialAd admob_InterstitialAd;
    RewardedAd admob_RewardedVideo;

    /*== RewardedVideo Delegate ===*/
    public delegate void videoCompleted(bool status);
    public static event videoCompleted rewardUserNow;
    static bool initilized = false;
    private void Awake()
    {
        AdsMediation.instance = this;
#if UNITY_IOS
        if (ATTrackingStatusBinding.GetAuthorizationTrackingStatus() ==
    ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED)
        {

            ATTrackingStatusBinding.RequestAuthorizationTracking();

        }
#endif
        if (!initilized)
        {
            GameAnalytics.Initialize();
            initilized = true;
        }
    }
    private void Start()
    {
        /*========= Admob ========== */
        this.admob_RequestInterstitial();
        this.admob_InitRewardedVideo();
        this.admob_RequestRewardBasedVideo();

        /*======== Unity ============= */
        Advertisement.Initialize(AdIds.instance.unity.get_game_id().Trim(), false);
        if (AdsConfig.Get_RemoveAds_Status() == 0)
        {
            this.admob_RequestBanner();
        }
    }
    /*========= Admob ========== */
    private void admob_InitRewardedVideo()
    {
#if UNITY_ANDROID
        this.admob_RewardedVideo = new RewardedAd(AdIds.instance.admob.androidRewarded.Trim());
#elif UNITY_IOS
        this.admob_RewardedVideo = new RewardedAd(AdIds.instance.admob.iosRewarded.Trim());
#endif
        this.admob_RewardedVideo.OnAdLoaded += new EventHandler<EventArgs>(this.HandleRewardBasedVideoLoaded);
        this.admob_RewardedVideo.OnAdOpening += new EventHandler<EventArgs>(this.HandleRewardBasedVideoOpened);
        this.admob_RewardedVideo.OnUserEarnedReward += new EventHandler<Reward>(this.HandleRewardBasedVideoRewarded);
        this.admob_RewardedVideo.OnAdClosed += new EventHandler<EventArgs>(this.HandleRewardBasedVideoClosed);
    }

    public void admob_RequestBanner()
    {
        string adUnitId;
#if UNITY_ANDROID
        adUnitId = AdIds.instance.admob.androidBanner.Trim();
#elif UNITY_IOS
        adUnitId = AdIds.instance.admob.iosBanner.Trim();
#endif
        this.admob_Banner = new BannerView(adUnitId, GoogleMobileAds.Api.AdSize.Banner, AdIds.instance.bannerOn == AdIds.AdPosition.Bottom ?  GoogleMobileAds.Api.AdPosition.Bottom : GoogleMobileAds.Api.AdPosition.Top);
        this.admob_Banner.OnAdLoaded += new EventHandler<EventArgs>(this.HandleAdLoaded);
        this.admob_Banner.OnAdFailedToLoad += new EventHandler<AdFailedToLoadEventArgs>(this.HandleAdFailedToLoad);
        this.admob_Banner.OnAdOpening += new EventHandler<EventArgs>(this.HandleAdOpened);
        this.admob_Banner.OnAdClosed += new EventHandler<EventArgs>(this.HandleAdClosed);
        this.admob_Banner.LoadAd(this.CreateAdRequest());
    }

    public void admob_RequestInterstitial()
    {
        string adUnitId;
#if UNITY_ANDROID
        adUnitId = AdIds.instance.admob.androidInterstitial.Trim();

#elif UNITY_IOS
        adUnitId = AdIds.instance.admob.iosInterstitial.Trim();
#endif
        this.admob_InterstitialAd = new GoogleMobileAds.Api.InterstitialAd(adUnitId);
        this.admob_InterstitialAd.OnAdLoaded += new EventHandler<EventArgs>(this.HandleInterstitialLoaded);
        this.admob_InterstitialAd.OnAdFailedToLoad += new EventHandler<AdFailedToLoadEventArgs>(this.HandleInterstitialFailedToLoad);
        this.admob_InterstitialAd.OnAdOpening += new EventHandler<EventArgs>(this.HandleInterstitialOpened);
        this.admob_InterstitialAd.OnAdClosed += new EventHandler<EventArgs>(this.HandleInterstitialClosed);
        this.admob_InterstitialAd.LoadAd(this.CreateAdRequest());
    }

    public void admob_RequestRewardBasedVideo()
    {
        this.admob_RewardedVideo.LoadAd(this.CreateAdRequest());
    }

    private AdRequest CreateAdRequest()
    {
        return new AdRequest.Builder().Build();
    }

    public void admob_ShowBanner()
    {
        if (this.admob_Banner != null && AdsConfig.Get_RemoveAds_Status() == 0)
        {
            this.admob_Banner.Show();
        }
    }

    public void admob_HideBanner()
    {
        if (this.admob_Banner != null)
        {
            Debug.LogError("Hiding");
            this.admob_Banner.Hide();
        }
    }

    bool admob_showInterstitial()
    {
        if (this.admob_InterstitialAd.IsLoaded())
        {
            this.admob_InterstitialAd.Show();
            return true;
        }
        else
        {
            this.admob_RequestInterstitial();
            return false;
        }
    }

    bool admob_ShowRewardedVideo()
    {
        if (this.admob_RewardedVideo.IsLoaded())
        {
            this.admob_RewardedVideo.Show();
            return true;
        }
        else
        {
            MonoBehaviour.print("Reward based video ad is not ready yet");
            admob_RequestRewardBasedVideo();
            return false;
        }
    }

    /*========= Admob Banner Callbacks ========== */
    public void HandleAdLoaded(object sender, EventArgs args)
    {
        admob_ShowBanner();
        MonoBehaviour.print("HandleAdLoaded event received.");
    }

    public void HandleAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print("HandleFailedToReceiveAd event received with message: " + args);
    }

    public void HandleAdOpened(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdOpened event received");
    }

    public void HandleAdClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdClosed event received");
    }

    public void HandleAdLeftApplication(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLeftApplication event received");
    }

    /*========= Admob Interstial Callbacks ========== */
    public void HandleInterstitialLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleInterstitialLoaded event received.");
    }

    public void HandleInterstitialFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print("HandleInterstitialFailedToLoad event received with message: " + args);
    }

    public void HandleInterstitialOpened(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleInterstitialOpened event received");
    }

    public void HandleInterstitialClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleInterstitialClosed event received");
        this.admob_RequestInterstitial();
    }

    public void HandleInterstitialLeftApplication(object sender, EventArgs args)
    {
        this.admob_RequestInterstitial();
        MonoBehaviour.print("HandleInterstitialLeftApplication event received");
    }

    /*========= Admob Rewarded Callbacks ========== */
    public void HandleRewardBasedVideoLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoLoaded event received");
    }

    public void HandleRewardBasedVideoFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoFailedToLoad event received with message: " + args);
    }

    public void HandleRewardBasedVideoOpened(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoOpened event received");
    }

    public void HandleRewardBasedVideoStarted(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoStarted event received");
    }

    public void HandleRewardBasedVideoClosed(object sender, EventArgs args)
    {
        if (rewardUserNow != null)
        {
            rewardUserNow(false);
        }
        this.admob_RequestRewardBasedVideo();
        MonoBehaviour.print("HandleRewardBasedVideoClosed event received");
    }

    public void HandleRewardBasedVideoRewarded(object sender, Reward args)
    {
        string type = args.Type;
        if (rewardUserNow != null)
        {
            rewardUserNow(true);
        }
        this.admob_RequestRewardBasedVideo();
        MonoBehaviour.print("HandleRewardBasedVideoRewarded event received for " + args.Amount.ToString() + " " + type);
    }

    public void HandleRewardBasedVideoLeftApplication(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoLeftApplication event received");
    }

    /*========= Unity ========== */
    void unity_ShowInterstial()
    {
        ShowOptions options = new ShowOptions();
        options.resultCallback = HandleShowResult;
        Advertisement.Show(AdIds.instance.unity.get_interstitial().Trim(), options);
    }

    void unity_ShowRewardedVideo()
    {
        ShowOptions options = new ShowOptions();
        options.resultCallback = HandleShowResult;
        Advertisement.Show(AdIds.instance.unity.get_rewarded().Trim(), options);
    }

    void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                if (rewardUserNow != null)
                {
                    rewardUserNow(true);
                }
                break;
            case ShowResult.Skipped:
                if (rewardUserNow != null)
                {
                    rewardUserNow(false);
                }
                break;
            case ShowResult.Failed:
                Debug.Log("Unity ad failed");
                if (rewardUserNow != null)
                {
                    rewardUserNow(false);
                }
                break;
        }
    }

    public void showInterstial()
    {
        if (AdsConfig.Get_RemoveAds_Status() == 0)
        {
            if (admob_showInterstitial())
            {

            }
            else if (Advertisement.IsReady(AdIds.instance.unity.get_interstitial().Trim()))
            {
                unity_ShowInterstial();
            }
        }
    }

    public void showRewardedVideo()
    {
        if (admob_ShowRewardedVideo())
        {

        }
        else if (Advertisement.IsReady(AdIds.instance.unity.get_rewarded().Trim()))
        {
            unity_ShowRewardedVideo();
        }
    }

    public bool isRewardedVideoReady()
    {
        bool isReady = false;
        if (this.admob_RewardedVideo.IsLoaded() || Advertisement.IsReady(AdIds.instance.unity.get_rewarded().Trim()))
        {
            isReady = true;
        }
        return isReady;
    }

    public bool isInterstitialReady()
    {
        bool isReady = false;
        if (this.admob_InterstitialAd.IsLoaded() || Advertisement.IsReady(AdIds.instance.unity.get_interstitial().Trim()))
        {
            isReady = true;
        }
        return isReady;
    }

    public void showBanner()
    {
        admob_ShowBanner();
    }

    public void hideBanner()
    {
        admob_HideBanner();
    }
}
