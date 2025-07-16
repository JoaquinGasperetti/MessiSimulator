using Unity.Notifications.Android;
using UnityEngine;

public class NotificationManager : MonoBehaviour
{
    void Start()
    {
        // Registrar canal si no se ha hecho antes
        var channel = new AndroidNotificationChannel()
        {
            Id = "default_channel",
            Name = "Canal General",
            Importance = Importance.Default,
            Description = "Recordatorios del juego",
        };
        AndroidNotificationCenter.RegisterNotificationChannel(channel);
    }

    void OnApplicationQuit()
    {
        // Crear una notificación programada
        var notification = new AndroidNotification();
        notification.Title = "MESSI TE NECESITA!!";
        notification.Text = "Vuelve a jugar y consigue recompensas.";
        notification.FireTime = System.DateTime.Now.AddMinutes(60); // En 30 minutos

        AndroidNotificationCenter.SendNotification(notification, "default_channel");
    }
}
