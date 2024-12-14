using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinsPip : MonoBehaviour
{
    public GameObject objetoASpawnear; // Objeto que será spawneado, asignado desde el inspector
    private Transform jugador; // Referencia al jugador

    private void Start()
    {
        // Encontrar al jugador por su tag
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            jugador = playerObj.transform;
        }
        else
        {
            Debug.LogError("Jugador no encontrado. Asegúrate de que el jugador tenga el tag 'Player'.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Verificar si el jugador ha entrado en el trigger
        if (other.CompareTag("Player"))
        {
            Debug.Log("El jugador ha entrado en el trigger.");

            // Spawnear el objeto en la posición del jugador
            if (objetoASpawnear != null && jugador != null)
            {
                Instantiate(objetoASpawnear, jugador.position, Quaternion.identity);
            }
            else
            {
                Debug.LogError("No se ha asignado el objeto a spawnear o el jugador es nulo.");
            }
        }
    }
}