using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SeguirPlayer : MonoBehaviour
{
    public GameObject jugador1; // Referencia al primer jugador
    public GameObject jugador2; // Referencia al segundo jugador

    public float suavizado = 0.1f; // Factor de suavizado para el movimiento del objeto que sigue

    private Transform jugadorActivo; // El jugador activo que será seguido

    void Start()
    {
        // Verificar al principio cuál jugador está activo
        ActualizarJugadorActivo();
    }

    void Update()
    {
        // Verificar y actualizar cuál jugador está activo en cada frame
        ActualizarJugadorActivo();

        // Si hay un jugador activo, mover el objeto que sigue
        if (jugadorActivo != null)
        {
            // Solo actualizamos la posición en X, manteniendo la Y y Z
            float nuevaPosX = Mathf.Lerp(transform.position.x, jugadorActivo.position.x, suavizado);

            // Establecer la nueva posición del objeto que sigue
            transform.position = new Vector3(nuevaPosX, transform.position.y, transform.position.z);
        }
    }

    // Método para actualizar cuál jugador está activo
    private void ActualizarJugadorActivo()
    {
        // Si el primer jugador está activo, lo asignamos como jugador activo
        if (jugador1.activeInHierarchy)
        {
            jugadorActivo = jugador1.transform;
        }
        // Si el primer jugador no está activo pero el segundo jugador está activo, lo asignamos
        else if (jugador2.activeInHierarchy)
        {
            jugadorActivo = jugador2.transform;
        }
    }
}