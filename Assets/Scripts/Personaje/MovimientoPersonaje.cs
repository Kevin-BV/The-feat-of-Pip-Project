using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MovimientoPersonaje : MonoBehaviour
{
    [Header("Configuración del Movimiento")]
    public float velocidad = 5f;
    public float fuerzaDeSalto = 5f;
    private bool enSuelo = true;
    private Rigidbody rb;
    private Animator anim;
    private bool puedeAumentarVelocidad = false;
    private bool velocidadAumentada = false;
    private float velocidadOriginal;

    [Header("Activación de Luciérnaga")]
    public GameObject objetoLuciernaga;
    private bool enColliderLuciernaga = false;
    private bool luciernagaActivada = false;

    [Header("Configuración del Puntaje")]
    public Puntaje puntajeScript;
    public int costoVelocidad = 30;
    public int costoLuciernaga = 40;

    [Header("Sonidos")]
    public AudioClip sonidoErrorPuntaje; // Clip de sonido cuando no hay suficiente puntaje
    private AudioSource audioSource;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        velocidadOriginal = velocidad;

        audioSource = GetComponent<AudioSource>();

        // Cargar velocidad guardada
        if (PlayerPrefs.HasKey("VelocidadPlayer"))
        {
            velocidad = PlayerPrefs.GetFloat("VelocidadPlayer", velocidadOriginal);
            if (velocidad > velocidadOriginal)
            {
                velocidadAumentada = true;
            }
        }

        // Cargar estado de la luciérnaga
        if (PlayerPrefs.HasKey("LuciérnagaActivada"))
        {
            luciernagaActivada = PlayerPrefs.GetInt("LuciérnagaActivada") == 1;
            if (luciernagaActivada && objetoLuciernaga != null)
            {
                objetoLuciernaga.SetActive(true);
            }
        }
    }

    void Update()
    {
        if (anim.GetBool("IsDead") || anim.GetBool("IsDamaged"))
            return;

        // Activar Luciérnaga
        if (enColliderLuciernaga && Input.GetKeyDown(KeyCode.X) && !luciernagaActivada)
        {
            if (puntajeScript.ConsumirPuntaje(costoLuciernaga))
            {
                objetoLuciernaga.SetActive(true);
                luciernagaActivada = true;
                PlayerPrefs.SetInt("LuciérnagaActivada", 1);
                PlayerPrefs.Save();
                Debug.Log("Luciérnaga activada y guardada.");

                // Destruir el objeto con el tag "Luciernaga"
                GameObject objetoLuci = GameObject.FindGameObjectWithTag("Luciernaga");
                if (objetoLuci)
                {
                    Destroy(objetoLuci);
                }
            }
            else
            {
                ReproducirSonidoError();
                Debug.Log("No tienes suficiente puntaje para activar la luciérnaga.");
            }
        }

        // Aumentar Velocidad
        if (puedeAumentarVelocidad && !velocidadAumentada && Input.GetKeyDown(KeyCode.X))
        {
            if (puntajeScript.ConsumirPuntaje(costoVelocidad))
            {
                velocidad = velocidadOriginal * 1.5f;
                velocidadAumentada = true;
                PlayerPrefs.SetFloat("VelocidadPlayer", velocidad);
                PlayerPrefs.Save();
                Debug.Log("Velocidad aumentada y guardada.");

                // Destruir el objeto con el tag "VelocidadExtra"
                GameObject objetoVelocidad = GameObject.FindGameObjectWithTag("VelocidadExtra");
                if (objetoVelocidad)
                {
                    Destroy(objetoVelocidad);
                }
            }
            else
            {
                ReproducirSonidoError();
                Debug.Log("No tienes suficiente puntaje para aumentar la velocidad.");
            }
        }

        // Movimiento
        float movimientoHorizontal = Input.GetAxis("Horizontal");
        float movimientoVertical = Input.GetAxis("Vertical");
        Vector3 movimiento = new Vector3(movimientoHorizontal, 0, movimientoVertical);

        if (movimiento.magnitude > 0.1f)
        {
            anim.SetBool("Correr", true);
            transform.Translate(movimiento.normalized * velocidad * Time.deltaTime, Space.World);
            float rotacionX = movimientoHorizontal > 0 ? 0.5f : movimientoHorizontal < 0 ? -0.5f : 0.5f;
            transform.localScale = new Vector3(rotacionX, 0.5f, 0.5f);
        }
        else
        {
            anim.SetBool("Correr", false);
        }

        // Salto
        if (Input.GetButtonDown("Jump") && enSuelo)
        {
            anim.SetBool("Saltar", true);
            rb.AddForce(Vector3.up * fuerzaDeSalto, ForceMode.Impulse);
            enSuelo = false;
        }
    }

    private void ReproducirSonidoError()
    {
        if (sonidoErrorPuntaje != null && audioSource != null)
        {
            audioSource.PlayOneShot(sonidoErrorPuntaje);
        }
    }

    void OnCollisionEnter(Collision colision)
    {
        if (colision.gameObject.CompareTag("Suelo"))
        {
            enSuelo = true;
            anim.SetBool("Saltar", false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("VelocidadExtra"))
        {
            puedeAumentarVelocidad = true;
        }

        if (other.CompareTag("Luciernaga"))
        {
            enColliderLuciernaga = true;
            Debug.Log("Dentro del rango de activación de Luciérnaga.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("VelocidadExtra"))
        {
            puedeAumentarVelocidad = false;
        }

        if (other.CompareTag("Luciernaga"))
        {
            enColliderLuciernaga = false;
            Debug.Log("Fuera del rango de activación de Luciérnaga.");
        }
    }
}