using UnityEngine;

public class GoalTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pelota"))
        {
            GameManager.Instance.AddPoint();

            // Opcional: destruir la pelota despu�s de anotar
            Destroy(other.gameObject);
        }
    }
}
