using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Avispa : MonoBehaviour
{
    public float velocidadMovimiento = 3f; // Velocidad de movimiento de la avispa
    public float rangoDeDeteccion = 5f; // Rango del gizmo para detectar al jugador
    public float rangoDeAcercamiento = 1.5f; // Distancia m�nima a la que se detiene del jugador
    public Transform visual; // Objeto visual para animaciones
    public Animator anim; // Animator para las animaciones ("idle", "fly", "attack")

    [Header("Configuraci�n del Gizmo")]
    public float tamanoGizmo = 1f; // Rango de ataque

    [Header("Sistema de Vida")]
    public int vidaMaxima = 3; // Vida m�xima
    private int vidaActual; // Vida actual

    private Transform jugador; // Referencia al jugador
    private bool mirandoDerecha = true; // Control de direcci�n

    private bool estaSiguiendo = false; // Controla si est� siguiendo al jugador
    private bool puedeAtacar = true; // Controla si puede atacar

    public AudioClip buzz;
    public AudioClip buzzhurt;
    private AudioSource audioSource;

    private bool estaReproduciendoBuzz = false; // Para evitar que el sonido de vuelo se superponga

    void Start()
    {
        vidaActual = vidaMaxima;

        // Buscar al jugador en la escena usando la etiqueta "Player"
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            jugador = playerObj.transform;
        }

        // Configurar el AudioSource
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (vidaActual <= 0) return; // Si est� muerta, no hace nada m�s

        if (jugador == null)
        {
            anim.SetBool("isFlying", false);
            audioSource.Stop(); // Detener sonido si no est� persiguiendo
            estaReproduciendoBuzz = false;
            return;
        }

        // Detectar al jugador en el rango horizontal (X)
        float distanciaX = Mathf.Abs(transform.position.x - jugador.position.x);
        estaSiguiendo = distanciaX <= rangoDeDeteccion;

        if (estaSiguiendo)
        {
            SeguirJugador();
        }

        // Atacar si est� en rango (basado en la distancia horizontal y vertical)
        float distanciaXZ = Vector3.Distance(new Vector3(transform.position.x, 0f, transform.position.z),
                                             new Vector3(jugador.position.x, 0f, jugador.position.z));
        if (distanciaXZ <= tamanoGizmo && puedeAtacar)
        {
            AtacarJugador();
        }
    }

    private void SeguirJugador()
    {
        Vector3 direccion = new Vector3(jugador.position.x - transform.position.x, 0f, jugador.position.z - transform.position.z).normalized;

        float distanciaXZ = Vector3.Distance(new Vector3(transform.position.x, 0f, transform.position.z),
                                             new Vector3(jugador.position.x, 0f, jugador.position.z));

        if (distanciaXZ > rangoDeAcercamiento)
        {
            // Movimiento solo en X y Z
            transform.position += new Vector3(direccion.x, 0f, direccion.z) * velocidadMovimiento * Time.deltaTime;

            anim.SetBool("isFlying", true);

            // Girar si cambia de direcci�n
            if ((direccion.x > 0 && !mirandoDerecha) || (direccion.x < 0 && mirandoDerecha))
            {
                Girar();
            }

            // Reproducir sonido de vuelo si no se est� reproduciendo
            if (!estaReproduciendoBuzz)
            {
                audioSource.clip = buzz;
                audioSource.loop = true; // Bucle para el sonido de vuelo
                audioSource.Play();
                estaReproduciendoBuzz = true;
            }
        }
        else
        {
            anim.SetBool("isFlying", false);

            // Detener sonido de vuelo si se detiene
            if (estaReproduciendoBuzz)
            {
                audioSource.Stop();
                estaReproduciendoBuzz = false;
            }
        }
    }

    private void AtacarJugador()
    {
        anim.SetTrigger("Attack");
        VidaConCorazones vidaJugador = jugador.GetComponent<VidaConCorazones>();
        if (vidaJugador != null)
        {
            vidaJugador.RecibirDano(1);
        }

        StartCoroutine(CooldownAtaque());
    }

    private IEnumerator CooldownAtaque()
    {
        puedeAtacar = false;
        yield return new WaitForSeconds(1f); // Cooldown de ataque
        puedeAtacar = true;
    }

    private void Girar()
    {
        mirandoDerecha = !mirandoDerecha;
        Vector3 nuevaEscala = visual.localScale;
        nuevaEscala.x *= -1;
        visual.localScale = nuevaEscala;
    }

    public void RecibirDano(int cantidad)
    {
        if (vidaActual <= 0) return;
        vidaActual -= cantidad;
        Debug.Log($"Avispa recibi� {cantidad} de da�o. Vida restante: {vidaActual}");

        // Reproducir sonido de da�o
        audioSource.Stop(); // Detener cualquier otro sonido
        audioSource.clip = buzzhurt;
        audioSource.loop = false; // Sonido de da�o no necesita bucle
        audioSource.Play();

        if (vidaActual <= 0)
        {
            Morir();
        }
    }

    private void Morir()
    {
        anim.SetTrigger("Die");
        velocidadMovimiento = 0;
        Destroy(gameObject, 5f); // Destruir despu�s de 5 segundos
    }

    private void OnTriggerEnter(Collider other)
    {
        // Detectar colisiones con objetos que tengan un tag de "Ataque"
        if (other.CompareTag("PlayerAttack"))
        {
            // Opcional: Obtener el da�o del objeto que colision�
            int da�oRecibido = 1; // Cambiar si el da�o viene desde otro componente
            RecibirDano(da�oRecibido);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(new Vector3(transform.position.x, 0f, transform.position.z), rangoDeDeteccion); // Rango de detecci�n solo en X/Z

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(new Vector3(transform.position.x, 0f, transform.position.z), tamanoGizmo); // Rango de ataque
    }
}
