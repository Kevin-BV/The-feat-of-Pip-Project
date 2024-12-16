using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Luciernaga : MonoBehaviour
{
    [Header("Configuración de la Luciérnaga")]
    public Transform jugador; // Referencia al jugador
    public Transform puntoDeMira; // Punto de mira en el Inspector
    public float velocidad = 2f; // Velocidad con la que la luciérnaga se mueve

    [Header("Rangos de Distancia")]
    public float rangoMinimo = 3f; // Distancia mínima al jugador
    public float rangoMaximo = 6f; // Distancia máxima al jugador

    private void Update()
    {
        if (jugador == null || puntoDeMira == null) return; // Si no hay jugador o punto de mira asignados, no hace nada

        // Calculamos la distancia entre la luciérnaga y el jugador
        float distancia = Vector3.Distance(transform.position, jugador.position);

        // Determinamos la dirección en la que la luciérnaga debe moverse
        Vector3 direccion = Vector3.zero;

        // Si está demasiado cerca, se aleja
        if (distancia < rangoMinimo)
        {
            direccion = (transform.position - jugador.position).normalized;
        }
        // Si está demasiado lejos, se acerca
        else if (distancia > rangoMaximo)
        {
            direccion = (jugador.position - transform.position).normalized;
        }
        // Dentro del rango permitido, se mueve hacia el jugador
        else
        {
            direccion = (jugador.position - transform.position).normalized;
        }

        // Actualizamos la posición de la luciérnaga
        transform.position += direccion * velocidad * Time.deltaTime;

        // Rotar hacia el punto de mira en el eje X
        RotarHaciaPuntoDeMira();
    }

    private void RotarHaciaPuntoDeMira()
    {
        // Obtener la dirección en el eje X hacia el punto de mira
        if (puntoDeMira != null)
        {
            float direccionX = Mathf.Sign(puntoDeMira.position.x - transform.position.x);

            // Ajustar la escala en el eje X para que la luciérnaga mire hacia el punto de mira
            Vector3 nuevaEscala = transform.localScale;
            nuevaEscala.x = direccionX; // Asignar la dirección en X
            transform.localScale = nuevaEscala;
        }
    }

    // Dibujar Gizmos para visualizar los rangos en la vista de escena
    private void OnDrawGizmos()
    {
        if (jugador != null)
        {
            Gizmos.color = Color.red; // Color para el rango mínimo
            Gizmos.DrawWireSphere(jugador.position, rangoMinimo);

            Gizmos.color = Color.green; // Color para el rango máximo
            Gizmos.DrawWireSphere(jugador.position, rangoMaximo);
        }
    }
}