using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class FinalTutorial : MonoBehaviour
{
    [Tooltip("Nombre de la escena a la que quieres cambiar.")]
    public string escenaFinal; // Nombre de la escena que puedes editar desde el inspector.

    [Tooltip("Tag que identifica a los enemigos en la escena.")]
    public string tagEnemy = "Enemy"; // Tag del enemigo, configurable desde el inspector.

    void Update()
    {
        // Verifica si hay algún objeto con el tag especificado.
        GameObject[] enemigos = GameObject.FindGameObjectsWithTag(tagEnemy);

        // Si no hay ningún enemigo en la escena, cambia a la escena especificada.
        if (enemigos.Length == 0)
        {
            CambiarEscena();
        }
    }

    void CambiarEscena()
    {
        // Verifica que el nombre de la escena no esté vacío antes de cambiar.
        if (!string.IsNullOrEmpty(escenaFinal))
        {
            Debug.Log("No quedan enemigos. Cambiando a la escena: " + escenaFinal);
            SceneManager.LoadScene(escenaFinal);
        }
        else
        {
            Debug.LogWarning("El nombre de la escena final no está configurado en el inspector.");
        }
    }
}