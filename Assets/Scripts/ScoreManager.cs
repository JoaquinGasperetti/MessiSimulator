using UnityEngine;
using TMPro;
using UnityEngine.SocialPlatforms;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instancia;

    public int puntaje = 0;
    public int record = 0;
    public int balonesDeOro = 0;

    public TextMeshProUGUI textoPuntaje;
    public TextMeshProUGUI textoRecord;
    public TextMeshProUGUI textoBalonesDeOro;

    private float tiempoTranscurrido = 0f;

    void Awake()
    {
        if (instancia == null)
        {
            instancia = this;
            record = PlayerPrefs.GetInt("Record", 0);
            balonesDeOro = PlayerPrefs.GetInt("BalonesDeOro", 0);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        textoBalonesDeOro.text = balonesDeOro.ToString();
        // Restauramos el puntaje guardado si existe
        puntaje = PlayerPrefs.GetInt("PuntajeGuardado", 0);
        textoBalonesDeOro.text = balonesDeOro.ToString();
    }

    void Update()
    {
        tiempoTranscurrido += Time.deltaTime;

        if (tiempoTranscurrido >= 1f)
        {
            puntaje += 10;
            tiempoTranscurrido = 0f;
        }

        textoPuntaje.text = puntaje.ToString();
        textoRecord.text = record.ToString();

        if (puntaje > record)
        {
            record = puntaje;
            PlayerPrefs.SetInt("Record", record);
            ReportarRecord(record);
        }
    }

    public void SumarPuntos(int cantidad)
    {
        puntaje += cantidad;

        if (puntaje > record)
        {
            record = puntaje;
            PlayerPrefs.SetInt("Record", record);
            ReportarRecord(record);
        }
    }

    public void SumarBalonDeOro()
    {
        balonesDeOro++;
        PlayerPrefs.SetInt("BalonesDeOro", balonesDeOro);
        textoBalonesDeOro.text = "" + balonesDeOro.ToString();
    }

    public void ReiniciarRecord()
    {
        PlayerPrefs.DeleteKey("Record");
        record = 0;
    }

    public void ReiniciarBalonesDeOro()
    {
        PlayerPrefs.DeleteKey("BalonesDeOro");
        balonesDeOro = 0;
        textoBalonesDeOro.text = "";
    }

    private void ReportarRecord(int score)
    {
        if (Social.localUser.authenticated)
        {
            Social.ReportScore(score, "CgkI2JCevYUUEAIQAA", success =>
            {
                Debug.Log("Record reportado a Google Play Juegos: " + success);
            });
        }
        else
        {
            Debug.Log("No se pudo reportar el record, el usuario no está autenticado.");
        }
    }
}
