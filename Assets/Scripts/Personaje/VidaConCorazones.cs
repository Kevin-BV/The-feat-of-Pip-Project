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

            // Guardamos los valores de vida máxima y vida actual
            Debug.Log("Vida Extra activada. Guardando valores...");
            GuardarEnPlayerPrefs("VidaMaxima", vidaMaxima);
            GuardarEnPlayerPrefs("VidaActual", vidaActual);
            PlayerPrefs.Save(); // Aseguramos que los datos se guarden inmediatamente

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
            PlayerPrefs.Save(); // Guardamos la vida actual

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
        // Guardar la última escena actual
        string escenaActual = SceneManager.GetActiveScene().name;
        PlayerPrefs.SetString("UltimaEscena", escenaActual);
        PlayerPrefs.Save(); // Aseguramos que la última escena se guarde
        Debug.Log("Escena guardada: " + escenaActual);
    }

    public void Curar(int curacion)
    {
        int vidaAnterior = vidaActual;
        vidaActual = Mathf.Min(vidaActual + curacion, vidaMaxima);
        GuardarEnPlayerPrefs("VidaActual", vidaActual);
        PlayerPrefs.Save(); // Guardamos la vida actual

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
        return PlayerPrefs.GetInt(key, defaultValue); // Eliminar Application.isEditor
    }

    private void GuardarEnPlayerPrefs(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
        PlayerPrefs.Save(); // Aseguramos que los datos se guarden inmediatamente
    }
}