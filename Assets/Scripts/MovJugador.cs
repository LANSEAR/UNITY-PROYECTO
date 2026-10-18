
using UnityEngine;

public class MovimientoJugador : MonoBehaviour
{
    public float velocidad = 5f;

    private Rigidbody2D rb;
    private Animator animator;
    private float movimientoHorizontal;
    private bool agachado;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Leer el movimiento horizontal
        movimientoHorizontal = Input.GetAxisRaw("Horizontal");

        // Detectar si el jugador está agachado
        agachado = Input.GetKey(KeyCode.S) ||
                   Input.GetKey(KeyCode.DownArrow);

        // Actualizar las animaciones
        animator.SetFloat(
            "Velocidad",
            Mathf.Abs(movimientoHorizontal)
        );

        animator.SetBool("Agachado", agachado);
    }

    void FixedUpdate()
    {
        // Mover al personaje horizontalmente
        rb.linearVelocity = new Vector2(
            movimientoHorizontal * velocidad,
            rb.linearVelocity.y
        );
    }
}
