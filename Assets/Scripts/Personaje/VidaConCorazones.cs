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

    public Puntaje puntajeScript; // Referencia al script de puntaje
    public int costoVidaExtra = 50; // Costo en puntaje para activar vida extra

    void Start()
    {
        anim = GetComponent<Animator>();
        bloqueoParry = GetComponent<BloqueoParry>();

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
            if (puntajeScript.ConsumirPuntaje(costoVidaExtra))
            {
                ActivarVidaExtra();
            }
            else
            {
                Debug.Log("No tienes suficiente puntaje para activar la vida extra.");
            }
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
                GuardarUltimaEscena();
                Morir();
            }
        }
    }

    private void GuardarUltimaEscena()
    {
        string escenaActual = SceneManager.GetActiveScene().name;
        PlayerPrefs.SetString("UltimaEscena", escenaActual);
        Debug.Log("Escena guardada: " + escenaActual);
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

    private void Morir()
    {
        Debug.Log("El jugador ha muerto.");
        if (audioSource && sonidoMuerte)
            audioSource.PlayOneShot(sonidoMuerte);

        if (anim != null)
        {
            anim.SetBool("IsDead", true);
        }

        StartCoroutine(ActivarAnimacionMuerte());
    }

    private IEnumerator ActivarAnimacionMuerte()
    {
        anim.SetTrigger("DeathPhase1"); // Primera animación de muerte
        yield return new WaitForSeconds(1.5f); // Duración estimada de la primera animación
        anim.SetTrigger("DeathPhase2"); // Segunda animación de muerte
        yield return new WaitForSeconds(1.5f);

        StartCoroutine(CargarEscenaGameOver());
    }

    private IEnumerator CargarEscenaGameOver()
    {
        yield return new WaitForSeconds(1.5f);
        ReiniciarVida();
        SceneManager.LoadScene("GameOver");
    }

    private void ReiniciarVida()
    {
        vidaMaxima = 6;
        vidaActual = vidaMaxima;
        GuardarEnPlayerPrefs("VidaActual", vidaActual);
        GuardarEnPlayerPrefs("VidaMaxima", vidaMaxima);
        Debug.Log("Vida reiniciada a su valor original");
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

    private int CargarDesdePlayerPrefs(string key, int defaultValue)
    {
        return Application.isEditor ? defaultValue : PlayerPrefs.GetInt(key, defaultValue);
    }

    private void GuardarEnPlayerPrefs(string key, int value)
    {
        if (!Application.isEditor)
        {
            PlayerPrefs.SetInt(key, value);
        }
    }
}