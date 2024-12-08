using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerOnDestroy : MonoBehaviour
{
    // Objeto que se va a spawnear
    public GameObject objectToSpawn;

    // Cantidad de objetos a spawnear
    public int spawnCount = 1;

    // M�todo que se llama cuando el objeto es destruido
    private void OnDestroy()
    {
        // Verifica que el objeto a spawnear est� asignado
        if (objectToSpawn != null)
        {
            for (int i = 0; i < spawnCount; i++)
            {
                // Instancia el objeto en la misma posici�n y rotaci�n del objeto destruido
                Instantiate(objectToSpawn, transform.position, transform.rotation);
            }
        }
        else
        {
            Debug.LogWarning("No hay ning�n objeto asignado para spawnear.");
        }
    }
}