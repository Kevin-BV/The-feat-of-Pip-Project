using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoEnemy : MonoBehaviour
{
    public Transform jugador; // Referencia al jugador
    public float velocidadMovimiento = 2f; // Velocidad de movimiento del enemigo
    public float distanciaPermitidaZ = 0.1f; // Tolerancia en Z antes de mover en X
    public Animator anim; // Referencia al Animator del enemigo
    public Transform visual; // Referencia al GameObject hijo para el giro visual
    public Transform rangoDeteccion; // El objeto hijo que representará el rango de detección
    public float rangoDeDeteccion = 2f; // Rango de acercamiento en el que el enemigo deja de moverse

    private Vector3 posicionObjetivo; // La posición objetivo que el enemigo seguirá
    private bool mirandoDerecha = true; // Para controlar la dirección actual del enemigo
    private bool enMovimiento = true; // Estado que indica si el enemigo está moviéndose o detenido

    void Update()
    {
        // No ejecutar lógica de movimiento si la velocidad es 0
        if (velocidadMovimiento <= 0f) return;

        if (!anim.GetBool("isTakingDamage"))
        {
            // Aquí iría el código para realizar el ataque
            anim.SetTrigger("Attack");
        }
        if (jugador == null) return;

        // Calcular la distancia al jugador usando la posición del rango de detección
        float distanciaAlJugador = Vector3.Distance(rangoDeteccion.position, jugador.position);

        // Si el jugador está dentro del rango de acercamiento, el enemigo se detiene
        if (distanciaAlJugador <= rangoDeDeteccion) // El enemigo deja de moverse si está demasiado cerca
        {
            enMovimiento = false;
            anim.SetBool("Walk", false); // Detener la animación de caminar
        }
        else
        {
            enMovimiento = true;
        }

        // Si el enemigo está en movimiento
        if (enMovimiento)
        {
            // Posición actual del enemigo y del jugador
            Vector3 posicionEnemigo = transform.position;
            Vector3 posicionJugador = jugador.position;

            // Movimiento en Z primero (frontal)
            if (Mathf.Abs(posicionEnemigo.z - posicionJugador.z) > distanciaPermitidaZ)
            {
                posicionObjetivo = new Vector3(posicionEnemigo.x, posicionEnemigo.y, posicionJugador.z);
            }
            else
            {
                posicionObjetivo = new Vector3(posicionJugador.x, posicionEnemigo.y, posicionEnemigo.z);
            }

            // Movimiento suave hacia la posición objetivo
            transform.position = Vector3.MoveTowards(posicionEnemigo, posicionObjetivo, velocidadMovimiento * Time.deltaTime);

            // Rotación en X (solo afecta al hijo visual)
            if (posicionObjetivo.x > posicionEnemigo.x && !mirandoDerecha)
            {
                Girar();
            }
            else if (posicionObjetivo.x < posicionEnemigo.x && mirandoDerecha)
            {
                Girar();
            }

            // Cambiar la animación según el movimiento
            if (posicionEnemigo != posicionObjetivo)
            {
                anim.SetBool("Walk", true); // Activar la animación de caminar
            }
            else
            {
                anim.SetBool("Walk", false); // Detener la animación de caminar
            }
        }
        else
        {
            // El enemigo está detenido, no se mueve y no hace nada
            anim.SetBool("Walk", false); // Asegurarse de que la animación de caminar esté apagada
        }
    }

    private void Girar()
    {
        mirandoDerecha = !mirandoDerecha;

        // Invertir la escala del objeto visual, no del objeto principal
        Vector3 nuevaEscala = visual.localScale;
        nuevaEscala.x *= -1;
        visual.localScale = nuevaEscala;
    }

    // Mostrar los Gizmos en la vista de la escena para el rango de acercamiento
    private void OnDrawGizmosSelected()
    {
        // Gizmo para el rango de acercamiento, mostrando el gizmo alrededor del rango de detección
        if (rangoDeteccion != null)
        {
            Gizmos.color = Color.green; // Color para el rango de acercamiento
            Gizmos.DrawWireSphere(rangoDeteccion.position, rangoDeDeteccion); // Trazar la esfera alrededor del rango de detección
        }

        // Gizmo para el rango de seguimiento del jugador
        if (jugador != null)
        {
            Gizmos.color = Color.blue; // Color para el rango de seguimiento
            Gizmos.DrawLine(transform.position, jugador.position); // Línea que conecta al enemigo con el jugador
        }
    }
}