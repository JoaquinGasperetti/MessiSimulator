using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using GoogleMobileAds.Api;
using System;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverPanel;
    public Button retryButton;
    public Button mainMenuButton;

    private InterstitialAd interstitialAd;
    private BannerView bannerView;
    private RewardedInterstitialAd rewardedInterstitialAd;

    private string interstitialAdUnitId = "ca-app-pub-2266949018056491/6039459113";
    private string bannerAdUnitId = "ca-app-pub-2266949018056491/3860018339";
    private string rewardedInterstitialAdUnitId = "ca-app-pub-2266949018056491/3956401736";

    void Start()
    {
        gameOverPanel.SetActive(false);

        retryButton.onClick.AddListener(Retry);
        mainMenuButton.onClick.AddListener(GoToMainMenu);

        MobileAds.Initialize(initStatus =>
        {
            LoadInterstitialAd();
            LoadRewardedInterstitialAd();
            LoadBannerAd();
        });
    }

    public void GameOver()
    {
        Time.timeScale = 0f;
        gameOverPanel.SetActive(true);

        if (interstitialAd != null && interstitialAd.CanShowAd())
        {
            interstitialAd.Show();
        }
    }

    public void Retry()
    {
        // Guardamos el puntaje antes de reiniciar la escena
        PlayerPrefs.SetInt("PuntajeGuardado", ScoreManager.instancia.puntaje);

        if (rewardedInterstitialAd != null && rewardedInterstitialAd.CanShowAd())
        {
            rewardedInterstitialAd.Show((Reward reward) =>
            {
                Time.timeScale = 1f;
                SceneManager.LoadScene("SampleScene");
            });

            rewardedInterstitialAd.OnAdFullScreenContentClosed += () =>
            {
                LoadRewardedInterstitialAd();
            };

            rewardedInterstitialAd.OnAdFullScreenContentFailed += (AdError error) =>
            {
                Debug.LogWarning("Fallo al mostrar anuncio recompensado: " + error.GetMessage());
                LoadRewardedInterstitialAd();
                Time.timeScale = 1f;
                SceneManager.LoadScene("SampleScene");
            };
        }
        else
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("SampleScene");
        }
    }

    public void GoToMainMenu()
    {
        // Eliminamos el puntaje guardado si el jugador vuelve al menú
        PlayerPrefs.DeleteKey("PuntajeGuardado");

        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    private void LoadInterstitialAd()
    {
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
            interstitialAd = null;
        }

        AdRequest adRequest = new AdRequest();

        InterstitialAd.Load(interstitialAdUnitId, adRequest, (InterstitialAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                Debug.LogError("Error al cargar interstitial: " + error);
                return;
            }

            interstitialAd = ad;
        });
    }

    private void LoadRewardedInterstitialAd()
    {
        AdRequest adRequest = new AdRequest();

        RewardedInterstitialAd.Load(rewardedInterstitialAdUnitId, adRequest, (RewardedInterstitialAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                Debug.LogWarning("Error al cargar rewarded interstitial: " + error.GetMessage());
                return;
            }

            rewardedInterstitialAd = ad;
        });
    }

    private void LoadBannerAd()
    {
        bannerView = new BannerView(bannerAdUnitId, AdSize.Banner, AdPosition.Bottom);
        AdRequest adRequest = new AdRequest();
        bannerView.LoadAd(adRequest);
    }

    private void OnDestroy()
    {
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
        }

        if (bannerView != null)
        {
            bannerView.Destroy();
        }
    }
}
