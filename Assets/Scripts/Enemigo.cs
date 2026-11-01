
using UnityEngine;

public class Enemigo : MonoBehaviour
{
    [Header("Vida")]
    public int vidaMaxima = 3;
    public int vidaActual;

    private Animator animator;
    private bool muerto;

    void Start()
    {
        animator = GetComponent<Animator>();

        vidaActual = vidaMaxima;

        if (animator == null)
        {
            Debug.LogError("Falta Animator en el enemigo.");
        }
    }

    // RECIBIR DAÑO
    public void RecibirDano(int cantidad)
    {
        if (muerto || cantidad <= 0)
            return;

        vidaActual -= cantidad;

        Debug.Log("Vida del enemigo: " + vidaActual);

        if (vidaActual <= 0)
        {
            Morir();
        }
    }

    public void DestruirEnemigo()
    {
        Destroy(gameObject);
    }
    // MORIR
    void Morir()
    {
        muerto = true;

        animator.SetBool("Muerto", true);

        Debug.Log("El enemigo ha muerto.");
    }
}
