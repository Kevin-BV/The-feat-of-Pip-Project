using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rata : MonoBehaviour
{
    public float velocidadMovimiento = 3f; // Velocidad de movimiento de la rata
    public float rangoDeDeteccion = 5f; // Rango del gizmo para detectar al jugador
    public float rangoDeAcercamiento = 1.5f; // Distancia mínima a la que se detiene del jugador
    public Transform visual; // Objeto visual para animaciones
    public Animator anim; // Animator para las animaciones ("idle", "walk", "attack", "death")

    [Header("Configuración del Gizmo")]
    public float tamanoGizmo = 1f; // Rango de ataque

    [Header("Sistema de Vida")]
    public int vidaMaxima = 4; // Vida máxima
    private int vidaActual; // Vida actual

    private Transform jugador; // Referencia al jugador
    private bool mirandoDerecha = true; // Control de dirección

    private bool estaSiguiendo = false; // Controla si está siguiendo al jugador
    private bool puedeAtacar = true; // Controla si puede atacar

    private Vector3 posicionAnterior; // Para verificar si hay movimiento

    [Header("Sistema de Sonido")]
    public AudioClip hiss; // Audio al caminar
    public AudioClip hisshurt; // Audio al recibir daño
    private AudioSource audioSource;

    void Start()
    {
        vidaActual = vidaMaxima;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            jugador = playerObj.transform;
        }

        posicionAnterior = transform.position; // Inicializar la posición previa

        // Inicializar el componente AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("No se encontró un AudioSource en el GameObject de la Rata.");
        }
    }

    void Update()
    {
        if (vidaActual <= 0) return;

        if (jugador == null)
        {
            anim.SetBool("Walk", false);
            return;
        }

        float distanciaX = Mathf.Abs(transform.position.x - jugador.position.x); // Solo mide distancia en X
        estaSiguiendo = distanciaX <= rangoDeDeteccion;

        if (estaSiguiendo)
        {
            SeguirJugador();
        }

        float distanciaXZ = Vector3.Distance(new Vector3(transform.position.x, 0f, transform.position.z),
                                             new Vector3(jugador.position.x, 0f, jugador.position.z));
        if (distanciaXZ <= tamanoGizmo && puedeAtacar)
        {
            AtacarJugador();
        }

        VerificarMovimiento();
    }

    private void SeguirJugador()
    {
        Vector3 direccion = new Vector3(jugador.position.x - transform.position.x, 0f, jugador.position.z - transform.position.z).normalized;

        float distanciaXZ = Vector3.Distance(new Vector3(transform.position.x, 0f, transform.position.z),
                                             new Vector3(jugador.position.x, 0f, jugador.position.z));

        if (distanciaXZ > rangoDeAcercamiento)
        {
            transform.position += new Vector3(direccion.x, 0f, direccion.z) * velocidadMovimiento * Time.deltaTime;

            if ((direccion.x > 0 && !mirandoDerecha) || (direccion.x < 0 && mirandoDerecha))
            {
                Girar();
            }
        }
    }

    private void VerificarMovimiento()
    {
        if (transform.position != posicionAnterior)
        {
            anim.SetBool("Walk", true);

            // Reproducir sonido al caminar si no está ya sonando
            if (!audioSource.isPlaying)
            {
                audioSource.clip = hiss;
                audioSource.loop = true; // Loop para el sonido de caminar
                audioSource.Play();
            }
        }
        else
        {
            anim.SetBool("Walk", false);

            // Detener el sonido si no está caminando
            if (audioSource.clip == hiss && audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }

        posicionAnterior = transform.position;
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
        Debug.Log($"Rata recibió {cantidad} de daño. Vida restante: {vidaActual}");

        // Reproducir sonido al recibir daño
        audioSource.PlayOneShot(hisshurt);

        if (vidaActual <= 0)
        {
            Morir();
        }
    }

    private void Morir()
    {
        anim.SetTrigger("Death");
        velocidadMovimiento = 0;

        // Detener cualquier sonido restante
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        Destroy(gameObject, 5f); // Destruir después de 5 segundos
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(new Vector3(transform.position.x, 0f, transform.position.z), rangoDeDeteccion);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(new Vector3(transform.position.x, 0f, transform.position.z), tamanoGizmo);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerAttack"))
        {
            RecibirDano(1); // Ajusta el daño según sea necesario
        }
    }
}
