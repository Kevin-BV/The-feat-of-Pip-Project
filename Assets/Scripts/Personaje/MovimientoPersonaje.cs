using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MovimientoPersonaje : MonoBehaviour
{
    public float velocidad = 5f;
    public float fuerzaDeSalto = 5f;
    private bool enSuelo = true; // Variable para verificar si est� en el suelo
    private Rigidbody rb;
    private Animator anim;
    private bool puedeAumentarVelocidad = false; // Verifica si el player est� en el collider
    private bool velocidadAumentada = false; // Verifica si ya se aument� la velocidad
    private float velocidadOriginal; // Guarda la velocidad original del jugador

    void Start()
    {
        // Obtenemos el Rigidbody para aplicar la f�sica al personaje
        rb = GetComponent<Rigidbody>();

        // Obtenemos el Animator para controlar las animaciones
        anim = GetComponent<Animator>();

        // Guardamos la velocidad original
        velocidadOriginal = velocidad;

        // Solo cargar velocidad desde PlayerPrefs si no estamos en el editor
        if (!Application.isEditor)
        {
            velocidad = PlayerPrefs.GetFloat("VelocidadPlayer", velocidadOriginal);
            if (velocidad > velocidadOriginal)
            {
                velocidadAumentada = true;
            }
        }
    }

    void Update()
    {
        // Si el personaje est� muerto o recibiendo da�o, no permite el movimiento
        if (anim.GetBool("IsDead") || anim.GetBool("IsDamaged"))
        {
            return; // Detiene todo el movimiento si est� muerto o recibiendo da�o
        }

        // Aumentar velocidad si est� en el collider y presiona X (solo una vez)
        if (puedeAumentarVelocidad && !velocidadAumentada && Input.GetKeyDown(KeyCode.X))
        {
            velocidad = velocidadOriginal * 1.5f; // Aumenta la velocidad en un 50%
            velocidadAumentada = true; // Marca que ya se aument�

            // Guardar en PlayerPrefs solo si no estamos en el editor
            if (!Application.isEditor)
            {
                PlayerPrefs.SetFloat("VelocidadPlayer", velocidad);
                PlayerPrefs.Save();
            }
        }

        // Capturamos las entradas de movimiento
        float movimientoHorizontal = Input.GetAxis("Horizontal");
        float movimientoVertical = Input.GetAxis("Vertical");

        // Creamos el vector de movimiento
        Vector3 movimiento = new Vector3(movimientoHorizontal, 0, movimientoVertical);

        // Movemos al personaje si hay movimiento
        if (movimiento.magnitude > 0.1f) // Verificamos si el personaje se est� moviendo
        {
            anim.SetBool("Correr", true); // Activamos la animaci�n de correr

            // Movemos el personaje
            transform.Translate(movimiento.normalized * velocidad * Time.deltaTime, Space.World);

            // Cambiamos la escala para simular rotaci�n en el eje X
            float rotacionX = movimientoHorizontal > 0 ? 0.5f : movimientoHorizontal < 0 ? -0.5f : 0.5f;
            transform.localScale = new Vector3(rotacionX, 0.5f, 0.5f); // Modificamos la escala en el eje X
        }
        else
        {
            anim.SetBool("Correr", false); // Detenemos la animaci�n de correr
        }

        // Detectamos la entrada de salto y verificamos si est� en el suelo
        if (Input.GetButtonDown("Jump") && enSuelo)
        {
            anim.SetBool("Saltar", true); // Activamos la animaci�n de salto
            rb.AddForce(Vector3.up * fuerzaDeSalto, ForceMode.Impulse); // Aplicamos fuerza para saltar
            enSuelo = false; // Indicamos que ya no est� en el suelo
        }
    }

    // Detecta cuando toca el suelo
    void OnCollisionEnter(Collision colision)
    {
        if (colision.gameObject.CompareTag("Suelo"))
        {
            enSuelo = true; // Indicamos que est� en el suelo
            anim.SetBool("Saltar", false); // Detenemos la animaci�n de salto
        }
    }

    // Detecta cuando entra en el collider con tag VelocidadExtra
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("VelocidadExtra"))
        {
            puedeAumentarVelocidad = true; // Habilita el aumento de velocidad
        }
    }

    // Detecta cuando sale del collider con tag VelocidadExtra
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("VelocidadExtra"))
        {
            puedeAumentarVelocidad = false; // Deshabilita el aumento de velocidad
        }
    }

    private void OnApplicationQuit()
    {
        // Resetear PlayerPrefs al salir del juego en el editor
        if (Application.isEditor)
        {
            PlayerPrefs.DeleteKey("VelocidadPlayer");
        }
    }
}