using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform jugador; // El transform del jugador a seguir
    public float distanciaMinima = 1f; // Distancia mínima entre la cámara y el jugador
    public float suavizado = 0.1f; // Factor de suavizado para el movimiento de la cámara

    private Vector3 posicionInicial; // La posición inicial de la cámara

    void Start()
    {
        // Guardamos la posición inicial de la cámara
        posicionInicial = transform.position;
    }

    void Update()
    {
        // Solo actualizamos la posición en X, manteniendo la Y y Z
        float nuevaPosX = Mathf.Lerp(transform.position.x, jugador.position.x, suavizado);

        // Establecemos la nueva posición de la cámara
        transform.position = new Vector3(nuevaPosX, transform.position.y, transform.position.z);
    }
}