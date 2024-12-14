using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float distanciaMinima = 1f; // Distancia mínima entre la cámara y el jugador
    public float suavizado = 0.1f; // Factor de suavizado para el movimiento de la cámara

    private Transform jugador; // Transform del jugador
    private Vector3 posicionInicial; // Posición inicial de la cámara

    void Start()
    {
        // Guardamos la posición inicial de la cámara
        posicionInicial = transform.position;
    }

    void Update()
    {
        // Verificar si el jugador está activo
        if (jugador == null || !jugador.gameObject.activeInHierarchy)
        {
            BuscarJugador(); // Si no está activo, buscar otro jugador activo
        }

        // Si el jugador es válido, seguimos su posición
        if (jugador != null && jugador.gameObject.activeInHierarchy)
        {
            // Solo actualizamos la posición en X, manteniendo la Y y Z
            float nuevaPosX = Mathf.Lerp(transform.position.x, jugador.position.x, suavizado);

            // Establecemos la nueva posición de la cámara
            transform.position = new Vector3(nuevaPosX, transform.position.y, transform.position.z);
        }
    }

    // Método para buscar al jugador por el tag "Player", y verificar si está activado
    private void BuscarJugador()
    {
        // Buscar todos los objetos con el tag "Player"
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject playerObj in players)
        {
            if (playerObj.activeInHierarchy) // Verificamos si el jugador está activado
            {
                jugador = playerObj.transform;
                Debug.Log("Jugador encontrado: " + playerObj.name);
                return; // Salimos del método una vez encontramos un jugador activo
            }
        }

        // Si no encontramos ningún jugador activo
        Debug.LogWarning("No se encuentra ningún jugador activo con el tag 'Player'.");
    }
}