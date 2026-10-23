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
    public string estadoAtaqueDerecha = "MainAttack";
    public string estadoAtaqueIzquierda = "LeftAttack1";

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

        movimientoHorizontal = Input.GetAxisRaw("Horizontal");

        if (movimientoHorizontal < 0)
            direccion = -1f;
        else if (movimientoHorizontal > 0)
            direccion = 1f;

        corriendo =
            (Input.GetKey(KeyCode.LeftShift) ||
             Input.GetKey(KeyCode.RightShift))
            && Mathf.Abs(movimientoHorizontal) > 0;

        agachado = Input.GetKey(KeyCode.S) ||
                   Input.GetKey(KeyCode.DownArrow);

        bool controlPulsado =
            Input.GetKey(KeyCode.LeftControl) ||
            Input.GetKey(KeyCode.RightControl);

        deslizando = corriendo && controlPulsado;

        enSuelo = false;

        if (comprobadorSuelo != null)
        {
            enSuelo = Physics2D.OverlapCircle(
                comprobadorSuelo.position,
                radioSuelo,
                capaSuelo
            );
        }

        if (enSuelo && rb.linearVelocity.y <= 0.01f)
            saltando = false;

        if (Input.GetKeyDown(KeyCode.Space) &&
            enSuelo && !agachado && !deslizando && !atacando)
        {
            saltando = true;

            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                fuerzaSalto
            );
        }

        // Ataque con clic izquierdo del ratón
        if (Input.GetMouseButtonDown(0) && !atacando)
        {
            string nombreAtaque =
                direccion < 0
                ? estadoAtaqueIzquierda
                : estadoAtaqueDerecha;

            string rutaEstado = "Base Layer." + nombreAtaque;
            hashAtaqueActual = Animator.StringToHash(rutaEstado);

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
                    "No se encuentra el estado de ataque: "
                    + rutaEstado
                );
            }
        }

        // Detectar cuándo termina el ataque
        if (atacando)
        {
            AnimatorStateInfo estado =
                animator.GetCurrentAnimatorStateInfo(0);

            if (estado.fullPathHash == hashAtaqueActual)
            {
                ataqueIniciado = true;

                if (estado.normalizedTime >= 1f)
                    TerminarAtaque();
            }
            else if (ataqueIniciado)
            {
                TerminarAtaque();
            }
        }

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

    public void TerminarAtaque()
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
