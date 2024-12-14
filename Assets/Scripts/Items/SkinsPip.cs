using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinsPip : MonoBehaviour
{
    public GameObject objetoASpawnear; // Objeto que ser� spawneado, asignado desde el inspector
    private Transform jugador; // Referencia al jugador
    private bool jugadorDentroTrigger = false; // Verificar si el jugador est� dentro del trigger
    private bool yaSpawneado = false; // Controlar que solo spawnee una vez

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
            Debug.LogError("Jugador no encontrado. Aseg�rate de que el jugador tenga el tag 'Player'.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Verificar si el jugador ha entrado en el trigger
        if (other.CompareTag("Player"))
        {
            Debug.Log("El jugador ha entrado en el trigger.");
            jugadorDentroTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Verificar si el jugador ha salido del trigger
        if (other.CompareTag("Player"))
        {
            Debug.Log("El jugador ha salido del trigger.");
            jugadorDentroTrigger = false;
        }
    }

    private void Update()
    {
        // Comprobar si el jugador est� en el trigger y presion� la tecla X
        if (jugadorDentroTrigger && !yaSpawneado && Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log("Tecla X presionada. Destruyendo al jugador y spawneando objeto...");
            StartCoroutine(SpawnObjetoDespuesDeUnSegundo());
            yaSpawneado = true; // Evitar que se ejecute m�s de una vez
        }
    }

    private IEnumerator SpawnObjetoDespuesDeUnSegundo()
    {
        if (jugador != null)
        {
            Vector3 posicionJugador = jugador.position;
            Destroy(jugador.gameObject); // Destruir al jugador
            yield return new WaitForSeconds(1f); // Esperar 1 segundo
            Instantiate(objetoASpawnear, posicionJugador, Quaternion.identity); // Spawnear el objeto en la posici�n del jugador
        }
        else
        {
            Debug.LogError("No se encontr� al jugador para destruir o spawnear el objeto.");
        }
    }
}
