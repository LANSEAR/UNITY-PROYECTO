
using UnityEngine;

public class MovimientoJugador : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidad = 3f;
    public float velocidadCarrera = 5f;

    [Header("Deslizamiento")]
    public float velocidadSlide = 9f;
    public float alturaDeslizando = 0.10f;

    [Header("Salto")]
    public float fuerzaSalto = 10f;
    public Transform comprobadorSuelo;
    public float radioSuelo = 0.2f;
    public LayerMask capaSuelo;

    [Header("Escalada")]
    public float velocidadEscalada = 3f;
    public LayerMask capaMuro;
    private bool escalando = false;
    private bool tocandoMuro = false;

    [Header("Ataque")]
    public string estadoAtaqueDerecha = "MainAttack";
    public string estadoAtaqueIzquierda = "LeftAttack1";
    public string estadoAtaque2Derecha = "MainAttack2";
    public string estadoAtaque2Izquierda = "LeftAttack2";

    [Header("Vida")]
    public int vidaMaxima = 3;
    public int vidaActual;

    private Rigidbody2D rb;
    private Animator animator;
    private CapsuleCollider2D colliderJugador;

    private Vector2 tamanoNormal;
    private Vector2 offsetNormal;
    private Vector3 posicionInicial;

    private float movimientoHorizontal;
    private float direccion = 1f;

    private bool corriendo;
    private bool agachado;
    private bool deslizando;
    private bool enSuelo;
    private bool saltando;
    private bool atacando;
    private bool bloqueando;

    private bool comboSolicitado;
    private bool segundoAtaque;
    private bool ataqueIniciado;

    private int hashAtaqueActual;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        colliderJugador = GetComponent<CapsuleCollider2D>();

        posicionInicial = transform.position;
        vidaActual = vidaMaxima;

        if (rb == null)
            Debug.LogError("Falta Rigidbody2D en el jugador.");

        if (animator == null)
            Debug.LogError("Falta Animator en el jugador.");

        if (comprobadorSuelo == null)
            Debug.LogError("Asigna ComprobadorSuelo en el Inspector.");

        if (colliderJugador != null)
        {
            tamanoNormal = colliderJugador.size;
            offsetNormal = colliderJugador.offset;
        }
        else
        {
            Debug.LogError("Falta CapsuleCollider2D en el jugador.");
        }
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

        bloqueando = Input.GetKey(KeyCode.F);
        animator.SetBool("Bloqueando", bloqueando);

        bool controlPulsado =
            Input.GetKey(KeyCode.LeftControl) ||
            Input.GetKey(KeyCode.RightControl);

        deslizando = corriendo && controlPulsado;

        // Reducir el collider durante el deslizamiento.
        if (colliderJugador != null)
        {
            if (deslizando)
            {
                colliderJugador.size = new Vector2(
                    tamanoNormal.x,
                    alturaDeslizando
                );
            }
            else
            {
                colliderJugador.size = tamanoNormal;
                colliderJugador.offset = offsetNormal;
            }
        }



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

        // Ataque y combo.
        if (Input.GetMouseButtonDown(0))
        {
            if (atacando && !segundoAtaque)
            {
                comboSolicitado = true;
                animator.SetBool("Combo", true);
            }
            else if (!atacando)
            {
                IniciarAtaque(
                    direccion < 0
                    ? estadoAtaqueIzquierda
                    : estadoAtaqueDerecha
                );
            }
        }

        // Comprobar el final del ataque actual.
        if (atacando)
        {
            AnimatorStateInfo estado =
                animator.GetCurrentAnimatorStateInfo(0);

            if (estado.fullPathHash == hashAtaqueActual)
            {
                ataqueIniciado = true;

                if (estado.normalizedTime >= 1f)
                {
                    if (!segundoAtaque && comboSolicitado)
                    {
                        string siguienteAtaque =
                            direccion < 0
                            ? estadoAtaque2Izquierda
                            : estadoAtaque2Derecha;

                        IniciarSegundoAtaque(siguienteAtaque);
                    }
                    else
                    {
                        TerminarAtaque();
                    }
                }
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

    void IniciarAtaque(string nombreEstado)
    {
        int hash = Animator.StringToHash(
            "Base Layer." + nombreEstado
        );

        if (!animator.HasState(0, hash))
        {
            Debug.LogWarning(
                "No se encuentra el estado de ataque: " + nombreEstado
            );
            return;
        }

        atacando = true;
        segundoAtaque = false;
        comboSolicitado = false;
        ataqueIniciado = false;
        hashAtaqueActual = hash;

        animator.SetBool("Atacando", true);
        animator.SetBool("Combo", false);
        animator.Play(hashAtaqueActual, 0, 0f);
    }

    void IniciarSegundoAtaque(string nombreEstado)
    {
        int hash = Animator.StringToHash(
            "Base Layer." + nombreEstado
        );

        if (!animator.HasState(0, hash))
        {
            Debug.LogWarning(
                "No se encuentra el segundo ataque: " + nombreEstado
            );

            TerminarAtaque();
            return;
        }

        segundoAtaque = true;
        comboSolicitado = false;
        ataqueIniciado = false;
        hashAtaqueActual = hash;

        animator.SetBool("Combo", false);
        animator.Play(hashAtaqueActual, 0, 0f);
    }

    public void TerminarAtaque()
    {
        atacando = false;
        ataqueIniciado = false;
        comboSolicitado = false;
        segundoAtaque = false;

        animator.SetBool("Atacando", false);
        animator.SetBool("Combo", false);
    }

    public void RecibirDano(int cantidad)
    {
        if (cantidad <= 0 || vidaActual <= 0)
            return;

        vidaActual = Mathf.Max(0, vidaActual - cantidad);

        Debug.Log("Vida del jugador: " + vidaActual);

        if (vidaActual <= 0)
            ReiniciarJugador();
    }

    void ReiniciarJugador()
    {
        transform.position = posicionInicial;
        rb.linearVelocity = Vector2.zero;

        vidaActual = vidaMaxima;

        atacando = false;
        comboSolicitado = false;
        segundoAtaque = false;
        ataqueIniciado = false;
        bloqueando = false;
        saltando = false;

        if (colliderJugador != null)
        {
            colliderJugador.size = tamanoNormal;
            colliderJugador.offset = offsetNormal;
        }

        animator.SetBool("Atacando", false);
        animator.SetBool("Combo", false);
        animator.SetBool("Bloqueando", false);
        animator.SetBool("Saltando", false);
        animator.SetBool("Deslizando", false);

        Debug.Log("Jugador reiniciado. Vida: " + vidaActual);
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
