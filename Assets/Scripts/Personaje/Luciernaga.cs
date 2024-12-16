using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Luciernaga : MonoBehaviour
{
    [Header("Configuraci�n de la Luci�rnaga")]
    public Transform jugador; // Referencia al jugador
    public Transform puntoDeMira; // Punto de mira en el Inspector
    public float velocidad = 2f; // Velocidad con la que la luci�rnaga se mueve

    [Header("Rangos de Distancia")]
    public float rangoMinimo = 3f; // Distancia m�nima al jugador
    public float rangoMaximo = 6f; // Distancia m�xima al jugador

    private void Update()
    {
        if (jugador == null || puntoDeMira == null) return; // Si no hay jugador o punto de mira asignados, no hace nada

        // Calculamos la distancia entre la luci�rnaga y el jugador
        float distancia = Vector3.Distance(transform.position, jugador.position);

        // Determinamos la direcci�n en la que la luci�rnaga debe moverse
        Vector3 direccion = Vector3.zero;

        // Si est� demasiado cerca, se aleja
        if (distancia < rangoMinimo)
        {
            direccion = (transform.position - jugador.position).normalized;
        }
        // Si est� demasiado lejos, se acerca
        else if (distancia > rangoMaximo)
        {
            direccion = (jugador.position - transform.position).normalized;
        }
        // Dentro del rango permitido, se mueve hacia el jugador
        else
        {
            direccion = (jugador.position - transform.position).normalized;
        }

        // Actualizamos la posici�n de la luci�rnaga
        transform.position += direccion * velocidad * Time.deltaTime;

        // Rotar hacia el punto de mira en el eje X
        RotarHaciaPuntoDeMira();
    }

    private void RotarHaciaPuntoDeMira()
    {
        // Obtener la direcci�n en el eje X hacia el punto de mira
        if (puntoDeMira != null)
        {
            float direccionX = Mathf.Sign(puntoDeMira.position.x - transform.position.x);

            // Ajustar la escala en el eje X para que la luci�rnaga mire hacia el punto de mira
            Vector3 nuevaEscala = transform.localScale;
            nuevaEscala.x = direccionX; // Asignar la direcci�n en X
            transform.localScale = nuevaEscala;
        }
    }

    // Dibujar Gizmos para visualizar los rangos en la vista de escena
    private void OnDrawGizmos()
    {
        if (jugador != null)
        {
            Gizmos.color = Color.red; // Color para el rango m�nimo
            Gizmos.DrawWireSphere(jugador.position, rangoMinimo);

            Gizmos.color = Color.green; // Color para el rango m�ximo
            Gizmos.DrawWireSphere(jugador.position, rangoMaximo);
        }
    }
}