using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MovimientoPersonaje : MonoBehaviour
{
    [Header("Configuraci�n del Movimiento")]
    public float velocidad = 5f;
    public float fuerzaDeSalto = 5f;
    private bool enSuelo = true;
    private Rigidbody rb;
    private Animator anim;
    private bool puedeAumentarVelocidad = false;
    private bool velocidadAumentada = false;
    private float velocidadOriginal;

    [Header("Activaci�n de Luci�rnaga")]
    public GameObject objetoLuciernaga; // Objeto Luci�rnaga a activar desde el Inspector
    private bool enColliderLuciernaga = false; // Variable para el rango de activaci�n
    private bool luciernagaActivada = false;

    public Puntaje puntajeScript; // Referencia al script de puntaje
    public int costoVelocidad = 30; // Costo en puntaje para aumentar la velocidad
    public int costoLuciernaga = 40; // Costo en puntaje para activar luci�rnaga

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        velocidadOriginal = velocidad;

        // Cargar la velocidad guardada
        if (PlayerPrefs.HasKey("VelocidadPlayer"))
        {
            velocidad = PlayerPrefs.GetFloat("VelocidadPlayer", velocidadOriginal);
            if (velocidad > velocidadOriginal)
            {
                velocidadAumentada = true;
            }
        }

        // Cargar el estado de la luci�rnaga
        if (PlayerPrefs.HasKey("Luci�rnagaActivada"))
        {
            luciernagaActivada = PlayerPrefs.GetInt("Luci�rnagaActivada") == 1;
            if (luciernagaActivada && objetoLuciernaga != null)
            {
                objetoLuciernaga.SetActive(true);
            }
        }
    }

    void Update()
    {
        if (anim.GetBool("IsDead") || anim.GetBool("IsDamaged"))
        {
            return;
        }

        // Activar la luci�rnaga si est� en el rango y presiona X
        if (enColliderLuciernaga && Input.GetKeyDown(KeyCode.X) && !luciernagaActivada)
        {
            if (puntajeScript.ConsumirPuntaje(costoLuciernaga))
            {
                if (objetoLuciernaga != null)
                {
                    objetoLuciernaga.SetActive(true);
                    luciernagaActivada = true;
                    PlayerPrefs.SetInt("Luci�rnagaActivada", 1); // Guardar activaci�n
                    PlayerPrefs.Save();
                    Debug.Log("Luci�rnaga activada y guardada.");
                }
            }
            else
            {
                Debug.Log("No tienes suficiente puntaje para activar la luci�rnaga.");
            }
        }

        // Aumentar velocidad si est� en el collider y presiona X
        if (puedeAumentarVelocidad && !velocidadAumentada && Input.GetKeyDown(KeyCode.X))
        {
            if (puntajeScript.ConsumirPuntaje(costoVelocidad))
            {
                velocidad = velocidadOriginal * 1.5f;
                velocidadAumentada = true;

                PlayerPrefs.SetFloat("VelocidadPlayer", velocidad); // Guardar la velocidad
                PlayerPrefs.Save();
                Debug.Log("Velocidad aumentada y guardada.");
            }
            else
            {
                Debug.Log("No tienes suficiente puntaje para aumentar la velocidad.");
            }
        }

        // Movimiento
        // Resto del c�digo sin cambios...
    

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
            Debug.Log("Dentro del rango de activaci�n de Luci�rnaga.");
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
            Debug.Log("Fuera del rango de activaci�n de Luci�rnaga.");
        }
    }

}