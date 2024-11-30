using UnityEngine;
using UnityEngine.UI;

public class PantallaCompleta : MonoBehaviour
{
    public Toggle fullscreenToggle; 

    void Start()
    {
        // Inicializar el estado del toggle según la configuración actual
        fullscreenToggle.isOn = Screen.fullScreen;

        // Añadir listener al toggle
        fullscreenToggle.onValueChanged.AddListener(SetFullscreen);
    }

    void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen; // Activar/desactivar pantalla completa
    }

    private void OnDestroy()
    {
        // Eliminar el listener al destruir el objeto para evitar errores
        fullscreenToggle.onValueChanged.RemoveListener(SetFullscreen);
    }
}
