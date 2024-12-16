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
        // Cargar la escena anterior jugada
        string ultimaEscena = PlayerPrefs.GetString("UltimaEscena", SceneManager.GetActiveScene().name);
        Debug.Log("Cargando la última escena jugada: " + ultimaEscena);
        SceneManager.LoadScene(ultimaEscena);
    }

    private void VolverMenu()
    {
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