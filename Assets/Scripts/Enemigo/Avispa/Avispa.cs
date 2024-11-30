using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Avispa : MonoBehaviour
{
    public float velocidadMovimiento = 3f; // Velocidad a la que se mover� la avispa
    public float rangoDeDeteccion = 5f; // Rango en el que detectar� al jugador
    public float rangoDeAcercamiento = 1.5f; // Distancia m�nima a la que la avispa se detiene del jugador
    public Transform visual; // Referencia al objeto visual de la avispa (para animaciones)
    public Animator anim; // Animator para las animaciones "idle", "fly" y "attack"

    [Header("Configuraci�n del Gizmo")]
    public float tamanoGizmo = 1f; // Rango de ataque, editable desde el inspector

    [Header("Temporizadores")]
    public float tiempoAntesDeSeguir = 3f; // Tiempo que tarda en mirar al jugador antes de seguirlo
    public float tiempoSiguiendo = 5f; // Tiempo que sigue al jugador antes de reiniciar el ciclo
    public float cooldownAtaque = 1f; // Tiempo de espera entre ataques

    [Header("Sistema de Vida")]
    public int vidaMaxima = 3; // Vida m�xima de la avispa
    private int vidaActual; // Vida actual de la avispa

    private Transform jugador; // Referencia al objeto con el tag "Player"
    private Vector3 posicionInicial; // Posici�n inicial de la avispa (para referencia)
    private bool mirandoDerecha = true; // Controla si la avispa est� mirando hacia la derecha

    private bool estaSiguiendo = false; // Controla si la avispa est� actualmente siguiendo al jugador
    private bool estaEsperando = false; // Controla si est� en el estado de "mirar" antes de seguir
    private bool puedeAtacar = true; // Controla si la avispa puede atacar
    private Coroutine cicloMovimiento; // Para manejar el ciclo de movimiento

    void Start()
    {
        vidaActual = vidaMaxima; // Inicializar la vida
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            jugador = playerObj.transform;
        }

        posicionInicial = transform.position;

        if (jugador != null)
        {
            cicloMovimiento = StartCoroutine(CicloMirarYSeguir());
        }
    }

    void Update()
    {
        if (vidaActual <= 0) return; // Si est� muerta, no hace nada m�s

        if (jugador == null)
        {
            anim.SetBool("isFlying", false); // Animaci�n de idle
            return;
        }

        if (estaSiguiendo)
        {
            SeguirJugador();
        }

        if (Vector3.Distance(transform.position, jugador.position) <= tamanoGizmo && puedeAtacar)
        {
            AtacarJugador();
        }
    }

    private void SeguirJugador()
    {
        if (vidaActual <= 0) return; // Si est� muerta, no se mueve

        Vector3 direccion = new Vector3(jugador.position.x - transform.position.x, 0f, jugador.position.z - transform.position.z).normalized;

        float distancia = Vector3.Distance(new Vector3(transform.position.x, 0f, transform.position.z),
                                            new Vector3(jugador.position.x, 0f, jugador.position.z));

        if (distancia > rangoDeAcercamiento)
        {
            transform.position += direccion * velocidadMovimiento * Time.deltaTime;
            anim.SetBool("isFlying", true); // Animaci�n de vuelo

            if ((direccion.x > 0 && !mirandoDerecha) || (direccion.x < 0 && mirandoDerecha))
            {
                Girar();
            }
        }
        else
        {
            anim.SetBool("isFlying", false);
        }
    }

    private void AtacarJugador()
    {
        anim.SetTrigger("Attack");
        VidaConCorazones vidaJugador = jugador.GetComponent<VidaConCorazones>();
        if (vidaJugador != null)
        {
            vidaJugador.RecibirDano(1);
        }

        StartCoroutine(CooldownAtaque());
    }

    private IEnumerator CooldownAtaque()
    {
        puedeAtacar = false;
        yield return new WaitForSeconds(cooldownAtaque);
        puedeAtacar = true;
    }

    private void Girar()
    {
        mirandoDerecha = !mirandoDerecha;

        Vector3 nuevaEscala = visual.localScale;
        nuevaEscala.x *= -1;
        visual.localScale = nuevaEscala;
    }

    private IEnumerator CicloMirarYSeguir()
    {
        while (vidaActual > 0)
        {
            estaSiguiendo = false;
            estaEsperando = true;

            Vector3 direccion = (jugador.position - transform.position).normalized;
            if ((direccion.x > 0 && !mirandoDerecha) || (direccion.x < 0 && mirandoDerecha))
            {
                Girar();
            }

            anim.SetBool("isFlying", false);
            yield return new WaitForSeconds(tiempoAntesDeSeguir);

            estaSiguiendo = true;
            estaEsperando = false;

            anim.SetBool("isFlying", true);
            yield return new WaitForSeconds(tiempoSiguiendo);
        }
    }

    public void RecibirDano(int cantidad)
    {
        if (vidaActual <= 0) return; // No recibe da�o si ya est� muerta

        vidaActual -= cantidad;
        Debug.Log($"La avispa recibi� {cantidad} de da�o. Vida restante: {vidaActual}"); // Confirmar da�o

        if (vidaActual <= 0)
        {
            Morir();
        }
    }

    private void Morir()
    {
        anim.SetTrigger("Die"); // Animaci�n de muerte
        velocidadMovimiento = 0; // Detener movimiento
        Destroy(gameObject, 5f); // Destruir despu�s de 5 segundos
    }

    private void OnTriggerEnter(Collider other)
    {
        // Verificar si colision� con algo que puede hacer da�o
        if (other.CompareTag("PlayerAttack")) // Suponiendo que el ataque del jugador tiene este tag
        {
            Debug.Log("La avispa ha sido golpeada por el ataque del jugador.");
            RecibirDano(1); // Aplicar da�o (puedes cambiar la cantidad)
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, rangoDeDeteccion);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, tamanoGizmo);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, rangoDeAcercamiento);
    }
}