using System;
using System.Collections;
using System.Collections.Generic;
using AppsFlyerSDK;
using DG.Tweening;
using Firebase.Analytics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AdManager : MonoBehaviour
{
    public static AdManager adManager;
    public string key, appsflyerKey;
    public bool isInitialized;
    public bool debugStatus;
    private int rewardedFailCount;
    private int interstitialFailCount;
    private IronSourceImpressionData impressionData;

    private void Awake()
    {
        adManager = this;
        Debug.unityLogger.logEnabled = debugStatus;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        SceneManager.LoadScene(1);
        IronSourceEvents.onImpressionDataReadyEvent += OnImpressionDataReadyEvent;
    }

    private void OnImpressionDataReadyEvent(IronSourceImpressionData obj)
    {
        if (impressionData?.revenue == null) return;

        Firebase.Analytics.Parameter[] adParameters =
        {
            new Firebase.Analytics.Parameter("ad_platform", "ironSource"),
            new Firebase.Analytics.Parameter("ad_source", impressionData.adNetwork),
            new Firebase.Analytics.Parameter("ad_unit_name", impressionData.instanceName),
            new Firebase.Analytics.Parameter("ad_format", impressionData.adUnit),
            new Firebase.Analytics.Parameter("currency", "USD"),
            new Firebase.Analytics.Parameter("value", impressionData.revenue.Value)
        };
        FirebaseAnalytics.LogEvent("ad_impression", adParameters);

        var dic = new Dictionary<string, string>
        {
            { "ad_unit_name", impressionData.instanceName },
            { "ad_format", impressionData.adUnit }
        };
        AppsFlyerAdRevenue.logAdRevenue(impressionData.adNetwork,
            AppsFlyerAdRevenueMediationNetworkType.AppsFlyerAdRevenueMediationNetworkTypeIronSource,
            impressionData.revenue.Value, "USD", dic);
    }

    public void Init()
    {
        IronSource.Agent.validateIntegration();
        IronSource.Agent.init(key);
        AppsFlyer.initSDK(appsflyerKey, "");
        AppsFlyer.startSDK();
        AppsFlyerAdRevenue.start();
        AppsFlyerAdRevenue.setIsDebug(true);
        AppsFlyer.setIsDebug(true);
        //LoadBanner();
        LoadInterstitial();
        LoadRewarded();
    }

    private void OnEnable()
    {
        IronSourceEvents.onSdkInitializationCompletedEvent += SDKInitialized;

        //Add AdInfo Banner Events
        IronSourceBannerEvents.onAdLoadedEvent += BannerOnAdLoadedEvent;
        IronSourceBannerEvents.onAdLoadFailedEvent += BannerOnAdLoadFailedEvent;
        IronSourceBannerEvents.onAdClickedEvent += BannerOnAdClickedEvent;
        IronSourceBannerEvents.onAdScreenPresentedEvent += BannerOnAdScreenPresentedEvent;
        IronSourceBannerEvents.onAdScreenDismissedEvent += BannerOnAdScreenDismissedEvent;
        IronSourceBannerEvents.onAdLeftApplicationEvent += BannerOnAdLeftApplicationEvent;

        //Add AdInfo Interstitial Events
        IronSourceInterstitialEvents.onAdReadyEvent += InterstitialOnAdReadyEvent;
        IronSourceInterstitialEvents.onAdLoadFailedEvent += InterstitialOnAdLoadFailed;
        IronSourceInterstitialEvents.onAdOpenedEvent += InterstitialOnAdOpenedEvent;
        IronSourceInterstitialEvents.onAdClickedEvent += InterstitialOnAdClickedEvent;
        IronSourceInterstitialEvents.onAdShowSucceededEvent += InterstitialOnAdShowSucceededEvent;
        IronSourceInterstitialEvents.onAdShowFailedEvent += InterstitialOnAdShowFailedEvent;
        IronSourceInterstitialEvents.onAdClosedEvent += InterstitialOnAdClosedEvent;

        //Add AdInfo Rewarded Video Events
        IronSourceRewardedVideoEvents.onAdOpenedEvent += RewardedVideoOnAdOpenedEvent;
        IronSourceRewardedVideoEvents.onAdClosedEvent += RewardedVideoOnAdClosedEvent;
        IronSourceRewardedVideoEvents.onAdAvailableEvent += RewardedVideoOnAdAvailable;
        IronSourceRewardedVideoEvents.onAdUnavailableEvent += RewardedVideoOnAdUnavailable;
        IronSourceRewardedVideoEvents.onAdShowFailedEvent += RewardedVideoOnAdShowFailedEvent;
        IronSourceRewardedVideoEvents.onAdRewardedEvent += RewardedVideoOnAdRewardedEvent;
        IronSourceRewardedVideoEvents.onAdClickedEvent += RewardedVideoOnAdClickedEvent;
    }

    private void SDKInitialized()
    {
        Debug.Log("Sdk is initialized !!");
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        IronSource.Agent.onApplicationPause(pauseStatus);
    }

    #region Banner

    public void LoadBanner()
    {
        IronSource.Agent.loadBanner(IronSourceBannerSize.BANNER, IronSourceBannerPosition.BOTTOM);
    }

    public void DestroyBanner()
    {
        IronSource.Agent.destroyBanner();
    }

    /************* Banner AdInfo Delegates *************/
//Invoked once the banner has loaded
    void BannerOnAdLoadedEvent(IronSourceAdInfo adInfo)
    {
    }

//Invoked when the banner loading process has failed.
    void BannerOnAdLoadFailedEvent(IronSourceError ironSourceError)
    {
    }

// Invoked when end user clicks on the banner ad
    void BannerOnAdClickedEvent(IronSourceAdInfo adInfo)
    {
    }

//Notifies the presentation of a full screen content following user click
    void BannerOnAdScreenPresentedEvent(IronSourceAdInfo adInfo)
    {
    }

//Notifies the presented screen has been dismissed
    void BannerOnAdScreenDismissedEvent(IronSourceAdInfo adInfo)
    {
    }

//Invoked when the user leaves the app
    void BannerOnAdLeftApplicationEvent(IronSourceAdInfo adInfo)
    {
    }

    #endregion

    #region Interstitial

    public void LoadInterstitial()
    {
        IronSource.Agent.loadInterstitial();
    }

    public void ShowInterstitial(Action onFailed, Action onSuccess)
    {
        if (!IronSource.Agent.isInterstitialReady())
        {
            Debug.LogError("Interstitial not ready !!");
            interstitialFailCount++;
            DOVirtual.DelayedCall(Mathf.Pow(2, Mathf.Min(interstitialFailCount, 6)),
                (() => { LoadInterstitial(); }));
            onFailed();
            return;
        }

        if (IronSource.Agent.isInterstitialReady())
        {
            interstitialFailCount = 0;
            IronSource.Agent.showInterstitial();
            onSuccess();
        }
        else
        {
            onFailed();
            Debug.LogError("Interstitial not ready !!");
        }
    }

    /************* Interstitial AdInfo Delegates *************/
// Invoked when the interstitial ad was loaded succesfully.
    void InterstitialOnAdReadyEvent(IronSourceAdInfo adInfo)
    {
    }

// Invoked when the initialization process has failed.
    void InterstitialOnAdLoadFailed(IronSourceError ironSourceError)
    {
    }

// Invoked when the Interstitial Ad Unit has opened. This is the impression indication. 
    void InterstitialOnAdOpenedEvent(IronSourceAdInfo adInfo)
    {
    }

// Invoked when end user clicked on the interstitial ad
    void InterstitialOnAdClickedEvent(IronSourceAdInfo adInfo)
    {
        CallAdRevenue();
    }

    private void CallAdRevenue()
    {
        if (impressionData?.revenue == null)
        {
            Debug.LogError("impressionData is null");
            return;
        }

        Firebase.Analytics.Parameter[] adParameters =
        {
            new Firebase.Analytics.Parameter("ad_platform", "ironSource"),
            new Firebase.Analytics.Parameter("ad_source", impressionData.adNetwork),
            new Firebase.Analytics.Parameter("ad_unit_name", impressionData.instanceName),
            new Firebase.Analytics.Parameter("ad_format", impressionData.adUnit),
            new Firebase.Analytics.Parameter("currency", "USD"),
            new Firebase.Analytics.Parameter("value", impressionData.revenue.Value)
        };
        FirebaseAnalytics.LogEvent("ad_impression", adParameters);

        var dic = new Dictionary<string, string>
        {
            { "ad_unit_name", impressionData.instanceName },
            { "ad_format", impressionData.adUnit }
        };
        AppsFlyerAdRevenue.logAdRevenue(impressionData.adNetwork,
            AppsFlyerAdRevenueMediationNetworkType.AppsFlyerAdRevenueMediationNetworkTypeIronSource,
            impressionData.revenue.Value, "USD", dic);
        Debug.Log($"impressionddata {impressionData.revenue.Value}");
    }

    // Invoked when the ad failed to show.
    void InterstitialOnAdShowFailedEvent(IronSourceError ironSourceError, IronSourceAdInfo adInfo)
    {
    }

// Invoked when the interstitial ad closed and the user went back to the application screen.
    void InterstitialOnAdClosedEvent(IronSourceAdInfo adInfo)
    {
    }

// Invoked before the interstitial ad was opened, and before ts he InterstitialOnAdOpenedEvent ireported.
// This callback is not supported by all networks, and we recommend using it only if  
// it's supported by all networks you included in your build. 
    void InterstitialOnAdShowSucceededEvent(IronSourceAdInfo adInfo)
    {
        CallAdRevenue();
    }

    #endregion

    #region Rewarded

    public void LoadRewarded()
    {
        IronSource.Agent.loadRewardedVideo();
    }

    public void ShowRewarded(Action onFailed, Action onSuccess)
    {
        if (!IronSource.Agent.isRewardedVideoAvailable())
        {
            Debug.LogError("rewarded not ready !!");
            rewardedFailCount++;
            DOVirtual.DelayedCall(Mathf.Pow(2, Mathf.Min(rewardedFailCount, 6)),
                (() => { LoadRewarded(); }));
            onFailed();
            return;
        }

        if (IronSource.Agent.isRewardedVideoAvailable())
        {
            rewardedFailCount = 0;
            IronSource.Agent.showRewardedVideo();
            onSuccess();
        }
        else
        {
            //LoadRewarded();
            onFailed();
            Debug.LogError("Rewarded not ready!!");
        }
    }

/************* RewardedVideo AdInfo Delegates *************/
// Indicates that there’s an available ad.
// The adInfo object includes information about the ad that was loaded successfully
// This replaces the RewardedVideoAvailabilityChangedEvent(true) event
    void RewardedVideoOnAdAvailable(IronSourceAdInfo adInfo)
    {
    }

// Indicates that no ads are available to be displayed
// This replaces the RewardedVideoAvailabilityChangedEvent(false) event
    void RewardedVideoOnAdUnavailable()
    {
    }

// The Rewarded Video ad view has opened. Your activity will loose focus.
    void RewardedVideoOnAdOpenedEvent(IronSourceAdInfo adInfo)
    {
    }

// The Rewarded Video ad view is about to be closed. Your activity will regain its focus.
    void RewardedVideoOnAdClosedEvent(IronSourceAdInfo adInfo)
    {
    }

// The user completed to watch the video, and should be rewarded.
// The placement parameter will include the reward data.
// When using server-to-server callbacks, you may ignore this event and wait for the ironSource server callback.
    void RewardedVideoOnAdRewardedEvent(IronSourcePlacement placement, IronSourceAdInfo adInfo)
    {
        CallAdRevenue();
    }

// The rewarded video ad was failed to show.
    void RewardedVideoOnAdShowFailedEvent(IronSourceError error, IronSourceAdInfo adInfo)
    {
    }

// Invoked when the video ad was clicked.
// This callback is not supported by all networks, and we recommend using it only if
// it’s supported by all networks you included in your build.
    void RewardedVideoOnAdClickedEvent(IronSourcePlacement placement, IronSourceAdInfo adInfo)
    {
        CallAdRevenue();
    }

    #endregion
}