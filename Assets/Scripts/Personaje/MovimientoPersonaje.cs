using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MovimientoPersonaje : MonoBehaviour
{
    public float velocidad = 5f;
    public float fuerzaDeSalto = 5f;
    private bool enSuelo = true; // Variable para verificar si está en el suelo
    private Rigidbody rb;

    void Start()
    {
        // Obtenemos el Rigidbody para aplicar la física al personaje
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Capturamos las entradas de movimiento
        float movimientoHorizontal = Input.GetAxis("Horizontal");
        float movimientoVertical = Input.GetAxis("Vertical");

        // Creamos el vector de movimiento
        Vector3 movimiento = new Vector3(movimientoHorizontal, 0, movimientoVertical) * velocidad * Time.deltaTime;

        // Movemos el personaje
        transform.Translate(movimiento, Space.World);

        // Detectamos la entrada de salto y verificamos si está en el suelo
        if (Input.GetButtonDown("Jump") && enSuelo)
        {
            // Aplicamos fuerza en el eje Y para saltar
            rb.AddForce(Vector3.up * fuerzaDeSalto, ForceMode.Impulse);
            enSuelo = false; // Indicamos que el personaje ya no está en el suelo
        }
    }

    // Detecta cuando toca el suelo
    void OnCollisionEnter(Collision colision)
    {
        if (colision.gameObject.CompareTag("Suelo"))
        {
            enSuelo = true; // Indicamos que está en el suelo
        }
    }
}