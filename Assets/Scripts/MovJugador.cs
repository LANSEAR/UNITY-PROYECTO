
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
    public float velocidadEscalada = 1f;
    public float alturaMaximaEscalada = 3f;
    public LayerMask capaMuro;
    public LayerMask capaSuperficieEscalada;
    public float radioDeteccionMuro = 0.35f;
    public float alturaDeteccionMuro = 0.8f;
    public float distanciaDeteccionMuro = 0.35f;
    public float margenBorde = 0.05f;

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
    private Vector2 destinoEscalada;

    private Vector3 posicionInicial;

    private float gravedadNormal;
    private float movimientoHorizontal;
    private float direccion = 1f;

    private bool corriendo;
    private bool agachado;
    private bool deslizando;
    private bool enSuelo;
    private bool saltando;
    private bool atacando;
    private bool bloqueando;

    private bool escalando;
    private bool tocandoMuro;

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

        if (rb != null)
            gravedadNormal = rb.gravityScale;
        else
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

        // Detectar un muro escalable delante del jugador.
        Vector2 origenMuro = new Vector2(
            transform.position.x + direccion * distanciaDeteccionMuro,
            transform.position.y
        );

        Vector2 tamanoDeteccion = new Vector2(
            radioDeteccionMuro * 2f,
            alturaDeteccionMuro
        );

        tocandoMuro = Physics2D.OverlapBox(
            origenMuro,
            tamanoDeteccion,
            0f,
            capaMuro
        );

        // Comprobar el suelo.
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

        // Gestionar la escalada.
        if (escalando)
        {
            animator.SetBool("Escalando", true);
            animator.SetBool("Saltando", false);
            animator.SetBool("EnSuelo", false);
            animator.SetFloat("Velocidad", 0f);

            // Pulsar espacio permite abandonar la escalada.
            if (Input.GetKeyDown(KeyCode.Space))
            {
                TerminarEscalada();
            }

            return;
        }

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
                    Mathf.Clamp(
                        alturaDeslizando,
                        0.05f,
                        tamanoNormal.y
                    )
                );
            }
            else
            {
                colliderJugador.size = tamanoNormal;
                colliderJugador.offset = offsetNormal;
            }
        }

        // Primer toque: salto.
        // Segundo toque en el aire junto al muro: escalar.
        if (Input.GetKeyDown(KeyCode.Space) &&
            !agachado && !deslizando &&
            !atacando && !bloqueando && !escalando)
        {
            if (enSuelo)
            {
                saltando = true;

                rb.linearVelocity = new Vector2(
                    rb.linearVelocity.x,
                    fuerzaSalto
                );
            }
            else if (tocandoMuro)
            {
                IniciarEscalada();
                return;
            }
        }

        // Ataque y combo.
        if (Input.GetMouseButtonDown(0))
        {
            if (atacando && !segundoAtaque)
            {
                comboSolicitado = true;
                animator.SetBool("Combo", true);
            }
            else if (!atacando && !escalando)
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

    // INICIAR ESCALADA
    void IniciarEscalada()
    {
        if (colliderJugador == null)
            return;

        Bounds limites = colliderJugador.bounds;

        // Detectar el muro a la altura del centro del jugador.
        Vector2 origenMuro = limites.center;

        float distancia = limites.extents.x +
                          distanciaDeteccionMuro + 0.5f;

        RaycastHit2D golpeMuro = Physics2D.Raycast(
            origenMuro,
            Vector2.right * direccion,
            distancia,
            capaMuro
        );

        if (!golpeMuro)
        {
            Debug.Log("No se detecta el muro. Comprueba la capa y la distancia.");
            return;
        }

        Debug.Log("Muro detectado correctamente.");

        // Calcular la posición horizontal del jugador.
        float centroDestinoX = golpeMuro.point.x +
            direccion * (limites.extents.x + margenBorde);

        // Buscar la superficie superior.
        Vector2 origenSuperior = new Vector2(
            centroDestinoX,
            limites.max.y + alturaMaximaEscalada + 0.5f
        );

        RaycastHit2D golpeSuperior = Physics2D.Raycast(
            origenSuperior,
            Vector2.down,
            alturaMaximaEscalada + limites.size.y + 1f,
            capaSuperficieEscalada
        );

        if (!golpeSuperior)
        {
            Debug.Log("No se encuentra la superficie superior.");
            return;
        }

        // Comprobar que la plataforma está dentro
        // de la altura máxima permitida.
        float diferenciaAltura =
            golpeSuperior.point.y - limites.min.y;

        if (diferenciaAltura <= 0 ||
            diferenciaAltura > alturaMaximaEscalada)
        {
            Debug.Log("La plataforma está fuera del alcance.");
            return;
        }

        // Compensar el offset del collider.
        Vector2 offsetMundo = transform.TransformVector(
            colliderJugador.offset
        );

        // Calcular la posición final del jugador.
        destinoEscalada = new Vector2(
            centroDestinoX,
            golpeSuperior.point.y + limites.extents.y
        ) - offsetMundo;

        // Activar la escalada.
        escalando = true;
        saltando = false;

        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 0f;

        animator.SetBool("Saltando", false);
        animator.SetBool("Escalando", true);

        Debug.Log("Escalada activada.");
    }

    // TERMINAR ESCALADA
    void TerminarEscalada()
    {
        escalando = false;

        if (rb != null)
        {
            rb.gravityScale = gravedadNormal;
            rb.linearVelocity = Vector2.zero;
        }

        if (animator != null)
            animator.SetBool("Escalando", false);
    }

    // INICIAR ATAQUE
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

    // SEGUNDO ATAQUE
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

    // TERMINAR ATAQUE
    public void TerminarAtaque()
    {
        atacando = false;
        ataqueIniciado = false;
        comboSolicitado = false;
        segundoAtaque = false;

        animator.SetBool("Atacando", false);
        animator.SetBool("Combo", false);
    }

    // RECIBIR DAÑO
    public void RecibirDano(int cantidad)
    {
        if (cantidad <= 0 || vidaActual <= 0)
            return;

        vidaActual = Mathf.Max(0, vidaActual - cantidad);

        Debug.Log("Vida del jugador: " + vidaActual);

        if (vidaActual <= 0)
            ReiniciarJugador();
    }

    // REINICIAR JUGADOR
    void ReiniciarJugador()
    {
        transform.position = posicionInicial;

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.gravityScale = gravedadNormal;
        }

        vidaActual = vidaMaxima;

        atacando = false;
        escalando = false;
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

        if (animator != null)
        {
            animator.SetBool("Atacando", false);
            animator.SetBool("Combo", false);
            animator.SetBool("Bloqueando", false);
            animator.SetBool("Saltando", false);
            animator.SetBool("Deslizando", false);
            animator.SetBool("Escalando", false);
        }

        Debug.Log("Jugador reiniciado. Vida: " + vidaActual);
    }

    // MOVIMIENTO FÍSICO
    void FixedUpdate()
    {
        if (rb == null)
            return;

        // Movimiento de escalada.
        if (escalando)
        {
            rb.gravityScale = 0f;
            rb.linearVelocity = Vector2.zero;

            Vector2 nuevaPosicion = Vector2.MoveTowards(
                rb.position,
                destinoEscalada,
                velocidadEscalada * Time.fixedDeltaTime
            );

            rb.MovePosition(nuevaPosicion);

            if (Vector2.Distance(nuevaPosicion, destinoEscalada) < 0.02f)
            {
                rb.MovePosition(destinoEscalada);
                TerminarEscalada();
            }

            return;
        }

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

    // GIZMOS DE DETECCIÓN
    void OnDrawGizmosSelected()
    {
        if (comprobadorSuelo != null)
        {
            Gizmos.color = Color.yellow;

            Gizmos.DrawWireSphere(
                comprobadorSuelo.position,
                radioSuelo
            );
        }

        float direccionGizmo = direccion < 0 ? -1f : 1f;

        Vector2 origenMuro = new Vector2(
            transform.position.x +
                direccionGizmo * distanciaDeteccionMuro,
            transform.position.y
        );

        Gizmos.color = Color.cyan;

        Gizmos.DrawWireCube(
            origenMuro,
            new Vector2(
                radioDeteccionMuro * 2f,
                alturaDeteccionMuro
            )
        );
    }
}
