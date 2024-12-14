using UnityEngine;
using UnityEngine.SceneManagement; 
using UnityEngine.UI;


public class CambioEscena : MonoBehaviour
{
    [Header("Configuraci�n de la escena")]
    [Tooltip("Nombre de la escena a la que se cambiar�")]
    public string nombreEscena; // Variable editable desde el inspector

    // M�todo para cambiar de escena desde un bot�n
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
