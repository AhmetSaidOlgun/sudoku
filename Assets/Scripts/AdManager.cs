using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using GoogleMobileAds.Api;

public class AdManager : MonoBehaviour
{
    // These ad units are configured to always serve test ads.
#if UNITY_ANDROID
    private string _adUnitId = "ca-app-pub-9691034239281674/9961401918";
#elif UNITY_IPHONE
  private string _adUnitId = "ca-app-pub-3940256099942544/1712485313";
#else
  private string _adUnitId = "unused";
#endif

    private RewardedAd _rewardedAd;

    private void Awake()
    {
        Managers.AdManager = this;
    }

    public void Start()
    {
        MobileAds.Initialize( (InitializationStatus initStatus) =>
        {
           LoadRewardedAd();
        });
    }
    
    public Task LoadRewardedAd()
    {
        TaskCompletionSource<bool> taskCompleted = new TaskCompletionSource<bool>();
        if (_rewardedAd != null)
        {
            _rewardedAd.Destroy();
            _rewardedAd = null;
        }
        Debug.Log("Loading the rewarded ad.");
        var adRequest = new AdRequest();
        RewardedAd.Load(_adUnitId, adRequest,
            (RewardedAd ad, LoadAdError error) =>
            {
                if (error != null || ad == null)
                {
                    Debug.LogError("Rewarded ad failed to load an ad " +
                                   "with error : " + error);
                    taskCompleted.SetResult(false);
                    return;
                }

                Debug.Log("Rewarded ad loaded with response : "
                          + ad.GetResponseInfo());

                _rewardedAd = ad;
                taskCompleted.SetResult(true);
            });
        return taskCompleted.Task;
    }

    public Task ShowRewardedAd()
    {
        TaskCompletionSource<bool> taskCompletionSource = new TaskCompletionSource<bool>();

        const string rewardMsg =
            "Rewarded ad rewarded the user. Type: {0}, amount: {1}.";

        if (_rewardedAd != null && _rewardedAd.CanShowAd())
        {
            _rewardedAd.Show((Reward reward) =>
            {
                taskCompletionSource.SetResult(true);
            });
        }
        else
        {
            taskCompletionSource.SetResult(false);
        }
        return taskCompletionSource.Task;
    }
}