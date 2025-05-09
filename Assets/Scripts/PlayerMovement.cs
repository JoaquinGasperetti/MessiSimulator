using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float forwardSpeed = 10f;
    public float laneDistance = 3f;
    public float jumpForce = 7f;
    public float slideDuration = 0.5f;
    public LayerMask groundLayer;
    public GameObject GameOverManager;

    private int currentLane = 1;
    private Rigidbody rb;
    private Vector3 targetPosition;
    private Animator animator;
    private bool isSliding = false;
    private float slideTimer;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>(); // Animator en el modelo hijo
        targetPosition = transform.position;
        rb.freezeRotation = true;
    }

    void Update()
    {
        Vector3 velocity = rb.linearVelocity;

        // Movimiento hacia adelante constante
        velocity.z = forwardSpeed;

        bool grounded = IsGrounded();

        // Detectar salto
        if ((Input.GetKeyDown(KeyCode.Space) || SwipeManager.swipeUp) && grounded && !isSliding)
        {
            velocity.y = jumpForce;
            animator.SetBool("IsJumping", true);
        }
        else
        {
            velocity.y = rb.linearVelocity.y;
        }

        // Detectar deslizamiento
        if ((Input.GetKeyDown(KeyCode.DownArrow) || SwipeManager.swipeDown) && grounded && !isSliding)
        {
            StartSlide();
        }
        else if ((Input.GetKeyDown(KeyCode.DownArrow) || SwipeManager.swipeDown) && !grounded)
        {
            // Forzar bajada rápida en el aire
            velocity.y = -jumpForce;
        }

        // Terminar slide
        if (isSliding)
        {
            slideTimer -= Time.deltaTime;
            if (slideTimer <= 0)
            {
                EndSlide();
            }
        }

        rb.linearVelocity = velocity;

        // Actualizar animaciones
        animator.SetFloat("Speed", grounded ? forwardSpeed : 0f);
        animator.SetBool("IsJumping", !grounded);

        // Movimiento lateral suave
        Vector3 newPosition = transform.position;
        newPosition.x = Mathf.Lerp(transform.position.x, targetPosition.x, Time.deltaTime * 10f);
        transform.position = newPosition;

        // Cambio de carril
        if (Input.GetKeyDown(KeyCode.LeftArrow) || SwipeManager.swipeLeft)
            ChangeLane(-1);
        if (Input.GetKeyDown(KeyCode.RightArrow) || SwipeManager.swipeRight)
            ChangeLane(1);
    }

    void StartSlide()
    {
        isSliding = true;
        slideTimer = slideDuration;
        animator.SetBool("Slide", true);
    }

    void EndSlide()
    {
        isSliding = false;
        animator.SetBool("Slide", false);
    }

    void ChangeLane(int direction)
    {
        int targetLane = Mathf.Clamp(currentLane + direction, 0, 2);
        if (targetLane != currentLane)
        {
            currentLane = targetLane;
            float targetX = (currentLane - 1) * laneDistance;
            targetPosition = new Vector3(targetX, transform.position.y, transform.position.z);
        }
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.1f, groundLayer);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Obstaculo"))
        {
            FindObjectOfType<GameOverManager>().GameOver();
        }
    }
}
