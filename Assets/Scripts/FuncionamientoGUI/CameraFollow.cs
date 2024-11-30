using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform jugador; // El transform del jugador a seguir
    public float distanciaMinima = 1f; // Distancia m�nima entre la c�mara y el jugador
    public float suavizado = 0.1f; // Factor de suavizado para el movimiento de la c�mara

    private Vector3 posicionInicial; // La posici�n inicial de la c�mara

    void Start()
    {
        // Guardamos la posici�n inicial de la c�mara
        posicionInicial = transform.position;
    }

    void Update()
    {
        // Solo actualizamos la posici�n en X, manteniendo la Y y Z
        float nuevaPosX = Mathf.Lerp(transform.position.x, jugador.position.x, suavizado);

        // Establecemos la nueva posici�n de la c�mara
        transform.position = new Vector3(nuevaPosX, transform.position.y, transform.position.z);
    }
}