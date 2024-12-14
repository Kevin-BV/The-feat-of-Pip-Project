using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float distanciaMinima = 1f; // Distancia m�nima entre la c�mara y el jugador
    public float suavizado = 0.1f; // Factor de suavizado para el movimiento de la c�mara

    private Transform jugador; // Transform del jugador
    private Vector3 posicionInicial; // Posici�n inicial de la c�mara

    void Start()
    {
        // Guardamos la posici�n inicial de la c�mara
        posicionInicial = transform.position;
    }

    void Update()
    {
        // Verificar si el jugador est� activo
        if (jugador == null || !jugador.gameObject.activeInHierarchy)
        {
            BuscarJugador(); // Si no est� activo, buscar otro jugador activo
        }

        // Si el jugador es v�lido, seguimos su posici�n
        if (jugador != null && jugador.gameObject.activeInHierarchy)
        {
            // Solo actualizamos la posici�n en X, manteniendo la Y y Z
            float nuevaPosX = Mathf.Lerp(transform.position.x, jugador.position.x, suavizado);

            // Establecemos la nueva posici�n de la c�mara
            transform.position = new Vector3(nuevaPosX, transform.position.y, transform.position.z);
        }
    }

    // M�todo para buscar al jugador por el tag "Player", y verificar si est� activado
    private void BuscarJugador()
    {
        // Buscar todos los objetos con el tag "Player"
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject playerObj in players)
        {
            if (playerObj.activeInHierarchy) // Verificamos si el jugador est� activado
            {
                jugador = playerObj.transform;
                Debug.Log("Jugador encontrado: " + playerObj.name);
                return; // Salimos del m�todo una vez encontramos un jugador activo
            }
        }

        // Si no encontramos ning�n jugador activo
        Debug.LogWarning("No se encuentra ning�n jugador activo con el tag 'Player'.");
    }
}