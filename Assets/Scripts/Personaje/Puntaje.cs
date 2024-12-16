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

    // Variables para consumir puntaje
    public int puntajeVidaExtra = 50; // Puntaje para vida extra
    public int puntajeVelocidad = 30; // Puntaje para aumentar velocidad
    public int puntajeLuciernaga = 40; // Puntaje para luciérnaga

    // Sonido para sumar puntaje
    public AudioClip sonidoPuntaje;
    private AudioSource audioSource;

    private void Start()
    {
        // Obtener el AudioSource en el objeto
        audioSource = GetComponent<AudioSource>();

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

            // Reproducir sonido de puntaje
            if (sonidoPuntaje != null && audioSource != null)
            {
                audioSource.PlayOneShot(sonidoPuntaje);
            }

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
            scoreText.text = "" + score;
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

    // Método para consumir puntaje cuando se usa una habilidad
    public bool ConsumirPuntaje(int cantidad)
    {
        if (score >= cantidad)
        {
            score -= cantidad;
            PlayerPrefs.SetInt("Puntaje", score);  // Guardar el puntaje actualizado
            UpdateScoreText();
            return true;
        }
        return false;
    }
}