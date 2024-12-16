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
    private int vidaMaximaConExtra = 8;
    private bool puedeActivarVidaExtra = false;

    [Header("Configuración del UI de Corazones")]
    public List<Image> corazones;
    public Sprite corazonLleno;
    public Sprite corazonMitad;
    public Sprite corazonVacio;

    public GameObject corazonExtra;
    public GameObject backgroundGrande;

    [Header("Sonidos")]
    public AudioSource audioSource;
    public AudioClip sonidoDanio, sonidoMuerte, sonidoCuracion, sonidoVidaExtra;

    private Animator anim;
    private BloqueoParry bloqueoParry;

    void Start()
    {
        anim = GetComponent<Animator>();
        bloqueoParry = GetComponent<BloqueoParry>();

        // Cargar la vida del jugador solo si no estamos en el editor
        vidaMaxima = CargarDesdePlayerPrefs("VidaMaxima", 6);
        vidaActual = vidaMaxima;

        if (vidaMaxima == vidaMaximaConExtra)
        {
            corazonExtra.SetActive(true);
            backgroundGrande.SetActive(true);
        }

        ActualizarCorazones();
    }

    void Update()
    {
        if (puedeActivarVidaExtra && Input.GetKeyDown(KeyCode.X))
        {
            ActivarVidaExtra();
        }
    }

    private void ActivarVidaExtra()
    {
        if (vidaMaxima < vidaMaximaConExtra)
        {
            vidaMaxima = vidaMaximaConExtra;
            vidaActual = vidaMaxima;

            GuardarEnPlayerPrefs("VidaMaxima", vidaMaxima);
            GuardarEnPlayerPrefs("VidaActual", vidaActual);

            corazonExtra.SetActive(true);
            backgroundGrande.SetActive(true);

            ActualizarCorazones();

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
            GuardarEnPlayerPrefs("VidaActual", vidaActual);

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
        GuardarEnPlayerPrefs("VidaActual", vidaActual);

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

            corazones[i].enabled = i < (vidaMaxima / 2);
        }
    }

    private void Morir()
    {
        Debug.Log("El jugador ha muerto.");
        GuardarEnPlayerPrefs("UltimaEscena", SceneManager.GetActiveScene().name);

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

    public void TerminarAnimacionDanio()
    {
        if (anim != null)
        {
            anim.SetBool("IsDamaged", false);
        }
    }

    // Métodos de Helper para PlayerPrefs
    private int CargarDesdePlayerPrefs(string key, int defaultValue)
    {
        return Application.isEditor ? defaultValue : PlayerPrefs.GetInt(key, defaultValue);
    }

    private void GuardarEnPlayerPrefs(string key, int value)
    {
        if (!Application.isEditor)  // Evita guardar en el editor
        {
            PlayerPrefs.SetInt(key, value);
        }
    }

    private void GuardarEnPlayerPrefs(string key, string value)
    {
        if (!Application.isEditor)  // Evita guardar en el editor
        {
            PlayerPrefs.SetString(key, value);
        }
    }
}