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

    void Start()
    {
        // Obtenemos el Rigidbody para aplicar la f�sica al personaje
        rb = GetComponent<Rigidbody>();

        // Obtenemos el Animator para controlar las animaciones
        anim = GetComponent<Animator>();
    }

    void Update()
    {
          // Si el personaje est� muerto o recibiendo da�o, no permite el movimiento
            if (anim.GetBool("IsDead") || anim.GetBool("IsDamaged"))
            {
                return; // Detiene todo el movimiento si est� muerto o recibiendo da�o
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

            // Volteamos el personaje seg�n la direcci�n del movimiento horizontal
            if (movimientoHorizontal != 0)
            {
                Vector3 escala = transform.localScale;
                escala.x = Mathf.Sign(movimientoHorizontal) * Mathf.Abs(escala.x); // Voltear en X
                transform.localScale = escala;
            }

            // Movemos el personaje
            transform.Translate(movimiento.normalized * velocidad * Time.deltaTime, Space.World);
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
}