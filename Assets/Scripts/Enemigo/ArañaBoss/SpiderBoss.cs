using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderBoss : MonoBehaviour
{
    public float moveSpeed = 5f; // Velocidad de movimiento
    public float moveAmount = 7f; // Cantidad de movimiento
    public float waitTime = 10f; // Tiempo de espera en segundos al principio y entre ciclos
    public Transform player; // Referencia al jugador (arrastrar el objeto del jugador al inspector)
    public float gizmoSize = 1f; // Tamaño del Gizmo de dirección

    public int vida = 20; // Vida de la araña (se puede editar desde el inspector)
    private Animator animator;

    public Collider ataqueCollider; // El único collider de la araña

    private int attackCounter = 0; // Contador de ataques para intercalar Attack_1 y Attack_2

    private float tiempoUltimoDanio = 0f; // Tiempo del último daño recibido

    public float intervaloDanio = 1f; // Intervalo en segundos para que el jugador reciba daño

    private bool jugadorDentroCollider = false; // Para verificar si el jugador está dentro del collider

    private float tiempoUltimoAtaque = 0f; // Tiempo del último ataque para que sea cada 4 segundos
    public float intervaloAtaque = 4f; // Intervalo entre ataques (4 segundos)

    void Start()
    {
        animator = GetComponent<Animator>();
        // Aseguramos que el collider de la araña sea un trigger
        ataqueCollider.isTrigger = true;
    }

    void Update()
    {
        // Lógica de movimiento de la araña (no cambia)
        if (player != null)
        {
            Vector3 directionToPlayer = player.position - transform.position;

            if (directionToPlayer.x < 0)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else
            {
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
        }

        // Verifica si ha pasado el intervalo para hacer un ataque
        if (jugadorDentroCollider && Time.time - tiempoUltimoAtaque >= intervaloAtaque)
        {
            // Solo aplicamos el daño si el jugador está dentro del collider y la araña ejecuta un ataque
            if (attackCounter < 3)
            {
                animator.SetTrigger("Attack_1");
                attackCounter++;
                // Hacer daño con Attack_1 (solo si está dentro del collider)
                player.GetComponent<VidaConCorazones>().RecibirDano(1);
                Debug.Log("El jugador ha recibido daño de 1 punto por Attack_1.");
            }
            else
            {
                animator.SetTrigger("Attack_2");
                attackCounter = 0; // Reseteamos el contador
                // Hacer daño con Attack_2 (solo si está dentro del collider)
                player.GetComponent<VidaConCorazones>().RecibirDano(2);
                Debug.Log("El jugador ha recibido daño de 2 puntos por Attack_2.");
            }

            tiempoUltimoAtaque = Time.time; // Actualizar el tiempo del último ataque
        }

        // Aquí eliminamos la lógica que aplicaba daño cuando el jugador está solo dentro del collider,
        // porque el daño ahora se aplica solo durante un ataque.
    }

    private void OnTriggerEnter(Collider other)
    {
        // Detectamos cuando el jugador entra en el rango del collider
        if (other.CompareTag("Player"))
        {
            jugadorDentroCollider = true; // El jugador está dentro del rango
            Debug.Log("El jugador ha entrado en el rango de la araña.");
        }
        else if (other.CompareTag("PlayerAttack"))
        {
            Debug.Log("¡La araña está recibiendo daño!");
            RecibirDano(1); // Aquí se llama a la función de daño
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Detectamos cuando el jugador sale del rango del collider
        if (other.CompareTag("Player"))
        {
            jugadorDentroCollider = false; // El jugador salió del rango
            Debug.Log("El jugador ha salido del rango de la araña.");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // No es necesario hacer nada aquí si ya estamos manejando los ataques en Update()
    }

    public void RecibirDano(int dano)
    {
        vida -= dano;
        Debug.Log("La araña recibió " + dano + " de daño. Vida restante: " + vida);

        if (vida <= 0)
        {
            Debug.Log("¡La araña ha muerto!");
            // Aquí puedes agregar la lógica de muerte (como destruir la araña, etc.)
        }
    }
}