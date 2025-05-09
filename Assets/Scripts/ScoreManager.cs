using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instancia;

    public int puntaje = 0;
    public int record = 0;
    public TextMeshProUGUI textoPuntaje;
    public TextMeshProUGUI textoRecord;

    private float tiempoTranscurrido = 0f;

    void Awake()
    {
        if (instancia == null)
        {
            instancia = this;
            // Cargar el record guardado
            record = PlayerPrefs.GetInt("Record", 0);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        // Incrementamos el contador de tiempo
        tiempoTranscurrido += Time.deltaTime;

        // Sumamos puntos cada segundo
        if (tiempoTranscurrido >= 1f)
        {
            puntaje += 10; // 10 puntos por segundo
            tiempoTranscurrido = 0f;
        }

        // Actualizamos el texto en pantalla
        textoPuntaje.text = "Puntos: " + puntaje.ToString();
        textoRecord.text = "Record: " + record.ToString();

        // Actualizar el record si es superado
        if (puntaje > record)
        {
            record = puntaje;
            PlayerPrefs.SetInt("Record", record);
        }
    }

    public void SumarPuntos(int cantidad)
    {
        puntaje += cantidad;

        // Verificar si se supera el récord al sumar puntos
        if (puntaje > record)
        {
            record = puntaje;
            PlayerPrefs.SetInt("Record", record);
        }
    }

    public void ReiniciarRecord()
    {
        PlayerPrefs.DeleteKey("Record");
        record = 0;
    }
}
