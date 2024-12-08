using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerOnDestroy : MonoBehaviour
{
    // Objeto que se va a spawnear
    public GameObject objectToSpawn;

    // Cantidad de objetos a spawnear
    public int spawnCount = 1;

    // Método que se llama cuando el objeto es destruido
    private void OnDestroy()
    {
        // Verifica que el objeto a spawnear esté asignado
        if (objectToSpawn != null)
        {
            for (int i = 0; i < spawnCount; i++)
            {
                // Instancia el objeto en la misma posición y rotación del objeto destruido
                Instantiate(objectToSpawn, transform.position, transform.rotation);
            }
        }
        else
        {
            Debug.LogWarning("No hay ningún objeto asignado para spawnear.");
        }
    }
}