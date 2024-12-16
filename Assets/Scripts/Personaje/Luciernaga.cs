using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Luciernaga : MonoBehaviour
{
    [Header("Configuración de la Luciérnaga")]
    public Transform jugador; // Referencia al jugador
    public float velocidad = 2f; // Velocidad con la que la luciérnaga se mueve

    [Header("Rangos de Distancia")]
    public float rangoMinimo = 3f; // Distancia mínima al jugador
    public float rangoMaximo = 6f; // Distancia máxima al jugador

    private void Update()
    {
        if (jugador == null) return; // Si no hay jugador asignado, no hace nada

        // Calculamos la distancia entre la luciérnaga y el jugador
        float distancia = Vector3.Distance(transform.position, jugador.position);

        // Si está demasiado cerca, se aleja
        if (distancia < rangoMinimo)
        {
            Vector3 direccion = (transform.position - jugador.position).normalized;
            transform.position += direccion * velocidad * Time.deltaTime;
        }
        // Si está demasiado lejos, se acerca
        else if (distancia > rangoMaximo)
        {
            Vector3 direccion = (jugador.position - transform.position).normalized;
            transform.position += direccion * velocidad * Time.deltaTime;
        }
        // Dentro del rango permitido, se mueve hacia el jugador
        else
        {
            Vector3 direccion = (jugador.position - transform.position).normalized;
            transform.position += direccion * velocidad * Time.deltaTime;
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