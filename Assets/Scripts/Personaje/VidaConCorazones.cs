using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement; // Necesario para cargar escenas

public class VidaConCorazones : MonoBehaviour
{
    [Header("Configuración de Vida")]
    public int vidaMaxima = 6;
    private int vidaActual;

    [Header("Configuración del UI de Corazones")]
    public List<Image> corazones;
    public Sprite corazonLleno;
    public Sprite corazonMitad;
    public Sprite corazonVacio;

    [Header("Sonidos")]
    public AudioSource audioSource; // Componente de AudioSource
    public AudioClip sonidoDanio; // Sonido al recibir daño
    public AudioClip sonidoMuerte; // Sonido al morir
    public AudioClip sonidoCuracion; // Sonido al recibir HP (por ejemplo, al consumir un ítem)

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
        if (bloqueoParry.PuedeRecibirDano()) // Solo recibir daño si el jugador no está bloqueando
        {
            vidaActual = Mathf.Max(vidaActual - dano, 0);
            ActualizarCorazones();

            // Reproducir sonido de daño
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

        // Solo reproducir sonido de curación si la vida aumentó
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

        // Activar la animación de muerte
        if (anim != null)
        {
            anim.SetBool("IsDead", true); // Activa la animación de muerte
        }

        // Aquí se llama a la coroutine para esperar 2 segundos antes de cargar la escena
        StartCoroutine(CargarEscenaGameOver());
    }

    private IEnumerator CargarEscenaGameOver()
    {
        // Esperar 2 segundos antes de cambiar de escena
        yield return new WaitForSeconds(2f);

        // Cambiar a la escena GameOver
        SceneManager.LoadScene("GameOver");  // Asegúrate de que la escena GameOver exista en tu proyecto
    }
}