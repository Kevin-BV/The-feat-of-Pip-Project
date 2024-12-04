using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement; // Necesario para cargar escenas

public class VidaConCorazones : MonoBehaviour
{
    [Header("Configuraci�n de Vida")]
    public int vidaMaxima = 6;
    private int vidaActual;

    [Header("Configuraci�n del UI de Corazones")]
    public List<Image> corazones;
    public Sprite corazonLleno;
    public Sprite corazonMitad;
    public Sprite corazonVacio;

    [Header("Sonidos")]
    public AudioSource audioSource; // Componente de AudioSource
    public AudioClip sonidoDanio; // Sonido al recibir da�o
    public AudioClip sonidoMuerte; // Sonido al morir
    public AudioClip sonidoCuracion; // Sonido al recibir HP (por ejemplo, al consumir un �tem)

    private Animator anim; // Referencia al Animator
    private BloqueoParry bloqueoParry; // Referencia al script de bloqueo para verificar invulnerabilidad

    void Start()
    {
        vidaActual = vidaMaxima;
        ActualizarCorazones();
        anim = GetComponent<Animator>(); // Obtenemos el Animator
        bloqueoParry = GetComponent<BloqueoParry>(); // Obtenemos la referencia del script de bloqueo
    }

    public void RecibirDano(int dano)
    {
        if (bloqueoParry.PuedeRecibirDano()) // Solo recibir da�o si el jugador no est� bloqueando
        {
            vidaActual = Mathf.Max(vidaActual - dano, 0);
            ActualizarCorazones();

            // Reproducir sonido de da�o
            if (audioSource && sonidoDanio)
                audioSource.PlayOneShot(sonidoDanio);

            if (vidaActual <= 0)
            {
                Morir();
            }
        }
    }

    public void Curar(int curacion)
    {
        int vidaAnterior = vidaActual;
        vidaActual = Mathf.Min(vidaActual + curacion, vidaMaxima);
        ActualizarCorazones();

        // Solo reproducir sonido de curaci�n si la vida aument�
        if (audioSource && sonidoCuracion && vidaActual > vidaAnterior)
        {
            audioSource.PlayOneShot(sonidoCuracion);
        }
    }

    private void ActualizarCorazones()
    {
        for (int i = 0; i < corazones.Count; i++)
        {
            int puntoDeVida = (i + 1) * 2;

            if (vidaActual >= puntoDeVida)
            {
                corazones[i].sprite = corazonLleno;
            }
            else if (vidaActual == puntoDeVida - 1)
            {
                corazones[i].sprite = corazonMitad;
            }
            else
            {
                corazones[i].sprite = corazonVacio;
            }
        }
    }

    private void Morir()
    {
        Debug.Log("El jugador ha muerto.");

        PlayerPrefs.SetString("UltimaEscena", SceneManager.GetActiveScene().name);

        // Reproducir sonido de muerte
        if (audioSource && sonidoMuerte)
            audioSource.PlayOneShot(sonidoMuerte);

        // Detener el movimiento del personaje
        GetComponent<MovimientoPersonaje>().enabled = false;  // Desactiva el script de movimiento para detenerlo

        // Activar la animaci�n de muerte
        if (anim != null)
        {
            anim.SetBool("IsDead", true); // Activa la animaci�n de muerte
        }

        // Aqu� se llama a la coroutine para esperar 2 segundos antes de cargar la escena
        StartCoroutine(CargarEscenaGameOver());
    }

    private IEnumerator CargarEscenaGameOver()
    {
        // Esperar 2 segundos antes de cambiar de escena
        yield return new WaitForSeconds(2f);

        // Cambiar a la escena GameOver
        SceneManager.LoadScene("GameOver");  // Aseg�rate de que la escena GameOver exista en tu proyecto
    }
}