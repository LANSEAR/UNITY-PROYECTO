
using UnityEngine;

public class MovimientoJugador : MonoBehaviour
{
    public float velocidad = 5f;

    private Rigidbody2D rb;
    private float movimientoHorizontal;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        movimientoHorizontal = Input.GetAxisRaw("Horizontal");
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(
            movimientoHorizontal * velocidad,
            rb.linearVelocity.y
        );
    }
}
