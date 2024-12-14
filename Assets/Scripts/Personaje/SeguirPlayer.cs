using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SeguirPlayer : MonoBehaviour
{
    public GameObject jugador1; // Referencia al primer jugador
    public GameObject jugador2; // Referencia al segundo jugador

    public float suavizado = 0.1f; // Factor de suavizado para el movimiento del objeto que sigue

    private Transform jugadorActivo; // El jugador activo que ser� seguido

    void Start()
    {
        // Verificar al principio cu�l jugador est� activo
        ActualizarJugadorActivo();
    }

    void Update()
    {
        // Verificar y actualizar cu�l jugador est� activo en cada frame
        ActualizarJugadorActivo();

        // Si hay un jugador activo, mover el objeto que sigue
        if (jugadorActivo != null)
        {
            // Solo actualizamos la posici�n en X, manteniendo la Y y Z
            float nuevaPosX = Mathf.Lerp(transform.position.x, jugadorActivo.position.x, suavizado);

            // Establecer la nueva posici�n del objeto que sigue
            transform.position = new Vector3(nuevaPosX, transform.position.y, transform.position.z);
        }
    }

    // M�todo para actualizar cu�l jugador est� activo
    private void ActualizarJugadorActivo()
    {
        // Si el primer jugador est� activo, lo asignamos como jugador activo
        if (jugador1.activeInHierarchy)
        {
            jugadorActivo = jugador1.transform;
        }
        // Si el primer jugador no est� activo pero el segundo jugador est� activo, lo asignamos
        else if (jugador2.activeInHierarchy)
        {
            jugadorActivo = jugador2.transform;
        }
    }
}