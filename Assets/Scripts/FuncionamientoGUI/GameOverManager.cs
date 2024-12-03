using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [Header("Configuración de la pantalla de Game Over")]
    public GameObject gameOverUI; // Referencia al objeto que contiene la pantalla de Game Over

    private bool isGameOver = false;

    void Start()
    {
        // Asegurarnos de que la UI de Game Over esté desactivada al inicio
        gameOverUI.SetActive(false);
    }

    public void ActivarGameOver()
    {
        if (isGameOver) return; // Evitar activarlo varias veces

        isGameOver = true;

        // Pausar el tiempo del juego
        Time.timeScale = 0f;

        // Mostrar la pantalla de Game Over
        gameOverUI.SetActive(true);
    }

    public void Reintentar()
    {
        // Restaurar el tiempo del juego
        Time.timeScale = 1f;

        // Recargar la escena actual
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void VolverAlMenu()
    {
        // Restaurar el tiempo del juego
        Time.timeScale = 1f;

        // Cargar la escena del menú principal (ajusta el nombre o índice si es necesario)
        SceneManager.LoadScene("MainMenu");
    }

    public void Salir()
    {
        // Restaurar el tiempo del juego
        Time.timeScale = 1f;

        // Cerrar el juego
        Application.Quit();

        // Si estás probando en el editor de Unity
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
