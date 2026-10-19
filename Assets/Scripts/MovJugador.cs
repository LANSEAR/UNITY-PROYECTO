
using UnityEngine;

public class MovimientoJugador : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidad = 3f;
    public float velocidadCarrera = 5f;

    [Header("Deslizamiento")]
    public float velocidadSlide = 9f;

    private Rigidbody2D rb;
    private Animator animator;

    private float movimientoHorizontal;
    private float direccion = 1f;

    private bool corriendo;
    private bool agachado;
    private bool deslizando;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Leer el movimiento horizontal
        movimientoHorizontal = Input.GetAxisRaw("Horizontal");

        // Recordar la última dirección
        if (movimientoHorizontal < 0)
        {
            direccion = -1f;
        }
        else if (movimientoHorizontal > 0)
        {
            direccion = 1f;
        }

        // Correr solo si hay movimiento y Shift está pulsado
        corriendo =
            (Input.GetKey(KeyCode.LeftShift) ||
             Input.GetKey(KeyCode.RightShift))
            && Mathf.Abs(movimientoHorizontal) > 0;

        // Agacharse manteniendo S o flecha abajo
        agachado = Input.GetKey(KeyCode.S) ||
                   Input.GetKey(KeyCode.DownArrow);

        // Deslizarse manteniendo Ctrl
        deslizando =
            Input.GetKey(KeyCode.LeftControl) ||
            Input.GetKey(KeyCode.RightControl);

        // Actualizar parámetros del Animator
        animator.SetFloat(
            "Velocidad",
            Mathf.Abs(movimientoHorizontal)
        );

        animator.SetFloat("Direccion", direccion);
        animator.SetBool("Corriendo", corriendo);
        animator.SetBool("Agachado", agachado);
        animator.SetBool("Deslizando", deslizando);
    }

    void FixedUpdate()
    {
        float velocidadActual;

        if (deslizando)
        {
            velocidadActual = velocidadSlide;
        }
        else if (corriendo)
        {
            velocidadActual = velocidadCarrera;
        }
        else
        {
            velocidadActual = velocidad;
        }

        float movimiento = deslizando
            ? direccion
            : movimientoHorizontal;

        rb.linearVelocity = new Vector2(
            movimiento * velocidadActual,
            rb.linearVelocity.y
        );
    }
}
