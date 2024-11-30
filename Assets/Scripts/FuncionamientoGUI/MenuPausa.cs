using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPausa : MonoBehaviour
{
    public GameObject pauseMenuUI; // Asigna el panel del men� de pausa en el inspector
    private bool isPaused = false;

    void Update()
    {
        // Detectar tecla "Esc" para pausar/despausar
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false); // Ocultar men� de pausa
        Time.timeScale = 1f; // Reanudar el tiempo
        isPaused = false;
    }

    public void PauseGame()
    {
        pauseMenuUI.SetActive(true); // Mostrar men� de pausa
        Time.timeScale = 0f; // Pausar el tiempo
        isPaused = true;
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f; // Asegurarse de que el tiempo est� normal antes de cargar otra escena
        SceneManager.LoadScene("MainMenu"); // Cambia "MainMenu" por el nombre de tu escena del men� principal
    }

    public void QuitGame()
    {
        Time.timeScale = 1f; // Normalizar tiempo antes de salir
        Application.Quit(); // Salir del juego (solo funciona en compilaciones, no en el editor)
    }
}
