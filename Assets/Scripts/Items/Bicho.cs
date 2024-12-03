using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bicho : MonoBehaviour
{
    [Header("Configuración")]
    public float velocidadEscapar = 5f; // Velocidad del bicho al escapar
    public float rangoDeteccion = 5f; // Rango del gizmo para detectar al jugador
    public Animator anim; // Animator para las animaciones ("Idle", "Alerta", "Escapar")

    [Header("Gizmo")]
    public Color colorGizmo = Color.yellow; // Color del gizmo

    private Transform jugador; // Referencia al jugador
    private bool jugadorDetectado = false; // Indica si el jugador está dentro del rango
    private bool enAlerta = false; // Controla si el bicho está en la animación de "Alerta"
    private bool escapando = false; // Controla si el bicho está escapando
    private bool mirandoDerecha = true; // Dirección del bicho (true = derecha, false = izquierda)

    void Start()
    {
        // Buscar al jugador por su tag "Player"
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            jugador = playerObj.transform;
        }
    }

    void Update()
    {
        if (jugador == null) return;

        // Comprobar si el jugador está dentro del rango de detección
        float distancia = Vector3.Distance(transform.position, jugador.position);
        jugadorDetectado = distancia <= rangoDeteccion;

        if (jugadorDetectado && !enAlerta)
        {
            StartCoroutine(ActivarAlerta());
        }
    }

    private IEnumerator ActivarAlerta()
    {
        enAlerta = true;

        // Reproducir animación de "Alerta"
        anim.SetTrigger("Alerta");
        yield return new WaitForSeconds(0.5f);

        // Cambiar a la animación de "Escapar"
        anim.SetTrigger("Escapar");
        escapando = true;

        // Iniciar movimiento en dirección contraria al jugador
        while (jugadorDetectado)
        {
            Escapar();
            yield return null;
        }

        // Si el jugador ya no está detectado, volver a "Idle"
        escapando = false;
        anim.SetTrigger("Idle");
        enAlerta = false;
    }

    private void Escapar()
    {
        // Calcular dirección contraria al jugador
        Vector3 direccionContraria = (transform.position - jugador.position).normalized;

        // Asegurarse de que el movimiento solo sea en X y Z
        direccionContraria.y = 0;

        // Rotar el bicho hacia la dirección del movimiento
        Girar(direccionContraria.x);

        // Mover al bicho
        transform.position += direccionContraria * velocidadEscapar * Time.deltaTime;
    }

    private void Girar(float direccionX)
    {
        // Si el bicho está moviéndose hacia la izquierda y no está mirando a la izquierda
        if (direccionX < 0 && mirandoDerecha)
        {
            mirandoDerecha = false;
            Voltear();
        }
        // Si el bicho está moviéndose hacia la derecha y no está mirando a la derecha
        else if (direccionX > 0 && !mirandoDerecha)
        {
            mirandoDerecha = true;
            Voltear();
        }
    }

    private void Voltear()
    {
        // Invertir la escala en el eje X
        Vector3 escala = transform.localScale;
        escala.x *= -1;
        transform.localScale = escala;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Si colisiona con el jugador
        if (other.CompareTag("Player"))
        {
            // Recuperar 1 de vida al jugador
            VidaConCorazones vidaJugador = other.GetComponent<VidaConCorazones>();
            if (vidaJugador != null)
            {
                vidaJugador.Curar(1);
                Debug.Log("El jugador ha recuperado 1 de vida.");
            }

            // Destruir al bicho
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Dibujar el rango de detección
        Gizmos.color = colorGizmo;
        Gizmos.DrawWireSphere(transform.position, rangoDeteccion);
    }
}