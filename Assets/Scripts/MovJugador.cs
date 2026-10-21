
using UnityEngine;

public class MovimientoJugador : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidad = 3f;
    public float velocidadCarrera = 5f;

    [Header("Deslizamiento")]
    public float velocidadSlide = 9f;

    [Header("Salto")]
    public float fuerzaSalto = 10f;
    public Transform comprobadorSuelo;
    public float radioSuelo = 0.2f;
    public LayerMask capaSuelo;

    [Header("Ataque")]
    public string estadoAtaqueDerecha = "MainAttack1";
    public string estadoAtaqueIzquierda = "";

    private Rigidbody2D rb;
    private Animator animator;

    private float movimientoHorizontal;
    private float direccion = 1f;

    private bool corriendo;
    private bool agachado;
    private bool deslizando;
    private bool enSuelo;
    private bool saltando;
    private bool atacando;

    private int hashAtaqueActual;
    private bool ataqueIniciado;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        if (rb == null)
            Debug.LogError("Falta Rigidbody2D en el jugador.");

        if (animator == null)
            Debug.LogError("Falta Animator en el jugador.");

        if (comprobadorSuelo == null)
            Debug.LogError("Asigna ComprobadorSuelo en el Inspector.");
    }

    void Update()
    {
        if (rb == null || animator == null)
            return;

        // Leer el movimiento horizontal
        movimientoHorizontal = Input.GetAxisRaw("Horizontal");

        if (movimientoHorizontal < 0)
            direccion = -1f;
        else if (movimientoHorizontal > 0)
            direccion = 1f;

        // Correr mientras se mantiene Shift y hay movimiento
        corriendo =
            (Input.GetKey(KeyCode.LeftShift) ||
             Input.GetKey(KeyCode.RightShift))
            && Mathf.Abs(movimientoHorizontal) > 0;

        // Agacharse
        agachado = Input.GetKey(KeyCode.S) ||
                   Input.GetKey(KeyCode.DownArrow);

        // Deslizarse solo mientras se corre
        bool controlPulsado =
            Input.GetKey(KeyCode.LeftControl) ||
            Input.GetKey(KeyCode.RightControl);

        deslizando = corriendo && controlPulsado;

        // Detectar el suelo
        enSuelo = false;

        if (comprobadorSuelo != null)
        {
            enSuelo = Physics2D.OverlapCircle(
                comprobadorSuelo.position,
                radioSuelo,
                capaSuelo
            );
        }

        // Restablecer el salto al aterrizar
        if (enSuelo && rb.linearVelocity.y <= 0.01f)
            saltando = false;

        // Saltar con Espacio
        if (Input.GetKeyDown(KeyCode.Space) &&
            enSuelo && !agachado && !deslizando && !atacando)
        {
            saltando = true;

            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                fuerzaSalto
            );
        }

        // Iniciar ataque con clic izquierdo
        if (Input.GetMouseButtonDown(0) && !atacando)
        {
            string nombreAtaque =
                direccion < 0 && !string.IsNullOrEmpty(estadoAtaqueIzquierda)
                ? estadoAtaqueIzquierda
                : estadoAtaqueDerecha;

            hashAtaqueActual = Animator.StringToHash(nombreAtaque);

            if (animator.HasState(0, hashAtaqueActual))
            {
                atacando = true;
                ataqueIniciado = false;

                animator.SetBool("Atacando", true);
                animator.Play(hashAtaqueActual, 0, 0f);
            }
            else
            {
                Debug.LogWarning(
                    "No se encuentra el estado de ataque: " + nombreAtaque
                );
            }
        }

        // Detectar el final del ataque
        if (atacando)
        {
            AnimatorStateInfo estado =
                animator.GetCurrentAnimatorStateInfo(0);

            if (estado.fullPathHash == hashAtaqueActual)
            {
                ataqueIniciado = true;

                if (estado.normalizedTime >= 1f)
                {
                    TerminarAtaque();
                }
            }
            else if (ataqueIniciado)
            {
                // Si el Animator ya salió del ataque, liberar el bloqueo
                TerminarAtaque();
            }
        }

        // Actualizar parámetros del Animator
        animator.SetFloat(
            "Velocidad",
            Mathf.Abs(movimientoHorizontal)
        );

        animator.SetFloat("Direccion", direccion);
        animator.SetBool("Corriendo", corriendo);
        animator.SetBool("Agachado", agachado);
        animator.SetBool("Deslizando", deslizando);
        animator.SetBool("Saltando", saltando);
        animator.SetBool("EnSuelo", enSuelo);
    }

    void TerminarAtaque()
    {
        atacando = false;
        ataqueIniciado = false;
        animator.SetBool("Atacando", false);
    }

    void FixedUpdate()
    {
        if (rb == null)
            return;

        float velocidadActual;

        if (deslizando)
            velocidadActual = velocidadSlide;
        else if (corriendo)
            velocidadActual = velocidadCarrera;
        else
            velocidadActual = velocidad;

        float movimiento = deslizando
            ? direccion
            : movimientoHorizontal;

        // Detener el movimiento horizontal durante el ataque
        if (atacando)
            movimiento = 0f;

        rb.linearVelocity = new Vector2(
            movimiento * velocidadActual,
            rb.linearVelocity.y
        );
    }

    void OnDrawGizmosSelected()
    {
        if (comprobadorSuelo == null)
            return;

        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(
            comprobadorSuelo.position,
            radioSuelo
        );
    }
}
