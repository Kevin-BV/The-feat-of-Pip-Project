using UnityEngine;
using UnityEngine.SceneManagement; 
using UnityEngine.UI;


public class CambioEscena : MonoBehaviour
{
    [Header("Configuración de la escena")]
    [Tooltip("Nombre de la escena a la que se cambiará")]
    public string nombreEscena; // Variable editable desde el inspector

    // Método para cambiar de escena desde un botón
    public void CambiarEscena()
    {
        if (!string.IsNullOrEmpty(nombreEscena))
        {
            SceneManager.LoadScene(nombreEscena);
        }
        else
        {
            Debug.LogWarning("No se ha asignado un nombre de escena en el Inspector.");
        }
    }
}
