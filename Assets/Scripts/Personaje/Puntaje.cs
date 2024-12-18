using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Puntaje : MonoBehaviour
{
    private int score = 0; // Puntaje actual
    public string targetTag = "Target"; // Tag del objeto con el que se colisiona
    public Text scoreText; // Referencia al texto del Canvas
    public int puntajeVidaExtra = 50;
    public int puntajeVelocidad = 30;
    public int puntajeLuciernaga = 40;

    public AudioClip sonidoPuntaje;
    public AudioClip sonidoCompra;
    private AudioSource audioSource;

    private int puntajeAntesDeEscena = 0; // Puntaje antes de entrar en una nueva escena

    private void Start()
    {
        // Obtener el AudioSource
        audioSource = GetComponent<AudioSource>();

        // Guardar la última escena jugada
        PlayerPrefs.SetString("UltimaEscena", SceneManager.GetActiveScene().name);

        // Cargar el puntaje inicial
        if (!PlayerPrefs.HasKey("PuntajeInicial"))
        {
            PlayerPrefs.SetInt("PuntajeInicial", 0); // Puntaje inicial predeterminado
        }

        // Cargar el puntaje de la escena actual
        score = PlayerPrefs.GetInt("PuntajeInicial");
        puntajeAntesDeEscena = score; // Guardar el puntaje antes de la nueva escena
        UpdateScoreText();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            // Incrementar el puntaje
            score++;

            // Guardar el puntaje actualizado
            PlayerPrefs.SetInt("Puntaje", score);

            // Reproducir sonido de puntaje
            PlaySound(sonidoPuntaje);

            // Actualizar el texto en pantalla
            UpdateScoreText();

            // Destruir el objeto con el que se colisionó
            Destroy(other.gameObject);
        }
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = score.ToString(); // Solo mostrar el puntaje
        }
        else
        {
            Debug.LogWarning("No se ha asignado un Text para mostrar el puntaje.");
        }
    }

    public bool ConsumirPuntaje(int cantidad)
    {
        if (score >= cantidad)
        {
            score -= cantidad;

            // Guardar el puntaje actualizado
            PlayerPrefs.SetInt("Puntaje", score);

            // Actualizar el texto
            UpdateScoreText();

            // Reproducir sonido de compra
            PlaySound(sonidoCompra);

            return true;
        }
        return false;
    }

    private void OnDisable()
    {
        // Guardar el progreso actual al salir de la escena
        PlayerPrefs.SetInt("Puntaje", score);
    }

    public void ReiniciarEscena()
    {
        // Restaurar el puntaje al valor antes de entrar en la escena
        score = puntajeAntesDeEscena; // Puntaje antes de la escena

        // Actualizar texto del puntaje
        UpdateScoreText();

        // Recargar la escena actual
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GuardarProgresoParaSiguienteNivel()
    {
        // Guardar el puntaje actual como punto de partida para la siguiente escena
        PlayerPrefs.SetInt("PuntajeInicial", score);
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}