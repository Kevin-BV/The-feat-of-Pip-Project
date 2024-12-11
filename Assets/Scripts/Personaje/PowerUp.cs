using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PowerUp : MonoBehaviour
{
    [Header("Configuraci�n del Power-Up")]
    public float duracionPowerUp = 10f; // Duraci�n del efecto del Power-Up en segundos
    private bool jugadorDentroDelTrigger = false; // Verifica si el jugador est� en el trigger
    private AtaquePersonaje ataquePersonaje; // Referencia al script de ataque del jugador

    [Header("Efectos Visuales")]
    public GameObject efectoPowerUp; // Objeto hijo que contiene el ParticleSystem

    private bool powerUpActivo = false; // Variable para evitar activar el Power-Up m�s de una vez

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorDentroDelTrigger = true;
            ataquePersonaje = other.GetComponent<AtaquePersonaje>();
            Debug.Log("Jugador dentro del trigger del Power-Up.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorDentroDelTrigger = false;
            ataquePersonaje = null;
            Debug.Log("Jugador sali� del trigger del Power-Up.");
        }
    }

    private void Update()
    {
        if (jugadorDentroDelTrigger && Input.GetKeyDown(KeyCode.X) && !powerUpActivo)
        {
            ActivarPowerUp();
        }
    }

    private void ActivarPowerUp()
    {
        if (ataquePersonaje != null)
        {
            powerUpActivo = true;

            Debug.Log("Power-Up activado. Duplicando da�o del jugador.");
            if (efectoPowerUp != null)
            {
                efectoPowerUp.SetActive(true); // Activa el GameObject con el ParticleSystem
            }

            StartCoroutine(AplicarPowerUp());
            Destroy(gameObject); // Destruye el objeto PowerUp
        }
    }

    private IEnumerator AplicarPowerUp()
    {
        int dano = ataquePersonaje.dano;

        ataquePersonaje.ModificarDano(dano * 2);
        Debug.Log($"Da�o inicial: {dano}, Da�o con Power-Up: {ataquePersonaje.dano}");

        yield return new WaitForSeconds(duracionPowerUp);

        ataquePersonaje.ModificarDano(dano);
        Debug.Log($"Power-Up finalizado. Da�o restaurado a: {ataquePersonaje.dano}");

        if (efectoPowerUp != null)
        {
            efectoPowerUp.SetActive(false); // Desactiva el GameObject con el ParticleSystem
            Debug.Log("Efecto visual desactivado.");
        }

        powerUpActivo = false;
    }
}