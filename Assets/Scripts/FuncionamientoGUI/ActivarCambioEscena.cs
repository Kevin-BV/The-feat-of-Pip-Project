using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ActivarCambioEscena : MonoBehaviour
{
    [Header("Configuraci�n del temporizador")]
    public float tiempoEspera = 6f; // Tiempo de espera configurable desde el inspector.

    [Header("HUD a activar")]
    public GameObject hudActivar; // El HUD que se activar�.

    private bool temporizadorActivo = false; // Bandera para evitar m�ltiples activaciones.
    private Coroutine coroutineActivarHUD;

    void Update()
    {
        // Verifica si hay objetos con el tag "Enemy" en la escena.
        GameObject[] enemigos = GameObject.FindGameObjectsWithTag("Enemy");

        if (enemigos.Length == 0 && !temporizadorActivo)
        {
            // Inicia el temporizador si no hay enemigos.
            temporizadorActivo = true;
            coroutineActivarHUD = StartCoroutine(ActivarHUDDespuesDeTiempo());
        }
        else if (enemigos.Length > 0 && temporizadorActivo)
        {
            // Reinicia si aparecen enemigos de nuevo.
            temporizadorActivo = false;
            if (coroutineActivarHUD != null)
            {
                StopCoroutine(coroutineActivarHUD);
                coroutineActivarHUD = null;
            }
        }
    }

    private IEnumerator ActivarHUDDespuesDeTiempo()
    {
        Debug.Log("Temporizador iniciado para activar el HUD.");
        yield return new WaitForSeconds(tiempoEspera);

        // Activa el HUD despu�s del tiempo configurado y pausa el juego.
        if (hudActivar != null)
        {
            hudActivar.SetActive(true);
            Time.timeScale = 0f; // Pausa el juego
            Debug.Log("HUD activado y juego pausado.");
        }
        else
        {
            Debug.LogWarning("El HUD no est� asignado en el inspector.");
        }
    }

    // M�todo para reanudar el juego al presionar un bot�n
    public void ReanudarJuego()
    {
        if (hudActivar != null)
        {
            hudActivar.SetActive(false); // Desactiva el HUD
        }

        Time.timeScale = 1f; // Reanuda el juego
        Debug.Log("Juego reanudado.");
    }
}