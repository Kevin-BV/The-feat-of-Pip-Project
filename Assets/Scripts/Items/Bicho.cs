using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bicho : MonoBehaviour
{
    [Header("Configuraci�n")]
    public float velocidadEscapar = 5f;
    public float rangoDeteccion = 5f;
    public Animator anim;

    [Header("Sonidos")]
    public AudioSource audioSource; // Componente de AudioSource
    public AudioClip sonidoAlerta; // Sonido al estar en alerta
    public AudioClip sonidoCorrer; // Sonido mientras escapa
    public AudioClip sonidoConsumido; // Sonido al ser consumido por el jugador

    [Header("Gizmo")]
    public Color colorGizmo = Color.yellow;

    private Transform jugador;
    private bool jugadorDetectado = false;
    private bool enAlerta = false;
    private bool escapando = false;
    private bool mirandoDerecha = true;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            jugador = playerObj.transform;
        }
    }

    void Update()
    {
        if (jugador == null) return;

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

        // Reproducir sonido de alerta
        if (audioSource && sonidoAlerta)
            audioSource.PlayOneShot(sonidoAlerta);

        anim.SetTrigger("Alerta");
        yield return new WaitForSeconds(0.5f);

        anim.SetTrigger("Escapar");
        escapando = true;

        // Reproducir sonido de correr mientras escapa
        if (audioSource && sonidoCorrer)
            audioSource.loop = true;
        audioSource.clip = sonidoCorrer;
        audioSource.Play();

        while (jugadorDetectado)
        {
            Escapar();
            yield return null;
        }

        // Detener sonido de correr
        if (audioSource && audioSource.clip == sonidoCorrer)
            audioSource.Stop();

        escapando = false;
        anim.SetTrigger("Idle");
        enAlerta = false;
    }

    private void Escapar()
    {
        Vector3 direccionContraria = (transform.position - jugador.position).normalized;
        direccionContraria.y = 0;

        Girar(direccionContraria.x);
        transform.position += direccionContraria * velocidadEscapar * Time.deltaTime;
    }

    private void Girar(float direccionX)
    {
        if (direccionX < 0 && mirandoDerecha)
        {
            mirandoDerecha = false;
            Voltear();
        }
        else if (direccionX > 0 && !mirandoDerecha)
        {
            mirandoDerecha = true;
            Voltear();
        }
    }

    private void Voltear()
    {
        Vector3 escala = transform.localScale;
        escala.x *= -1;
        transform.localScale = escala;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            VidaConCorazones vidaJugador = other.GetComponent<VidaConCorazones>();
            if (vidaJugador != null)
            {
                vidaJugador.Curar(1); // Esto activar� autom�ticamente el sonido desde `VidaConCorazones`
                Debug.Log("El jugador ha recuperado 1 de vida.");
            }

            // Destruir al bicho
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = colorGizmo;
        Gizmos.DrawWireSphere(transform.position, rangoDeteccion);
    }
}
