using UnityEngine;

public class Disparo : MonoBehaviour
{
    public GameObject pelotaPrefab;
    public Transform firePoint;
    public Joystick aimJoystick; // Opcional
    public float pelotaSpeed = 10f;

    public void Shoot()
    {
        Vector3 shootDirection;

        if (aimJoystick != null && (Mathf.Abs(aimJoystick.Horizontal) > 0.1f || Mathf.Abs(aimJoystick.Vertical) > 0.1f))
        {
            // Direcci�n del joystick
            shootDirection = new Vector3(aimJoystick.Horizontal, 0, aimJoystick.Vertical).normalized;
        }
        else
        {
            // Si no hay joystick o no se est� usando: disparar hacia adelante del jugador
            shootDirection = transform.forward;
        }

        GameObject pelota = Instantiate(pelotaPrefab, firePoint.position, Quaternion.identity);
        Rigidbody rb = pelota.GetComponent<Rigidbody>();
        rb.linearVelocity = shootDirection * pelotaSpeed;
    }
}
