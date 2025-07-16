using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using UnityEngine;
using UnityEngine.SceneManagement;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;
using System;

public class MainMenuManager : MonoBehaviour
{
    public AudioSource backgroundMusic;

    private string bannerAdUnitId = "ca-app-pub-2266949018056491/3860018339";
    private string appOpenAdUnitId = "ca-app-pub-2266949018056491/8186512516";

    private BannerView bannerView;
    private AppOpenAd appOpenAd;
    private DateTime appOpenLoadTime;

    void Start()
    {
        // Inicializar anuncios
        MobileAds.RaiseAdEventsOnUnityMainThread = true;
        MobileAds.Initialize(initStatus =>
        {
            RequestBanner();
            LoadAppOpenAd();
        });

        if (backgroundMusic != null && !backgroundMusic.isPlaying)
        {
            backgroundMusic.loop = true;
            backgroundMusic.Play();
        }

        ShowAppOpenAdIfAvailable();
    }

    private void RequestBanner()
    {
        bannerView = new BannerView(bannerAdUnitId, AdSize.Banner, AdPosition.Bottom);
        AdRequest adRequest = new AdRequest();
        bannerView.LoadAd(adRequest);
    }

    private void LoadAppOpenAd()
    {
        AdRequest request = new AdRequest();
        AppOpenAd.Load(appOpenAdUnitId, request, (AppOpenAd ad, LoadAdError error) =>
        {
            if (error != null)
            {
                Debug.LogWarning("Fallo al cargar App Open Ad: " + error.GetMessage());
                return;
            }

            appOpenAd = ad;
            appOpenLoadTime = DateTime.UtcNow;
        });
    }

    private void ShowAppOpenAdIfAvailable()
    {
        if (appOpenAd != null && IsAdAvailable())
        {
            appOpenAd.Show();
        }
        else
        {
            LoadAppOpenAd();
        }
    }

    private bool IsAdAvailable()
    {
        return appOpenAd != null && (DateTime.UtcNow - appOpenLoadTime).TotalHours < 4;
    }

    private void HandleAdClosed()
    {
        appOpenAd = null;
        LoadAppOpenAd();
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void RateApp()
    {
        string url = $"https://play.google.com/store/apps/details?id=com.HKemtrentainment.MessiSimulator&hl=es_AR";
        Application.OpenURL(url);
    }

    public void OpenLeaderboard()
    {
        if (Social.localUser.authenticated)
        {
            PlayGamesPlatform.Instance.ShowLeaderboardUI("CgkI2JCevYUUEAIQAA");
        }
        else
        {
            Debug.Log("El usuario no está autenticado.");
        }
    }

    void OnDestroy()
    {
        if (bannerView != null)
        {
            bannerView.Destroy();
        }
    }
}
