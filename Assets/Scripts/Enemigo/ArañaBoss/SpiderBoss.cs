using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Necesario para trabajar con UI

using UnityEngine.SceneManagement; // Para cambiar de escena

public class SpiderBoss : MonoBehaviour
{
    public float moveSpeed = 5f; // Velocidad de movimiento
    public float moveAmount = 7f; // Cantidad de movimiento
    public float waitTime = 10f; // Tiempo de espera en segundos al principio y entre ciclos
    public Transform player; // Referencia al jugador
    public float gizmoSize = 1f; // Tamaño del Gizmo de dirección

    public int vida = 50; // Vida de la araña
    private Animator animator;

    public Collider ataqueCollider; // El único collider de la araña

    private int attackCounter = 0; // Contador de ataques para intercalar Attack_1 y Attack_2

    private float tiempoUltimoDanio = 0f; // Tiempo del último daño recibido
    public float intervaloDanio = 1f; // Intervalo en segundos para que el jugador reciba daño

    private bool jugadorDentroCollider = false; // Verifica si el jugador está dentro del collider
    private float tiempoUltimoAtaque = 0f; // Tiempo del último ataque para que sea cada 4 segundos
    public float intervaloAtaque = 4f; // Intervalo entre ataques (4 segundos)

    // Referencia a la barra de vida
    public Image barraVida; // Barra de vida del boss (relleno)
    public Image bordeBarraVida; // Borde de la barra de vida (opcional)

    private bool estaMuerta = false; // Indica si la araña está muerta

    [Tooltip("Nombre de la escena de créditos")]
    public string escenaCreditos = "Creditos"; // Escena que se cargará tras la muerte

    void Start()
    {
        animator = GetComponent<Animator>();
        ataqueCollider.isTrigger = true;
        ActualizarBarraVida();
    }

    void Update()
    {
        // Si está muerta, no hace nada
        if (estaMuerta) return;

        // Lógica de movimiento de la araña
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

        // Ataque solo si el jugador está en rango
        if (jugadorDentroCollider && Time.time - tiempoUltimoAtaque >= intervaloAtaque)
        {
            if (attackCounter < 3)
            {
                animator.SetTrigger("Attack_1");
                attackCounter++;
                player.GetComponent<VidaConCorazones>().RecibirDano(1);
                Debug.Log("El jugador ha recibido daño de 1 punto por Attack_1.");
            }
            else
            {
                animator.SetTrigger("Attack_2");
                attackCounter = 0;
                player.GetComponent<VidaConCorazones>().RecibirDano(2);
                Debug.Log("El jugador ha recibido daño de 2 puntos por Attack_2.");
            }

            tiempoUltimoAtaque = Time.time;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (estaMuerta) return; // No detecta colisiones si está muerta

        if (other.CompareTag("Player"))
        {
            jugadorDentroCollider = true;
            Debug.Log("El jugador ha entrado en el rango de la araña.");
        }
        else if (other.CompareTag("PlayerAttack"))
        {
            Debug.Log("¡La araña está recibiendo daño!");
            RecibirDano(1);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (estaMuerta) return;

        if (other.CompareTag("Player"))
        {
            jugadorDentroCollider = false;
            Debug.Log("El jugador ha salido del rango de la araña.");
        }
    }

    public void RecibirDano(int dano)
    {
        if (estaMuerta) return; // No puede recibir daño si ya está muerta

        vida -= dano;
        Debug.Log("La araña recibió " + dano + " de daño. Vida restante: " + vida);
        ActualizarBarraVida();

        if (vida <= 0)
        {
            Morir();
        }
    }

    private void Morir()
    {
        if (estaMuerta) return; // Asegura que solo se ejecute una vez

        estaMuerta = true; // Marca que la araña está muerta
        animator.SetTrigger("Death"); // Reproduce la animación de muerte
        Debug.Log("¡La araña ha muerto!");

        // Desactiva el collider y detiene toda lógica
        ataqueCollider.enabled = false;

        // Cambia a la escena de créditos tras 5 segundos
        Invoke("CambiarEscenaCreditos", 5f);
    }

    private void CambiarEscenaCreditos()
    {
        if (!string.IsNullOrEmpty(escenaCreditos))
        {
            SceneManager.LoadScene(escenaCreditos);
        }
        else
        {
            Debug.LogWarning("El nombre de la escena de créditos no está configurado en el inspector.");
        }
    }

    private void ActualizarBarraVida()
    {
        float porcentajeVida = (float)vida / 50f;
        barraVida.fillAmount = porcentajeVida;
        Debug.Log("Barra de vida actualizada. Vida: " + vida + " / " + 50 + " | Relleno: " + porcentajeVida);
    }
}