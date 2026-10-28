
using UnityEngine;

public class PinchosDano : MonoBehaviour
{
    public int dano = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        MovimientoJugador jugador =
            other.GetComponent<MovimientoJugador>();

        if (jugador != null)
        {
            jugador.RecibirDano(dano);
        }
    }
}
