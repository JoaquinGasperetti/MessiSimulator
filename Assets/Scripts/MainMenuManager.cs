using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public AudioSource backgroundMusic;

    private string adUnitId = "ca-app-pub-2266949018056491/3860018339";
    private BannerView bannerView;

    void Start()
    {
        // Inicializar Google Mobile Ads
        MobileAds.Initialize(initStatus => {
            // Cargar y mostrar el banner una vez inicializado
            RequestBanner();
        });

        // Reproducir música de fondo
        if (backgroundMusic != null && !backgroundMusic.isPlaying)
        {
            backgroundMusic.loop = true;
            backgroundMusic.Play();
        }
    }

    private void RequestBanner()
    {
        // Crear el banner en la parte inferior de la pantalla
        bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);

        // Crear la solicitud del anuncio
        AdRequest adRequest = new AdRequest();

        // Cargar el banner
        bannerView.LoadAd(adRequest);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void RateApp()
    {
        string appPackageName = "com.HKemtrentertainment.MessiSimulator";
        string url = $"https://play.google.com/store/apps/details?id={appPackageName}&hl=es_AR";

#if UNITY_ANDROID
        Application.OpenURL(url);
#else
        Debug.Log("Abrir calificación solo disponible en Android.");
#endif
    }

    void OnDestroy()
    {
        // Destruir el banner al cerrar
        if (bannerView != null)
        {
            bannerView.Destroy();
        }
    }
}
