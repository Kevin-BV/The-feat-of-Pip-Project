using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Avispa : MonoBehaviour
{
    public float velocidadMovimiento = 3f; // Velocidad a la que se moverá la avispa
    public float rangoDeDeteccion = 5f; // Rango en el que detectará al jugador
    public Transform visual; // Referencia al objeto visual de la avispa (para animaciones)
    public Animator anim; // Animator para las animaciones "idle" y "fly"

    [Header("Configuración del Gizmo")]
    public float tamanoGizmo = 1f; // Tamaño del Gizmo, editable desde el inspector

    [Header("Temporizadores")]
    public float tiempoAntesDeSeguir = 3f; // Tiempo que tarda en mirar al jugador antes de seguirlo
    public float tiempoSiguiendo = 5f; // Tiempo que sigue al jugador antes de reiniciar el ciclo

    private Transform jugador; // Referencia al objeto con el tag "Player"
    private Vector3 posicionInicial; // Posición inicial de la avispa (para referencia)
    private bool mirandoDerecha = true; // Controla si la avispa está mirando hacia la derecha

    private bool estaSiguiendo = false; // Controla si la avispa está actualmente siguiendo al jugador
    private bool estaEsperando = false; // Controla si está en el estado de "mirar" antes de seguir
    private Coroutine cicloMovimiento; // Para manejar el ciclo de movimiento

    void Start()
    {
        // Buscar al jugador por su tag
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            jugador = playerObj.transform;
        }

        // Guardar la posición inicial de la avispa
        posicionInicial = transform.position;

        // Iniciar el ciclo de mirar y seguir
        if (jugador != null)
        {
            cicloMovimiento = StartCoroutine(CicloMirarYSeguir());
        }
    }

    void Update()
    {
        // Si no hay jugador, la avispa se queda en idle
        if (jugador == null)
        {
            anim.SetBool("isFlying", false); // Animación de idle
            return;
        }

        // Si está siguiendo, moverse hacia el jugador
        if (estaSiguiendo)
        {
            SeguirJugador();
        }
    }

    private void SeguirJugador()
    {
        // Calcular la dirección hacia el jugador
        Vector3 direccion = (jugador.position - transform.position).normalized;

        // Mover la avispa hacia el jugador
        transform.position += direccion * velocidadMovimiento * Time.deltaTime;

        // Actualizar la animación
        anim.SetBool("isFlying", true); // Activar la animación "fly"

        // Girar solo en X dependiendo de la dirección hacia el jugador
        if ((direccion.x > 0 && !mirandoDerecha) || (direccion.x < 0 && mirandoDerecha))
        {
            Girar();
        }
    }

    private void Girar()
    {
        mirandoDerecha = !mirandoDerecha;

        // Cambiar la escala solo en X
        Vector3 nuevaEscala = visual.localScale;
        nuevaEscala.x *= -1;
        visual.localScale = nuevaEscala;
    }

    private IEnumerator CicloMirarYSeguir()
    {
        while (true)
        {
            // Paso 1: Mirar hacia el jugador durante un tiempo
            estaSiguiendo = false; // No moverse durante este tiempo
            estaEsperando = true;

            // Girar hacia el jugador en X
            Vector3 direccion = (jugador.position - transform.position).normalized;
            if ((direccion.x > 0 && !mirandoDerecha) || (direccion.x < 0 && mirandoDerecha))
            {
                Girar();
            }

            anim.SetBool("isFlying", false); // Cambiar a animación de idle
            yield return new WaitForSeconds(tiempoAntesDeSeguir);

            // Paso 2: Seguir al jugador durante un tiempo
            estaSiguiendo = true; // Comenzar a moverse hacia el jugador
            estaEsperando = false;

            anim.SetBool("isFlying", true); // Cambiar a animación de fly
            yield return new WaitForSeconds(tiempoSiguiendo);
        }
    }

    // Dibujar un Gizmo que muestre la dirección hacia el jugador y el rango de detección
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

        // Mostrar el rango de detección
        Gizmos.DrawWireSphere(transform.position, rangoDeDeteccion);

        // Dibujar un Gizmo que represente la posición de la avispa
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, tamanoGizmo);

        // Dibujar una línea hacia el jugador si está asignado
        if (jugador != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, jugador.position);
        }
    }
}