using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Puntaje : MonoBehaviour
{
    // El puntaje actual
    private int score = 0;

    // Tag del objeto con el que se debe colisionar (configurable desde el Inspector)
    public string targetTag = "Target";

    // Referencia al texto del Canvas donde se mostrará el puntaje
    public Text scoreText;

    private void Start()
    {
        // Cargar el puntaje guardado
        score = PlayerPrefs.GetInt("Puntaje", 0);
        UpdateScoreText();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Verifica si el objeto tiene el tag correcto
        if (other.CompareTag(targetTag))
        {
            // Incrementa el puntaje
            score++;

            // Guarda el puntaje en PlayerPrefs
            PlayerPrefs.SetInt("Puntaje", score);

            // Actualiza el texto del puntaje
            UpdateScoreText();

            // Destruye el objeto con el que se colisionó
            Destroy(other.gameObject);
        }
    }

    // Método para actualizar el texto del puntaje
    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Puntaje: " + score;
        }
        else
        {
            Debug.LogWarning("No se ha asignado un Text para mostrar el puntaje.");
        }
    }

    private void OnApplicationQuit()
    {
        // Limpia el puntaje al cerrar el juego (opcional)
        PlayerPrefs.DeleteKey("Puntaje");
    }
}