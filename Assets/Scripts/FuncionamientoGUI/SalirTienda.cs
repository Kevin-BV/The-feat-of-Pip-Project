using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SalirTienda : MonoBehaviour
{
    [Header("Objeto del HUD a Activar")]
    public GameObject objetoHUD; // El objeto del HUD a activar desde el inspector

    [Header("Botón para Reanudar")]
    public Button botonReanudar; // Botón para reanudar el juego

    private bool juegoCongelado = false; // Controla si el juego está congelado

    private void Start()
    {
        // Asegurarse de que el botón tenga un listener
        if (botonReanudar != null)
        {
            botonReanudar.onClick.AddListener(ReanudarJuego);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Verificar si el objeto que entra en el trigger tiene el tag "Player"
        if (other.CompareTag("Player") && !juegoCongelado)
        {
            ActivarTienda();
        }
    }

    private void ActivarTienda()
    {
        // Activar el objeto del HUD
        if (objetoHUD != null)
        {
            objetoHUD.SetActive(true);
        }

        // Congelar la escena
        Time.timeScale = 0;
        juegoCongelado = true;

        Debug.Log("La tienda se activó y el juego está congelado.");
    }

    private void ReanudarJuego()
    {
        // Desactivar el objeto del HUD
        if (objetoHUD != null)
        {
            objetoHUD.SetActive(false);
        }

        // Reanudar la escena
        Time.timeScale = 1;
        juegoCongelado = false;

        Debug.Log("El juego se ha reanudado.");
    }
}