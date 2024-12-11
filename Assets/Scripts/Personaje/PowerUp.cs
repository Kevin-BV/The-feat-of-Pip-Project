using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PowerUp : MonoBehaviour
{
    [Header("Configuración del Power-Up")]
    public float duracionPowerUp = 10f; // Duración del efecto del Power-Up en segundos
    private bool jugadorDentroDelTrigger = false; // Verifica si el jugador está en el trigger
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
        // Si el jugador está dentro del trigger y presiona la tecla X
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
            // Duplica el daño del jugador
            StartCoroutine(AplicarPowerUp(ataquePersonaje));

            // Mensaje en consola para indicar que el Power-Up se activó
            Debug.Log("¡Power-Up activado! Daño duplicado por " + duracionPowerUp + " segundos.");

            // Destruye el objeto del Power-Up
            Destroy(gameObject);
        }
    }

    private IEnumerator AplicarPowerUp(AtaquePersonaje ataquePersonaje)
    {
        // Almacena el daño original
        int dañoOriginal = ataquePersonaje.dano;

        // Duplica el daño
        ataquePersonaje.dano = dañoOriginal * 2;

        // Espera la duración del Power-Up
        yield return new WaitForSeconds(duracionPowerUp);

        // Restaura el daño original
        ataquePersonaje.dano = dañoOriginal;

        // Mensaje en consola para indicar que el Power-Up terminó
        Debug.Log("El efecto del Power-Up ha terminado. Daño restaurado a " + dañoOriginal + ".");
    }
}