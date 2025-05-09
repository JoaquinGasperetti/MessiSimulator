using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using GoogleMobileAds.Api;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverPanel;
    public Button retryButton;
    public Button mainMenuButton;

    private InterstitialAd interstitialAd;
    private BannerView bannerView;

    private string interstitialAdUnitId = "ca-app-pub-2266949018056491/6039459113";
    private string bannerAdUnitId = "ca-app-pub-2266949018056491/3860018339";

    void Start()
    {
        gameOverPanel.SetActive(false);

        retryButton.onClick.AddListener(Retry);
        mainMenuButton.onClick.AddListener(GoToMainMenu);

        // Inicializar anuncios
        MobileAds.Initialize(initStatus =>
        {
            LoadInterstitialAd();
            LoadBannerAd();
        });
    }

    public void GameOver()
    {
        Time.timeScale = 0f;
        gameOverPanel.SetActive(true);

        // Mostrar el intersticial si está listo
        if (interstitialAd != null && interstitialAd.CanShowAd())
        {
            interstitialAd.Show();
        }
    }

    public void Retry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("SampleScene");
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    private void LoadInterstitialAd()
    {
        // Destruir si ya existe
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
