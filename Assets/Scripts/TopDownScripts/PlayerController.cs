using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Joystick movementJoystick;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Movimiento desde teclado
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Movimiento desde joystick
        float joystickHorizontal = movementJoystick != null ? movementJoystick.Horizontal : 0f;
        float joystickVertical = movementJoystick != null ? movementJoystick.Vertical : 0f;

        // Mezcla de ambos inputs (prioriza el teclado si se est� usando)
        float finalHorizontal = Mathf.Abs(horizontalInput) > 0.1f ? horizontalInput : joystickHorizontal;
        float finalVertical = Mathf.Abs(verticalInput) > 0.1f ? verticalInput : joystickVertical;

        Vector3 move = new Vector3(finalHorizontal, 0, finalVertical);
        rb.linearVelocity = move * moveSpeed;

        if (move.magnitude > 0.1f)
        {
            transform.forward = move;
        }
    }
}
