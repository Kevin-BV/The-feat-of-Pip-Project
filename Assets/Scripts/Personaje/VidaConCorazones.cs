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
    private int vidaMaximaConExtra = 8; // Vida máxima cuando se activa la mejora
    private bool puedeActivarVidaExtra = false;

    [Header("Configuración del UI de Corazones")]
    public List<Image> corazones;
    public Sprite corazonLleno;
    public Sprite corazonMitad;
    public Sprite corazonVacio;

    public GameObject corazonExtra; // Corazón adicional del HUD (desactivado por defecto)
    public GameObject backgroundGrande; // Nuevo background grande para los corazones

    [Header("Sonidos")]
    public AudioSource audioSource; // Componente de AudioSource
    public AudioClip sonidoDanio; // Sonido al recibir daño
    public AudioClip sonidoMuerte; // Sonido al morir
    public AudioClip sonidoCuracion; // Sonido al recibir HP
    public AudioClip sonidoVidaExtra; // Sonido cuando se activa la vida extra

    private Animator anim;
    private BloqueoParry bloqueoParry;

    void Start()
    {
        vidaActual = vidaMaxima;
        ActualizarCorazones();
        anim = GetComponent<Animator>();
        bloqueoParry = GetComponent<BloqueoParry>();
    }

    void Update()
    {
        // Si está en el trigger y presiona X
        if (puedeActivarVidaExtra && Input.GetKeyDown(KeyCode.X))
        {
            ActivarVidaExtra();
        }
    }

    private void ActivarVidaExtra()
    {
        if (vidaMaxima < vidaMaximaConExtra) // Solo si aún no está activada
        {
            vidaMaxima = vidaMaximaConExtra; // Aumentar vida máxima
            vidaActual = vidaMaxima; // Rellenar la vida actual al máximo
            corazonExtra.SetActive(true); // Activar el corazón extra en el HUD
            backgroundGrande.SetActive(true); // Activar el background grande

            ActualizarCorazones();

            // Reproducir sonido de vida extra
            if (audioSource && sonidoVidaExtra)
                audioSource.PlayOneShot(sonidoVidaExtra);

            Debug.Log("¡Vida Extra activada!");
        }
    }

    public void RecibirDano(int dano)
    {
        if (bloqueoParry.PuedeRecibirDano() && !anim.GetBool("IsDamaged"))
        {
            vidaActual = Mathf.Max(vidaActual - dano, 0);
            ActualizarCorazones();

            if (anim != null)
            {
                anim.SetTrigger("Damage");
                anim.SetBool("IsDamaged", true);
            }

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

            // Activar/desactivar corazones según la vida máxima
            corazones[i].enabled = i < (vidaMaxima / 2);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("VidaExtra"))
        {
            Debug.Log("En el trigger de Vida Extra. Presiona X para activarlo.");
            puedeActivarVidaExtra = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("VidaExtra"))
        {
            Debug.Log("Saliste del trigger de Vida Extra.");
            puedeActivarVidaExtra = false;
        }
    }

    private void Morir()
    {
        Debug.Log("El jugador ha muerto.");
        PlayerPrefs.SetString("UltimaEscena", SceneManager.GetActiveScene().name);

        if (audioSource && sonidoMuerte)
            audioSource.PlayOneShot(sonidoMuerte);

        GetComponent<MovimientoPersonaje>().enabled = false;

        if (anim != null)
        {
            anim.SetBool("IsDead", true);
        }

        StartCoroutine(CargarEscenaGameOver());
    }

    private IEnumerator CargarEscenaGameOver()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("GameOver");
    }
    public void TerminarAnimacionDanio()
    {
        if (anim != null)
        {
            anim.SetBool("IsDamaged", false); // Permitimos volver a atacar y moverse
        }
    }
}