using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanoEnemigo : MonoBehaviour
{
    public int dano = 1; // Cantidad de daño que inflige el enemigo al jugador

    private void OnCollisionEnter(Collision collision)
    {
        // Verificamos si el objeto con el que colisionó tiene el tag "Player"
        if (collision.gameObject.CompareTag("Player"))
        {
            // Obtenemos el script de vida del jugador
            VidaConCorazones vidaJugador = collision.gameObject.GetComponent<VidaConCorazones>();

            if (vidaJugador != null)
            {
                // Reducimos la vida del jugador
                vidaJugador.RecibirDano(dano);
            }
        }
    }
}
