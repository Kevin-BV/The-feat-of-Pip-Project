using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PowerUp : MonoBehaviour
{
    [Header("Configuraci�n del Power-Up")]
    public float duracionPowerUp = 10f; // Duraci�n del efecto del Power-Up en segundos
    private bool jugadorDentroDelTrigger = false; // Verifica si el jugador est� en el trigger
    private AtaquePersonaje ataquePersonaje; // Referencia al script de ataque del jugador

    private void OnTriggerEnter(Collider other)
    {
        // Detecta si el jugador entra en el trigger
        if (other.CompareTag("Player"))
        {
            jugadorDentroDelTrigger = true;
            ataquePersonaje = other.GetComponent<AtaquePersonaje>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Detecta si el jugador sale del trigger
        if (other.CompareTag("Player"))
        {
            jugadorDentroDelTrigger = false;
            ataquePersonaje = null;
        }
    }

    private void Update()
    {
        // Si el jugador est� dentro del trigger y presiona la tecla X
        if (jugadorDentroDelTrigger && Input.GetKeyDown(KeyCode.X))
        {
            ActivarPowerUp();
        }
    }

    private void ActivarPowerUp()
    {
        // Si el script de ataque del jugador no es nulo
        if (ataquePersonaje != null)
        {
            // Duplica el da�o del jugador
            StartCoroutine(AplicarPowerUp(ataquePersonaje));

            // Mensaje en consola para indicar que el Power-Up se activ�
            Debug.Log("�Power-Up activado! Da�o duplicado por " + duracionPowerUp + " segundos.");

            // Destruye el objeto del Power-Up
            Destroy(gameObject);
        }
    }

    private IEnumerator AplicarPowerUp(AtaquePersonaje ataquePersonaje)
    {
        // Almacena el da�o original
        int da�oOriginal = ataquePersonaje.dano;

        // Duplica el da�o
        ataquePersonaje.dano = da�oOriginal * 2;

        // Espera la duraci�n del Power-Up
        yield return new WaitForSeconds(duracionPowerUp);

        // Restaura el da�o original
        ataquePersonaje.dano = da�oOriginal;

        // Mensaje en consola para indicar que el Power-Up termin�
        Debug.Log("El efecto del Power-Up ha terminado. Da�o restaurado a " + da�oOriginal + ".");
    }
}