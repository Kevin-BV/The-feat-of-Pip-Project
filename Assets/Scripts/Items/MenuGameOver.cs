using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuGameOver : MonoBehaviour
{
    [Header("Botones")]
    public Button botonReintentar;
    public Button botonVolverMenu;
    public Button botonSalir;

    private void Start()
    {
        botonReintentar.onClick.AddListener(Reintentar);
        botonVolverMenu.onClick.AddListener(VolverMenu);
        botonSalir.onClick.AddListener(Salir);
    }

    private void Reintentar()
    {
        // Recuperar el puntaje antes de la escena de GameOver
        int puntajeAntesDeEscena = PlayerPrefs.GetInt("PuntajeInicial");

        // Descontar el puntaje ganado en la escena anterior (si fue mayor)
        int puntajeGanadoEnEscena = PlayerPrefs.GetInt("Puntaje") - puntajeAntesDeEscena;

        if (puntajeGanadoEnEscena > 0)
        {
            PlayerPrefs.SetInt("Puntaje", PlayerPrefs.GetInt("Puntaje") - puntajeGanadoEnEscena); // Restar el puntaje ganado
        }

        // Recargar la última escena jugada
        string ultimaEscena = PlayerPrefs.GetString("UltimaEscena", SceneManager.GetActiveScene().name);
        SceneManager.LoadScene(ultimaEscena);
    }

    private void VolverMenu()
    {
        // Cargar el menú principal
        SceneManager.LoadScene("MainMenu");
    }

    private void Salir()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}