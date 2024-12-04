using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuCreditos : MonoBehaviour
{
    [Header("Botones")]
    
    public Button botonVolverMenu;
    public Button botonSalir;

    private void Start()
    {
        // Asignar las funciones a los botones
        
        botonVolverMenu.onClick.AddListener(VolverMenu);
        botonSalir.onClick.AddListener(Salir);
    }

    // Función para reintentar: carga la última escena antes de GameOver
    private void Reintentar()
    {
        string ultimaEscena = PlayerPrefs.GetString("UltimaEscena", "EscenaInicial"); // Usamos "EscenaInicial" como valor por defecto
        SceneManager.LoadScene(ultimaEscena);
    }

    // Función para volver al menú principal
    private void VolverMenu()
    {
        SceneManager.LoadScene("MainMenu"); // Asegúrate de que la escena "MainMenu" existe en tu proyecto
    }

    // Función para salir del juego
    private void Salir()
    {
        // Si estamos en editor de Unity, salir no cierra el juego, pero si estamos en una compilación final, se cierra
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}